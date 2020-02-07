using System;
using System.Web.Mvc;
using MicroCloud.API.BL.Repositories;

namespace MicroCloud.API.ClientSide.Controllers
{
    public class PortForwardingController : Controller
    {
        protected RepositoryFactory RepositoryFactory = new RepositoryFactory(
            new ConfigurationProvider());


        /// <summary>
        /// get a list of all port forwardings
        /// </summary>
        /// <returns></returns>
        public JsonResult Index(string apiKey, string name)
        {
            try
            {
                var apiKeyRepository = RepositoryFactory.ApiKeyRepository();
                var apiKeyId = apiKeyRepository.GetApiKeyIdByCode(apiKey);

                var vmRepository = RepositoryFactory.VmRepository();
                var vm = vmRepository.GetByName(name, apiKeyId);
                if ( vm == null )
                    return Json(new { result = "not found" }, JsonRequestBehavior.AllowGet);

                var portForwardingRepository = RepositoryFactory.PortForwardingRepository();

                var portForwardings = portForwardingRepository.GetByVm(vm.Id);

                return Json(portForwardings, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}