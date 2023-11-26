using Amazon.Auth.AccessControlPolicy;
using ChatRoom.Hubs;
using ChatRoom.Models;
using ChatRoom.Services.FileConverter;
using ChatRoom.Services.MessageDocumentService;
using ChatRoom.Services.MessagePaaswordService;
using ChatRoom.Services.MessageService;
using ChatRoom.Services.PersonService;
using ChatRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NAudio.Wave;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ChatRoom.Controllers
{
    [AllowAnonymous]
    public class UploaderController : Controller
    {
        private IWebHostEnvironment hostingEnvironment;
		private readonly IHubContext<ChatHub> _hubContext;
        private readonly IMessageService messageService;
        private readonly IMessageDocumentService messageDocumentService;
        private readonly IPersonService personService;
        private readonly IMessagePaaswordService messagePaaswordService;
		private readonly IFileConverterService fileConverterService;
		public UploaderController(IWebHostEnvironment hostingEnvironment, IHubContext<ChatHub> hubContext, IMessageService messageService, IMessageDocumentService messageDocumentService, IPersonService personService, IMessagePaaswordService messagePaaswordService, IFileConverterService fileConverterService)
		{
			this.hostingEnvironment = hostingEnvironment;
			_hubContext = hubContext;
			this.messageService = messageService;
			this.messageDocumentService = messageDocumentService;
			this.personService = personService;
			this.messagePaaswordService = messagePaaswordService;
			this.fileConverterService = fileConverterService;
		}
		public async Task<IActionResult> Index()
        {
        
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(IList<IFormFile> files , string caption ,Guid recvId)
        {
            var _user = await GetUser();


			long totalBytes = files.Sum(f => f.Length);
           
            string filename =string.Empty;
            byte[] body=null;
           
			foreach (IFormFile source in files)
            {
                filename= ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.ToString().Trim('"');

                filename = this.EnsureCorrectFilename(filename);
              
                byte[] buffer = new byte[16 * 1024];
				
				using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                {
                    using (Stream input = source.OpenReadStream())
                    {
                        long totalReadBytes = 0;
                        int readBytes;

                        while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            if (Program.CanselSendFile) break;
                            await output.WriteAsync(buffer, 0, readBytes);
                            totalReadBytes += readBytes;
                             Program.counter= (int)((float)totalReadBytes / (float)totalBytes * 100.0);
						 



						}
					 
						body =await fileConverterService.Convert(source);
					}
                 

                }
            }
            var id = Guid.NewGuid();
            var time = DateTime.Now;
 
            var encript =await messagePaaswordService.Encript(new ViewModels.MessageEncriptViewModel
            {
                body=   string.Empty, MessageType= MessageType.MessageDocument, Id= id, 
                recvId= recvId,
                sender=_user.Id, Status= false, Time=DateTime.Now,
                bodyByte= await fileConverterService.Convert(files[0])
            });

            await messageDocumentService.CreateAsync(new MessageDocument()
            {

                Document = encript.Value.Key,
                DocumentType = GetDocumentType(filename),
                id = id,
                MessageId = id,
                Time = encript.Value.Value ? DateTime.Now : time,
                Id = id,
            }); 
            await Task.Delay(1 * 2000);
            await  _hubContext.Clients.User(_user.Id.ToString()).SendAsync("messageMedai",recvId,  caption ,   id );

        

			return this.Content("success");
        }
        [HttpPost]
        public  void CanselSendFile()
        {
            Program.CanselSendFile = true;
        }


		public  int  Progress()
        {
            int c = Program.counter;
            return c;
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            string path = this.hostingEnvironment.WebRootPath + "\\uploads\\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path + filename;
        }


       
        public IActionResult Download()
        {
            return View();
        }

        public async Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
            }

            var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
            var canReportProgress = total != -1 && progress != null;

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var totalRead = 0L;
                var buffer = new byte[4096];
                var isMoreToRead = true;

                do
                {
                    token.ThrowIfCancellationRequested();

                    var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);

                    if (read == 0)
                    {
                        isMoreToRead = false;
                    }
                    else
                    {
                        var data = new byte[read];
                        buffer.ToList().CopyTo(0, data, 0, read);

                        // TODO: put here the code to write the file to disk

                        totalRead += read;

                        if (canReportProgress)
                        {
                            progress.Report((totalRead * 1d) / (total * 1d) * 100);
                        }
                    }
                } while (isMoreToRead);
            }
        }
         public async  Task d()
        {
            var progress = new  Progress<double>();
            progress.ProgressChanged += (sender, value) => System.Console.Write("\r%{0:N0}", value);

            var cancellationToken = new CancellationTokenSource();

            await DownloadFileAsync("http://www.dotpdn.com/files/Paint.NET.3.5.11.Install.zip", progress, cancellationToken.Token);
           
        }

        public async Task<Person> GetUser() =>await personService.GetByUserName(User.Identity.Name);

        private string GetExtionFile(string fileName)
                => System.IO.Path.GetExtension(fileName);
        

        private DocumentType GetDocumentType(string filename)
        {
            string ext = GetExtionFile(filename);

			string doc = string.Empty;
            switch (ext)
            {
                case ".mp3":
               return DocumentType.Mp3;
				case ".mp4":
					return DocumentType.Video;
				case ".jpg":
				case ".png":
				case ".gif":
				case ".tif":
					return DocumentType.Image;
				case ".pdf":
					return DocumentType.Pdf;
				case "txt":
				case ".text":
					return DocumentType.Text;
				
				case ".zip":
					return DocumentType.Zip;
				case ".rar":
					return DocumentType.Rar;

                default:return DocumentType.Unknown;

			}
           
        }



 

	}
}
