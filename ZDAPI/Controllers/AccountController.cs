﻿using System.Collections.Generic;
using System.Web.Http;

using EntityInfos;
using SZORM;
using Helpers;
using System.Linq;
using System;
using ZDAPI.Controllers;
using System.Web.Configuration;
using System.Configuration;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [Authorize]
    public class AccountController : BasicController
    {
        public AccountController()
        {
        }

        private static string passwordStr = "zd";

        public static string PasswordStr
        {
            get
            {
                return passwordStr;
            }

            set
            {
                passwordStr = value;
            }
        }
        /// <summary>
        /// 用户登录成功并返回所有可以看到的菜单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UserLogin(ZB_FEED_COMPANY info)
        {
            using (StarOracle db = new StarOracle())
            { 
                string userName = info.CODE.Trim();
                string _password = info.PASSWORD;// MD51.StrMD5(passwordStr + info.PASSWORD); 
                if (userName == "admin")
                {
                    string pwd = GetWebConfigValueByKey("admin");
                    if (pwd == _password)
                    {
                        var query = db.ZB_FEED_COMPANY.AsQuery().OrderBy(w => w.CODE).ToList();
                        return Succeed(query, query, 0, "超级管理员", "0");
                    }
                    else {
                        return Succeed("拒绝访问", 1, "", "");
                    } 
                }
               
                var user = db.ZB_FEED_COMPANY.AsQuery().Where(w => w.CODE == userName && w.PASSWORD == _password).ToList();

                if (user.Count == 1)
                {
                    // var query = db.ZB_FEED_COMPANY.AsQuery().Where(r=>r.CODE== user[0].CODE);
                    return Succeed(user, user, 0, user[0].NAME, user[0].CODE);
                }
                else
                {
                    return Succeed("拒绝访问", 1, "", "");
                } 
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UserEditPwd(ZB_FEED_COMPANY info)
        {
            using (StarOracle db = new StarOracle())
            {
                string code = info.CODE;
                string oldpwd = info.SCHEMA;
                string _password = info.PASSWORD;// MD51.StrMD5(passwordStr + info.PASSWORD); 
                if (code == "admin" && GetWebConfigValueByKey("admin")==oldpwd)
                { 

                    Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
                    AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");


                    appSection.Settings.Remove("admin");
                    appSection.Settings.Add("admin", _password);
                    config.Save();

                    return Succeed("修改成功", 0, "");
                }
                else
                { 
                    var sqlNum = "update ZB_FEED_COMPANY set password='" + _password + "' where code='" + code + "' and password='"+ oldpwd + "'"; 
                    int allnum = db.ExecuteNoQuery(sqlNum);
                    db.Save();
                    if (allnum == 1)
                    { 
                        return Succeed("修改成功", 0, "");
                    }
                    else
                    {
                        return Succeed("修改失败", 1, "","旧密码错误");
                    }
                }
            }
        }
         
    }
}