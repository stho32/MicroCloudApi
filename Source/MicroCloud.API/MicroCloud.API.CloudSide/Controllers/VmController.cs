using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroCloud.API.CloudSide.Controllers
{
    /// <summary>
    /// The cloud side VM api is used by the virtual machines to declare their state and ip to the cloud.
    /// </summary>
    public class VmController : Controller
    {
        /// <summary>
        /// VMs are calling this method regulary to tell the cloud their cloud-internal IP address.
        /// We need that for a lot of stuff.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult IAm(string name)
        {
            return Json(new { });
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