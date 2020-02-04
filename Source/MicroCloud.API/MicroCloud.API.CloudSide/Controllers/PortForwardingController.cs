using System;
using System.Web.Mvc;
using MicroCloud.API.BL.Repositories;
using MicroCloud.API.CloudSide.App_Code;

namespace MicroCloud.API.CloudSide.Controllers
{
    public class PortForwardingController : Controller
    {
        protected RepositoryFactory repositoryFactory = new RepositoryFactory(
            new ConfigurationProvider());


        /// <summary>
        /// When the vm is up and running it can add port forwardings.
        /// It simply declares, that the port should be "open" which means that a port forwarding on the entrance router should be created
        /// </summary>
        /// <param name="name">The name of the virtual machine which acts as an access key</param>
        /// <param name="myPort">the port of the virtual machine which should be forwarded</param>
        /// <returns></returns>
        public JsonResult Add(string name, int myPort)
        {
            try
            {
                var vmRepository = repositoryFactory.VmRepository();
                var portForwardingRepository = repositoryFactory.PortForwardingRepository();

                var vm = vmRepository.GetByName(name);
                if (vm != null)
                {
                    var portForwarding = portForwardingRepository.GetByPort(myPort);
                    if (portForwarding == null)
                    {
                        portForwardingRepository.Add(vm.Id, myPort);
                    }
                    return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { result = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}