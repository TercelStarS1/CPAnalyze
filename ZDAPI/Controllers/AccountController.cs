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
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

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
               
                var user = db.ZB_FEED_COMPANY.AsQuery().Where(w => w.CODE == userName && w.PASSWORD == _password).ToList();

                if (user.Count == 1)
                { 
                    return Succeed(user, user, 0, user[0].NAME, user[0].CODE,user[0].PORTION);
                }
                else
                {
                    string path = HttpContext.Current.Server.MapPath("~/user.json");
                    using (StreamReader r = new StreamReader(path, Encoding.Default))
                    {
                        string json = r.ReadToEnd();
                        dynamic array = JsonConvert.DeserializeObject(json);
                        foreach (var item in array.user)
                        {
                            if (item.userCode == userName && item.userPwd == _password)
                            {
                                string portion = item.userPortion;
                                var query = db.ZB_FEED_COMPANY.AsQuery().Where(w=> w.PORTION == portion).OrderBy(w => w.CODE).ToList();
                                return Succeed(query, query, 0, (string)item.userName, (string)item.userCode, (string)portion);
                            }
                        }
                    }
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

                var sqlNum = "update ZB_FEED_COMPANY set password='" + _password + "' where code='" + code + "' and password='" + oldpwd + "'";
                int allnum = db.ExecuteNoQuery(sqlNum);
                db.Save();
                if (allnum == 1)
                {
                    return Succeed("修改成功", 0, "");
                }
                else
                {
                    string path = HttpContext.Current.Server.MapPath("~/user.json");
                    string jsonStr = File.ReadAllText(path, Encoding.Default);
                    JObject jo = JObject.Parse(jsonStr);   //解析Json
                    bool skin = false;
                    for (int i = 0; i < jo["user"].Count(); i++)
                    {
                        if ((string)jo["user"][i]["userCode"] == code && (string)jo["user"][i]["userPwd"] == oldpwd)
                        {
                            jo["user"][i]["userPwd"] = _password;
                            skin = true;
                            break;
                        }
                    }
                    if (skin)
                    {
                        string convertString = Convert.ToString(jo);
                        File.WriteAllText(path, convertString, Encoding.Default);   //将转换后的文件写入
                        return Succeed("修改成功", 0, "");
                    }
                    else
                    {
                        return Succeed("修改失败", 1, "", "旧密码错误");
                    } 
                }
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
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UserSave(StarClassHelp info)
        {
            string path = HttpContext.Current.Server.MapPath("~/user.json");
            string jsonStr = File.ReadAllText(path, Encoding.Default);
            JObject jo = JObject.Parse(jsonStr);   //解析Json
            bool skin = false;
            for (int i = 0; i < jo["user"].Count(); i++)
            {
                if ((string)jo["user"][i]["userCode"] == info.CODE)
                {
                    skin = true;
                    break;
                }
            }
            if (skin)
            {
                return Succeed("账号已经存在", 1, "");
            }
            else
            {
                jo["user"][0].AddAfterSelf(JObject.Parse("{\"userCode\":\"" + info.CODE + "\",\"userName\":\"" + info.NAME + "\",\"userPortion\":\"" + info.PORTION + "\",\"userPwd\":\"" + info.CODE + "\"}"));

                string convertString = Convert.ToString(jo);
                File.WriteAllText(path, convertString, Encoding.Default);   //将转换后的文件写入
                return Succeed("添加成功", 0, "");
            }

        }


        /// <summary>
        /// 返回所有用户列表
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UserListFx(StarClassHelp info)
        {
            string path = HttpContext.Current.Server.MapPath("~/user.json");
            using (StreamReader r = new StreamReader(path, Encoding.Default))
            {
                string json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);

                List<StarClassHelp> starhelp = new List<StarClassHelp>();
                foreach (var item in array.user)
                {
                    if (info.PORTION == (string)item.userPortion) {
                        StarClassHelp star = new StarClassHelp();
                        star.CODE = item.userCode;
                        star.NAME = item.userName;
                        starhelp.Add(star);
                    }
                }
                return Succeed(starhelp);
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
                var user = db.PBE_USER.AsQuery().OrderBy(w => w.CODE).ToList();

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
                else
                {
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


        [HttpPost]
        public IHttpActionResult UserPwdFx(StarClassHelp info)
        {
            string path = HttpContext.Current.Server.MapPath("~/user.json");
            string jsonStr = File.ReadAllText(path, Encoding.Default);
            JObject jo = JObject.Parse(jsonStr);   //解析Json
            bool skin = false;
            for (int i = 0; i < jo["user"].Count(); i++)
            {
                if ((string)jo["user"][i]["userCode"] == info.CODE)
                {
                    jo["user"][i]["userPwd"] = info.CODE;
                    skin = true;
                    break;
                }
            }
            if (skin)
            {
                string convertString = Convert.ToString(jo);
                File.WriteAllText(path, convertString, Encoding.Default);   //将转换后的文件写入
                return Succeed("修改成功", 0, "");
            }
            else
                return Succeed("修改失败", 1, "");
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
                var sql = "update PBE_USER set password='" + info.CODE + "' where code='" + info.CODE + "'";
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


        public class StarClassHelp
        {
            [SZColumn(MaxLength = 255)]
            public string CODE { get; set; }
            [SZColumn(MaxLength = 255)]
            public string NAME { get; set; }
            [SZColumn(MaxLength = 255)]
            public string PORTION { get; set; }
        }


    }
}