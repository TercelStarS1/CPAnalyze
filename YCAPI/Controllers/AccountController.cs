using System.Collections.Generic;
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
        public IHttpActionResult UserLoginYC(PBE_USER info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                string userName = info.CODE.Trim();
                string _password = info.PASSWORD; 
                 
                var user = db.PBE_USER.AsQuery().Where(w => w.CODE == userName && w.PASSWORD == _password).ToList();

                if (user.Count == 1) 
                {
                    return Succeed(user[0].CODE, 0, user[0].NAME, "");
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
        public IHttpActionResult UserEditPwdYC(PBE_USER info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                string code = info.CODE;
                string oldpwd = info.NAME;
                string _password = info.PASSWORD; 
                var sqlNum = "update PBE_USER set password='" + _password + "' where code='" + code + "' and password='" + oldpwd + "'";
                int allnum = db.ExecuteNoQuery(sqlNum);
                db.Save();
                if (allnum == 1)
                {
                    return Succeed("修改成功", 0, "");
                }
                else
                {
                    return Succeed("修改失败", 1, "", "旧密码错误");
                }
            }
        }


    }
}