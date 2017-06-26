using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wechat.Helper;
using WeChat.Helper;

namespace WeChat.Controllers
{
    public class HomeController : Controller
    {

        public static int sum = 0;

        public static string openid;//用户的唯一标识
        public static string nickname;//用户昵称
        public static string sex;//用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        public static string province;//用户个人资料填写的省份
        public static string city;//普通用户个人资料填写的城市
        public static string country;//国家，如中国为CN
        public static string headimgurl;//用户头像
        public static string privilege;//用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        public static string unionid;//只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。

        private static userInfo usInfo;


        //
        // GET: /Home/
        //第一步：用户同意授权，获取code
        public ActionResult Index()
        {
            int flag = 0;
            string code = Request.QueryString["code"];
            string state = Request.QueryString["state"];
            if (code != "" && state != null && flag==0)
            {
                flag = 1;
                System.Diagnostics.Debug.Write("<<<##############11111111111111111111111111#######");
                at(code);//拿code获取token跟openId
                //System.Diagnostics.Debug.Write(usInfo.openid + "能拿到吗？？？？？？？？？？？？？？？？？？");
                //System.Diagnostics.Debug.Write(usInfo.nickname + "能拿到吗？？？？？？？？？？？？？？？？？？");

                ViewBag.Message_ViewBag = usInfo.nickname;
                

            }

            //ViewBag.Message_ViewBag = nickname;
            


            //@Html.Encode(ViewBag.Message_ViewBag)

            return View();
        }

        //第二步：通过code换取网页授权access_token
        public void at(string code)
        {
            string AppID = "wxf924bae337057367";
            string AppSecret = "b4ee86400225d725683e147374fed7ef";
            string strUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            
            /*
            strUrl = string.Format(strUrl, AppID, AppSecret, code);
            string jsonStr = HttpCrossDomain.Get(strUrl);
            System.Diagnostics.Debug.Write(jsonStr+"111111111111111111111111");
            System.Diagnostics.Debug.Write(Tools.GetJsonValue(jsonStr, "openid")+"22222222222222222222222222");
            */

            
            strUrl = string.Format(strUrl, AppID, AppSecret, code);
            var request = (HttpWebRequest)WebRequest.Create(strUrl);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            atoken at = JsonConvert.DeserializeObject<atoken>(responseString);
            rt(AppID,at.refresh_token);
        }


        //第三步：刷新refresh_token（如果需要）
        public void rt(string AppID,string refresh_token)
        {
            //string AppID = "wxf924bae337057367";
            string strUrl = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}";
            strUrl = string.Format(strUrl, AppID, refresh_token);
            var request = (HttpWebRequest)WebRequest.Create(strUrl);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            rtoken rt = JsonConvert.DeserializeObject<rtoken>(responseString);
            gUserInfo(rt.access_token, rt.openid);

        }


        //第四步：拉取用户信息(需scope为 snsapi_userinfo)
        [HttpGet]
        public void gUserInfo(string access_token, string openId)
        {
            sum=sum+1;

            if (access_token != null)
            {
                string strUrl = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
                strUrl = string.Format(strUrl, access_token, openId);
   
                string jsonStr = HttpCrossDomain.Get(strUrl);
                System.Diagnostics.Debug.Write(jsonStr+"111111111111111111111111");


                //nickname = Tools.GetJsonValue(jsonStr, "nickname");

                usInfo = new userInfo();
                usInfo.openid = Tools.GetJsonValue(jsonStr, "openid");
                usInfo.nickname = Tools.GetJsonValue(jsonStr, "nickname");
                usInfo.sex = Tools.GetJsonValue(jsonStr, "sex");
                usInfo.province = Tools.GetJsonValue(jsonStr, "province");
                usInfo.city = Tools.GetJsonValue(jsonStr, "city");
                usInfo.country = Tools.GetJsonValue(jsonStr, "country");
                usInfo.headimgurl = Tools.GetJsonValue(jsonStr, "headimgurl");
                usInfo.privilege = Tools.GetJsonValue(jsonStr, "privilege");
                usInfo.unionid = Tools.GetJsonValue(jsonStr, "unionid");


                //System.Diagnostics.Debug.Write(Tools.GetJsonValue(jsonStr, "city")+"22222222222222222222222222");
                //System.Diagnostics.Debug.Write(Tools.GetJsonValue(jsonStr, "nickname") + "333333333333333333");
                //return View();


                /*
               var request = (HttpWebRequest)WebRequest.Create(strUrl);
               var response = (HttpWebResponse)request.GetResponse();
               var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

               System.Diagnostics.Debug.Write("====================" + responseString + "#####################");

               userInfo user = JsonConvert.DeserializeObject<userInfo>(responseString);
                */
                //System.Diagnostics.Debug.Write("nickname:" + user.nickname + "<<<#####################");
                //System.Diagnostics.Debug.Write("city:" + user.city + "<<<#####################");

                
            }
            else
            {
                System.Diagnostics.Debug.Write("没有拿到access_token:"+sum);
                //return View();
            }

            //Response.Redirect(gUserInfo);

            
        }


        /*
       [HttpPost]
       public ActionResult Login(FormCollection form)
       {
           var number = form["number"];
           var password = form["password"];
           System.Diagnostics.Debug.Write(number+"======================");
           System.Diagnostics.Debug.Write(password);
           return View();
       }
        */

        class atoken
        {
            public string access_token { set; get; }
            public string expires_in { set; get; }
            public string refresh_token { set; get; }
            public string openid { set; get; }
            public string scope { set; get; }
            
        }

        class rtoken
        {
            public string access_token { set; get; }
            public string expires_in { set; get; }
            public string refresh_token { set; get; }
            public string openid { set; get; }
            public string scope { set; get; }

        }

        class userInfo
        {
            public string openid { set; get; }
            public string nickname { set; get; }
            public string sex { set; get; }
            public string province { set; get; }
            public string city { set; get; }
            public string country { set; get; }
            public string headimgurl { set; get; }
            public string privilege { set; get; }
            public string unionid { set; get; }

        }

    }
}
