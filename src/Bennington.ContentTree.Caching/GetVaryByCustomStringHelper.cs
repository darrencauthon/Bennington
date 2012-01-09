using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Bennington.ContentTree.Caching
{
    public class GetVaryByCustomStringHelper
    {
        public static string GetVaryByCustomString(HttpContext httpContext, string varyByCustomOutputCacheDirectiveArgument)
        {
            if (varyByCustomOutputCacheDirectiveArgument == "Browser") return null;

            var prefix = string.Empty;

            // no output caching for draft mode
            if (((httpContext.Request.QueryString["VersionType"] ?? string.Empty) == "Draft") || (httpContext.Request.RawUrl.Contains("@Draft"))) return Guid.NewGuid().ToString();

            var stringBuilder = new StringBuilder(prefix);
            var cacheKeys = GetCacheKeys(varyByCustomOutputCacheDirectiveArgument);
            foreach (var cacheKey in cacheKeys)
            {
                stringBuilder.Append(GetStringFromApplicationSession(httpContext, cacheKey));
            }

            if (stringBuilder.Length == 0)
            {
                return null;
            }
            return stringBuilder.ToString();
        }

        private static string GetStringFromApplicationSession(HttpContext context, string sessionKey)
        {
            if (!context.Application.AllKeys.Where(a => a == sessionKey).Any())
            {
                context.Application[sessionKey] = Guid.NewGuid().ToString();
            }
            return context.Application[sessionKey].ToString();
        }

        private static IEnumerable<string> GetCacheKeys(string arg)
        {
            return arg.Split('|').OrderBy(a => a);
        }
    }
}
