using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ZDAPI.Controllers
{
    public class BaseMessage
    {
        public int code { get; set; }
        public int num { get; set; }
        public string skin { get; set; }
        public string remark { get; set; }
        public string message { get; set; }
    }
    public class ResultMessage<T>: BaseMessage
    { 
        public T data { get; set; }

        public T data2 { get; set; }
    } 

    public class BasicController: ApiController
    {
        public IHttpActionResult Succeed()
        {
            BaseMessage message = new BaseMessage();
            message.code = 1;
            message.message = "succeed";
            return Ok(message);
        }
        public IHttpActionResult Succeed<T>(T data)
        {
            ResultMessage<T> message = new ResultMessage<T>();
            message.code = 1;
            message.message = "succeed";
            message.data = data;
            return Ok(message);
        }

        public IHttpActionResult Succeed<T>(T data,int num, string skin)
        {
            ResultMessage<T> message = new ResultMessage<T>();
            message.code = 1;
            message.message = "succeed";
            message.num = num;
            message.skin = skin;
            message.data = data;
            return Ok(message);
        }

        public IHttpActionResult Succeed<T>(T data, T data2, int num, string skin)
        {
            ResultMessage<T> message = new ResultMessage<T>();
            message.code = 1;
            message.message = "succeed";
            message.num = num;
            message.skin = skin;
            message.data = data;
            message.data2 = data2;
            return Ok(message);
        }

        public IHttpActionResult Succeed<T>(T data, T data2, int num, string skin, string remark)
        {
            ResultMessage<T> message = new ResultMessage<T>();
            message.code = 1;
            message.message = "succeed";
            message.num = num;
            message.skin = skin;
            message.data = data;
            message.data2 = data2;
            return Ok(message);
        }

        public IHttpActionResult Succeed<T>(T data, int num, string skin,string remark)
        {
            ResultMessage<T> message = new ResultMessage<T>();
            message.code = 1;
            message.message = "succeed";
            message.num = num;
            message.skin = skin;
            message.data = data;
            message.remark = remark;
            return Ok(message);
        }
        public IHttpActionResult Error(string msg)
        {
            BaseMessage message = new BaseMessage();
            message.code = 2;
            message.message = msg;
            return Ok(message);
        }

        /// <summary>
        /// 通过key,获取appSetting的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public static string GetWebConfigValueByKey(string key)
        {
            string value = string.Empty;
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");
            if (appSetting.Settings[key] != null)//如果不存在此节点，则添加  
            {
                value = appSetting.Settings[key].Value;
            }
            config = null;
            return value;
        }
    }

}