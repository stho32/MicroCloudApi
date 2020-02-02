using MicroCloud.API.CloudSide.App_Code;
using System.Web.Mvc;
using MicroCloud.API.BL.Repositories;

namespace MicroCloud.API.CloudSide.Controllers
{
    /// <summary>
    /// The cloud side VM api is used by the virtual machines to declare their state and ip to the cloud.
    /// </summary>
    public class VmController : Controller
    {
        protected RepositoryFactory repositoryFactory = new RepositoryFactory(
            new ConfigurationProvider());

        /// <summary>
        /// VMs are calling this method regulary to tell the cloud their cloud-internal IP address.
        /// We need that for a lot of stuff.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult IAm(string name)
        {
            var vmRepository = repositoryFactory.VmRepository();
            var vm = vmRepository.GetByName(name);
            if ( vm != null )
            {
                if ( vm.CloudInternalIP != Request.UserHostAddress)
                {
                    vmRepository.SetCloudInternalIP(vm.Id, Request.UserHostAddress);
                }

                return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "VM not found" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// On the client side there may be scripts waiting for a specific state, like 
        /// "Online" or "Provisioned".
        /// </summary>
        /// <param name="name"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public JsonResult MyStateIs(string name, string state)
        {
            return Json(new { });
        }

        /// <summary>
        /// Tell the cloud that this vm would like to be removed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult KillMe(string name)
        {
            return Json(new { });
        }
    }
}