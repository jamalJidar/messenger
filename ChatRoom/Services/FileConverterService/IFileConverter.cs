using SharpCompress.Common;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Drawing2D;
using System.Drawing;
namespace ChatRoom.Services.FileConverter
{
	public interface IFileConverterService
    {
		public Task<byte[]> Convert(IFormFile file );
	}

	public class FileConverterService : IFileConverterService
	{
		public async Task<byte[]> Convert(IFormFile file)
		{
			const int MaxChunkSizeInBytes = 2048;
			var totalBytes = 0;
			byte[] fileByteArray;
			var fileByteArrayChunk = new byte[MaxChunkSizeInBytes];

			using (var stream = file.OpenReadStream())
			{
				int bytesRead;
				while ((bytesRead = stream.Read(fileByteArrayChunk, 0, fileByteArrayChunk.Length)) > 0)
				{
					totalBytes += bytesRead;
				}
				fileByteArray = new byte[totalBytes];
				stream.Seek(0, SeekOrigin.Begin);
				stream.Read(fileByteArray, 0, totalBytes);
			}
			return fileByteArray;
		}


		public async Task<MemoryStream> ConvertByteToImg(byte[] byteArrayIn)
		 => new MemoryStream(byteArrayIn);
	}


    /*
	 
	 text/plain	Plain text.
text/html	HTML.
text/xml	XML data.
text/richtext	Rich Text Format (RTF).
text/scriptlet	Windows script component.
audio/x-aiff	Audio Interchange File, Macintosh.
audio/basic	Audio file, UNIX.
audio/mid	Windows Internet Explorer 7 and later. MIDI sequence.
audio/wav	Pulse Code Modulation (PCM) Wave audio, Windows.
image/gif	Graphics Interchange Format (GIF).
image/jpeg	JPEG image.
image/pjpeg	Default type for JPEG images.
	 */




}
