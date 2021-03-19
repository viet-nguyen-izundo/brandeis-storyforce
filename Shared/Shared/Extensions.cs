using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryForce.Shared
{
    public static class Extensions
    {
        public static bool IsPathOrFileValid(this string name)
        {
            return (name != null &&
                    name.IndexOfAny(Constants.InvalidFileNameChars) == -1 &&
                    !Constants.InvalidFileNameEndingChars.Any(name.EndsWith) &&
                    !Constants.ReservedDevices.Split(',').Contains(name.ToUpper().Split('.')[0]));
        }

        public static string GetFileNameWithoutTimeStamp(this string fileName)
        {
            if (!fileName.Contains("-"))
            {
                return fileName;
            }

            var index = fileName.IndexOf("-", StringComparison.InvariantCulture);
            return fileName.Substring(index + 1, (fileName.Length - index - 1));
        }
    }
}
