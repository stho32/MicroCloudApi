using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroCloud.API.ClientSide.Controllers
{
    /// <summary>
    /// On the client side we need to create vms, interact with them and work with their results.
    /// Also we need ways to list and remove old stuff in case we have created a mess.
    /// </summary>
    public class VmController : Controller
    {
        /// <summary>
        /// List all VMs with their current state and stuff (cloud internal ip ...)
        /// </summary>
        /// <returns></returns>
        public JsonResult List(string apiKey)
        {
            return Json(new { });
        }

        /// <summary>
        /// Create a new VM
        /// </summary>
        /// <returns></returns>
        public JsonResult New(string apiKey, string baseImage, string parametersJson)
        {
            return Json(new { });
        }

        /// <summary>
        /// Grab the state of a vm
        /// </summary>
        /// <returns></returns>
        public JsonResult Index(string apiKey, string vmname)
        {
            return Json(new { });
        }

        /// <summary>
        /// Shutdown and remove a vm (or just remove it)
        /// </summary>
        /// <param name="vmname"></param>
        /// <returns></returns>
        public JsonResult RemoveVm(string apiKey, string vmname)
        {
            return Json(new { });
        }
    }
}