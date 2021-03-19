using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryForce.Shared
{
    public class Constants
    {
        public const string ImageFiles = "jpg,jpeg,jpe,gif,bmp,png,ico";
        public const string UploadableFiles = ImageFiles + ",mov,wmv,avi,mp4,mp3,doc,docx,xls,xlsx,ppt,pptx,pdf,txt,zip,nupkg,csv";
        public const string ReservedDevices = "CON,NUL,PRN,COM0,COM1,COM2,COM3,COM4,COM5,COM6,COM7,COM8,COM9,LPT0,LPT1,LPT2,LPT3,LPT4,LPT5,LPT6,LPT7,LPT8,LPT9,CONIN$,CONOUT$";
        public static readonly char[] InvalidFileNameChars =
        {
            '\"', '<', '>', '|', '\0', (Char) 1, (Char) 2, (Char) 3, (Char) 4, (Char) 5, (Char) 6, (Char) 7, (Char) 8,
            (Char) 9, (Char) 10, (Char) 11, (Char) 12, (Char) 13, (Char) 14, (Char) 15, (Char) 16, (Char) 17, (Char) 18,
            (Char) 19, (Char) 20, (Char) 21, (Char) 22, (Char) 23, (Char) 24, (Char) 25, (Char) 26, (Char) 27,
            (Char) 28, (Char) 29, (Char) 30, (Char) 31, ':', '*', '?', '\\', '/'
        };
        public static readonly string[] InvalidFileNameEndingChars = { ".", " " };
    }
}
