using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryForce.Shared.Models;
using Tewr.Blazor.FileReader;

namespace StoryForce.Client.ViewModels
{
    public class UploadFile
    {
        public UploadFile(IFileReference fileReference, StoryFile storyFile)
        {
            this.FileReference = fileReference;
            this.StoryFile = storyFile;
        }
        public IFileReference FileReference { get; private set; }

        public StoryFile StoryFile { get; private set; }

        public decimal Percentage { get; set; }
    }

    public static class MyExtensions
    {
        public static string ToFileSizeString(this long size)
        {
            string[] suf = { "b", "kb", "mb", "gb", "tb", "pb", "eb" }; //Longs run out around EB
            if (size == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(size);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(size) * num) + suf[place];
        }
    }
}
