using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MicroCloud.API.BL.Repositories;

namespace MicroCloud.API.ClientSide.Controllers
{
    /// <summary>
    /// On the client side we need to create vms, interact with them and work with their results.
    /// Also we need ways to list and remove old stuff in case we have created a mess.
    /// </summary>
    public class VmController : Controller
    {
        protected RepositoryFactory repositoryFactory = new RepositoryFactory(new ConfigurationProvider());

        /// <summary>
        /// List all VMs with their current state and stuff (cloud internal ip ...)
        /// </summary>
        /// <returns></returns>
        public JsonResult Index(string apiKey)
        {
            try
            {
                var apiKeyRepository = repositoryFactory.ApiKeyRepository();
                var apiKeyId = apiKeyRepository.GetApiKeyIdByCode(apiKey);

                var vmRepository = repositoryFactory.VmRepository();
                var vms = vmRepository.GetByApiKey(apiKeyId);

                return Json(vms, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Create a new VM
        /// </summary>
        /// <returns></returns>
        public JsonResult New(string apiKey, string baseImage, int? ramInGb, string parametersJson)
        {
            try
            {
                var apiKeyRepository = repositoryFactory.ApiKeyRepository();
                var apiKeyId = apiKeyRepository.GetApiKeyIdByCode(apiKey);
                if (apiKeyId <= 0)
                {
                    return Json(new { result = "Not a valid api key." }, JsonRequestBehavior.AllowGet);
                }

                if ( !ramInGb.HasValue)
                {
                    ramInGb = 4;
                }

                var vmRepository = repositoryFactory.VmRepository();
                var vm = vmRepository.CreateForApiKey(apiKeyId, baseImage, ramInGb.Value, parametersJson);

                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Grab the state of a vm
        /// </summary>
        /// <returns></returns>
        public JsonResult ByName(string apiKey, string name)
        {
            if ( name == null )
                return Json(new { result = "Not found" }, JsonRequestBehavior.AllowGet);

            try
            {
                var apiKeyRepository = repositoryFactory.ApiKeyRepository();
                var apiKeyId = apiKeyRepository.GetApiKeyIdByCode(apiKey);

                var vmRepository = repositoryFactory.VmRepository();
                var vms = vmRepository.GetByApiKey(apiKeyId);

                var vm = vms.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

                if (vm == null)
                    return Json(new {result = "Not found"}, JsonRequestBehavior.AllowGet);

                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Shutdown and remove a vm (or just remove it)
        /// </summary>
        /// <param name="vmname"></param>
        /// <returns></returns>
        public JsonResult RemoveVm(string apiKey, string vmname)
        {
            try
            {
                var apiKeyRepository = repositoryFactory.ApiKeyRepository();
                var apiKeyId = apiKeyRepository.GetApiKeyIdByCode(apiKey);
                if (apiKeyId <= 0)
                {
                    return Json(new { result = "Not a valid api key." }, JsonRequestBehavior.AllowGet);
                }

                var vmRepository = repositoryFactory.VmRepository();
                vmRepository.RemoveVm(apiKeyId, vmname);

                return Json(new {result = "OK"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SetVmAlias(string apiKey, string vmname, string alias)
        {
            try
            {
                var apiKeyRepository = repositoryFactory.ApiKeyRepository();
                var apiKeyId = apiKeyRepository.GetApiKeyIdByCode(apiKey);
                if (apiKeyId <= 0)
                {
                    return Json(new { result = "Not a valid api key." }, JsonRequestBehavior.AllowGet);
                }

                var vmRepository = repositoryFactory.VmRepository();
                vmRepository.SetVmAlias(apiKeyId, vmname, alias);

                return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Stop(string apiKey, string vmname)
        {
            try
            {
                var apiKeyRepository = repositoryFactory.ApiKeyRepository();
                var apiKeyId = apiKeyRepository.GetApiKeyIdByCode(apiKey);
                if (apiKeyId <= 0)
                {
                    return Json(new { result = "Not a valid api key." }, JsonRequestBehavior.AllowGet);
                }

                var vmRepository = repositoryFactory.VmRepository();
                vmRepository.StopVm(apiKeyId, vmname);

                return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Start(string apiKey, string vmname)
        {
            try
            {
                var apiKeyRepository = repositoryFactory.ApiKeyRepository();
                var apiKeyId = apiKeyRepository.GetApiKeyIdByCode(apiKey);
                if (apiKeyId <= 0)
                {
                    return Json(new { result = "Not a valid api key." }, JsonRequestBehavior.AllowGet);
                }

                var vmRepository = repositoryFactory.VmRepository();
                vmRepository.StartVm(apiKeyId, vmname);

                return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}