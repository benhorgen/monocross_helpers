using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;

using MonoCross.Navigation;

namespace MonoCross.Webkit
{
    public class MXWebkitContainer : MXContainer<MXWebkitContainer>, IMXContainer
    {
        public static void Initialize(MXApplication theApp)
        {
            MXContainer<MXWebkitContainer>.Initialize(new MXWebkitContainer(), theApp);
        }
        public static Dictionary<string, string> GetParameters(HttpRequestBase request)
        {
            Dictionary<string, string> retval = new Dictionary<string, string>();
            System.Text.StringBuilder strb = new System.Text.StringBuilder();

            if (HttpContext.Current.Session["ActionParameters"] != null && HttpContext.Current.Request.HttpMethod == "POST")
            {
                Dictionary<string, string> actionparms = (Dictionary<string, string>)HttpContext.Current.Session["ActionParameters"];
                foreach (KeyValuePair<string, string> parm in (actionparms))
                {
                    strb.Append(parm.Key + ": " + parm.Value);
                    retval.Add(parm.Key, parm.Value);
                }
                HttpContext.Current.Session.Remove("ActionParameters");
            }
            foreach (string key in request.Form.AllKeys.Where(key => !key.StartsWith("__")))
            {
                strb.Append(key + ": " + request.Form[key]);
                retval[key.Replace("rdl_", string.Empty)] = request.Form[key];
            }
            foreach (string key in request.QueryString.AllKeys.Where(key => !key.StartsWith("__")))
            {
                strb.Append(key + ": " + request.QueryString[key]);
                retval[key] = request.QueryString[key];
            }
            return retval;
        }
        public static IMXController Navigate(string url, HttpRequestBase request)
        {
            return Navigate(url, GetParameters(request));
        }
        public static void WriteControlToResponse(string viewId, string viewTitle, Control control)
        {
            string xml = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Ajax.xml"));
            xml = xml.Replace("{ViewId}", viewId);
            xml = xml.Replace("{Title}", viewTitle);
            using (System.IO.StringWriter strwriter = new System.IO.StringWriter())
            {
                using (HtmlTextWriter writer = new HtmlTextWriter(strwriter))
                {
                    control.RenderControl(writer);
                    xml = xml.Replace("{Markup}", strwriter.ToString());
                }
            }
            HttpContext.Current.Response.ContentType = "application/xml";
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Output.Write(xml);
        }
        public static void WriteJsonToResponse(string json)
        {
            HttpContext.Current.Response.Write(json);
            HttpContext.Current.Response.Flush();
        }
        public static string GetResponseString(WebResponse response)
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), true))
            {
                return reader.ReadToEnd(); 
            }
        }

        public override void Redirect(string url)
        {
            Navigate(url); 
            CancelLoad = true;            
        }
    }
}
