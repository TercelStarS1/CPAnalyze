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


        /// <summary>
        /// 返回所有用户列表
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UserList()
        {
            using (ZDSYYC db = new ZDSYYC())
            { 
                var user = db.PBE_USER.AsQuery().OrderBy(w=>w.CODE).ToList();

                return Succeed(user);
            }
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult EidtUser(PBE_USER info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                int user = db.PBE_USER.AsQuery().Where(w => w.CODE == info.CODE).Count();
                if (user != 0)
                {
                    return Succeed(1);
                }
                else {
                    PBE_USER pinfo = new PBE_USER();
                    pinfo.CODE = info.CODE.Trim().ToLower();
                    pinfo.NAME = info.NAME;
                    pinfo.PASSWORD = info.CODE;
                    db.PBE_USER.Add(pinfo);
                    db.Save();
                    return Succeed("");
                }
                
            }
        }

        /// <summary>
        /// 用户密码重置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UserPwd(PBE_USER info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var sql = "update PBE_USER set password='"+ info.CODE+ "' where code='" + info.CODE + "'";
                int allnum = db.ExecuteNoQuery(sql);
                db.Save();
                return Succeed("修改成功");
            }
        }


        /// <summary>
        /// 用户禁用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UserDisable(PBE_USER info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var sql = "update PBE_USER set password='" + info.CODE + "' where code='jy00001'";
                int allnum = db.ExecuteNoQuery(sql);
                db.Save();
                return Succeed("禁用成功");
            }
        }

    }
}