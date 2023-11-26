 

namespace Utility
{
	public static class MediaGenerator
	{
		public static string Generator(byte[] bytes,ChatRoom.Models. DocumentType type)
		{

	 
			string doc=string.Empty;
			string filesrc=string.Empty;
			try
			{

			filesrc=	$"data:image/jpeg;base64," + Convert.ToBase64String(bytes, 0, bytes.Length);
				switch (type)
				{
					case ChatRoom.Models.DocumentType.Mp3:
						doc = $"<audio controls>\r\n          " +
								$"  <source src={filesrc} type=\"audio/mpeg\">\r\n  " +
								"          Your browser does not support the audio tag.\r\n       " +
								" </audio>";
						break;

					case ChatRoom.Models.DocumentType.Video:
						doc = "<video width=\"320\" height=\"240\" style=\"width:100%; object-fit:fill; \" controls>\r\n    " +
							$"<source src={filesrc} type=\"video/mp4\">\r\n" +
							"Your browser does not support the video tag.\r\n</video>";
						break;
	        case ChatRoom.Models.DocumentType.Image:
						doc = $"<img src={filesrc} />";
						break;

					default:
						doc = $"<a href='{filesrc}'>download</a>";
						break;



				} }
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);

			}




			return doc;
		}

	}
}
