
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.Xml.Linq;

namespace Utility
{
    public static class ExtensionFile
    {
        public static DocumentType GetExtension(string path)
        {
            //              string[] mediaExtensions = {
            //    ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", //etc
            //    ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", //etc
            //    ".AVI", ".MP4", ".DIVX", ".WMV", //etc
            //};

            string ext = Path.GetExtension(path);
            switch (ext)
            {
                case ".mp3":
                    return DocumentType.Mp3;
                case ".mp4":
                    return DocumentType.Video;
                case ".pdf":
                    return DocumentType.Pdf;
                case ".jpg":
                case ".png":
                case ".gif":
                    return DocumentType.Img;

                default: return DocumentType.Unknown;
            }
        }
    }


    public enum DocumentType
    {
        [Display(Name = "ویدئو")]
        Video,
        [Display(Name = "فایل")]
        Mp3,
        [Display(Name = "pdf")]
        Pdf,
        [Display(Name = "تصویر")]
        Img,
        [Display(Name = "ناشناخته")]
        Unknown,



    }

}