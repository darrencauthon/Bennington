using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Bennington.Cms.Helpers
{
    public interface ISlugGenerator
    {
        string GenerateSlug(string name);
    }

    public class SlugGenerator : ISlugGenerator
    {
        public string GenerateSlug(string name)
        {
            var workingString = name.Replace(' ', '-');
            var returnString = Regex.Replace(workingString, @"[^a-zA-Z0-9\-]", string.Empty);
            return returnString;
        }
    }
}