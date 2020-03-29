using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MicroCloud.API.BL.Repositories;

namespace MicroCloud.API.ClientSide.Controllers
{
    /// <summary>
    /// What images are available?
    /// </summary>
    public class ImageController : Controller
    {
        protected RepositoryFactory repositoryFactory = new RepositoryFactory(new ConfigurationProvider());

        /// <summary>
        /// List all available images
        /// </summary>
        /// <returns></returns>
        public JsonResult Index(string apiKey)
        {
            try
            {
                var apiKeyRepository = repositoryFactory.ApiKeyRepository();
                var apiKeyId = apiKeyRepository.GetApiKeyIdByCode(apiKey);

                var imageRepository = repositoryFactory.ImageRepository();
                var vms = imageRepository.GetByApiKey(apiKeyId);

                return Json(vms, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}