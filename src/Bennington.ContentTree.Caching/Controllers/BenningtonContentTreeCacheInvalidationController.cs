using System;
using System.Configuration;
using System.Web.Mvc;
using Bennington.ContentTree.Caching.Blades;
using Bennington.ContentTree.Caching.Models;

namespace Bennington.ContentTree.Caching.Controllers
{
    public class BenningtonContentTreeCacheInvalidationController : Controller
    {
        public ActionResult Invalidate(string guid, string cacheKey)
        {
            if (guid != ConfigurationManager.AppSettings["Bennington.ContentTree.Cahce.InvalidationUrlGuid"]) 
                return View("Invalidate", new InvalidateViewModel()
                                                {
                                                    Message = "NOT INVALIDATED!"
                                                });

            HttpContext.Application[(string) ContentTreeCacheInvalidationBlade.ContentTreeApplicationSessionKey] = Guid.NewGuid().ToString();
            return View("Invalidate", new InvalidateViewModel()
                                          {
                                              Message = string.Format("Cache successfully invalidated with cache key: {0}", cacheKey)
                                          });
        }
    }
}
