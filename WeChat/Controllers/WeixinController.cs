using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeChat.Controllers
{
    public class WeixinController : Controller
    {
        //
        // GET: /Weixin/
        /*
        public ActionResult Index()
        {
            return View();
        }*/

        public string Index()
        {
            foreach (var item in this.Request.QueryString)
            {
                //this._logger.LogInformation(item.Key + ":" + item.Value);
            }
            var token = "kaoshipai123";
            if (string.IsNullOrEmpty(token))
            {
                return string.Empty;
            }

            string echoString = this.Request.QueryString["echoStr"]; // Request.QueryString("echoStr"）;
            //string signature = this.Request.QueryString["signature"];
            //string timestamp = this.Request.QueryString["timestamp"];
            //string nonce = this.Request.QueryString["nonce"];

            return echoString;
        }

    }
}
