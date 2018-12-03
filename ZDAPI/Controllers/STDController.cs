using System.Collections.Generic;
using System.Web.Http;

using EntityInfos;
using SZORM;
using Helpers;
using System.Linq;
using System;
using ZDAPI.Controllers;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [Authorize]
    public class STDController : BasicController
    {
        public STDController()
        {
        }

        /// <summary>
        /// 疫苗使用标准
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult YMBZList(PBE_STDVACCINE info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var sql = "select t1.DAYS, t1.vaccine,t1.qty,t2.name VARIETY from  PBE_STDVACCINE t1 join PBE_VACCINE t2 on( t1.vaccine = t2.code )";
                var query = db.ExecuteSqlToList<PBE_STDVACCINE>(sql).ToList();
                return Succeed(query);
            }
        }

        /// <summary>
        /// 生长标准
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SZBZList(PBE_STDWITHAGE info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var query = db.PBE_STDWITHAGE.AsQuery().OrderBy(g => g.DAYS).ToList();

                return Succeed(query);
            }
        }

        /// <summary>
        /// 新建项目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult NewProject()
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var query1 = db.PBE_VACCINE.AsQuery().ToList();  //疫苗标准
                var query2 = db.PBE_FEED.AsQuery().ToList();    //饲料标准
                var query3 = db.PBE_STD.AsQuery().ToList().Where(w => w.PROJECT == "001").ToList();    //日龄相关标准  
                var query4 = db.PBE_MEDICINECOST.AsQuery().Where(w => w.PROJECT == "001").ToList();   //药品标准
                var query5 = db.PBE_PIGLETCOST.AsQuery().Where(w => w.PROJECT == "001").ToList();   //猪苗标准
                var query6 = db.PBE_PIGMARKETPRICE.AsQuery().Where(w => w.PROJECT == "001").ToList();   //种猪标准
                var query = new { ymlist = query1, sllist = query2, rllist = query3, yplist = query4, zmlist = query5, zzlist = query6 };
                return Succeed(query);
            }
        }

        [HttpPost]
        public IHttpActionResult FeedProject()
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var query = db.PBE_FEED.AsQuery().ToList();    //饲料标准

                return Succeed(query);
            }
        }

        [HttpPost]
        public IHttpActionResult FeedEidt(PBE_LIST infos)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                foreach (var item in infos.PBE_FEEDCOSTList)
                {
                    var sql = "update PBE_FEED set feedfac=" + item.FEEDFAC + " ,management=" + item.MANAGEMENT + ", faRm=" + item.FARM + " where code ='" + item.FEED + "'";
                    int allnum = db.ExecuteNoQuery(sql);
                }
                db.Save();
                return Succeed(db.PBE_FEED.AsQuery().ToList());
            }
        }

        [HttpPost]
        public IHttpActionResult VacProject()
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var query = db.PBE_VACCINE.AsQuery().ToList();    //饲料标准

                return Succeed(query);
            }
        }

        [HttpPost]
        public IHttpActionResult VacEidt(PBE_LIST infos)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                foreach (var item in infos.PBE_VACCINECOSTList)
                {
                    var sql = "update pbe_Vaccine set  management=" + item.MANAGEMENT + ", faRm=" + item.FARM + " where code ='" + item.CODE + "'";
                    int allnum = db.ExecuteNoQuery(sql);
                }
                db.Save();
                return Succeed(db.PBE_VACCINE.AsQuery().ToList());
            }
        }

        [HttpPost]
        public IHttpActionResult BasicProject()
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var query3 = db.PBE_STD.AsQuery().ToList().Where(w => w.PROJECT == "001").ToList();    //日龄相关标准  
                var query4 = db.PBE_MEDICINECOST.AsQuery().Where(w => w.PROJECT == "001").ToList();   //药品标准
                var query5 = db.PBE_PIGLETCOST.AsQuery().Where(w => w.PROJECT == "001").ToList();   //猪苗标准
                var query6 = db.PBE_PIGMARKETPRICE.AsQuery().Where(w => w.PROJECT == "001").ToList();   //种猪标准
                var query = new { rllist = query3, yplist = query4, zmlist = query5, zzlist = query6 };

                return Succeed(query);
            }
        }

        [HttpPost]
        public IHttpActionResult BasicEidt(PBE_LIST infos)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                //项目
                var sql = "update pbe_std set PIGLETQTY= " + infos.PBE_PROJECTINFO.PIGLETQTY + ",ADJUSTFEE=" + infos.PBE_PROJECTINFO.ADJUSTFEE + ",CULLINGRATE=" + infos.PBE_PROJECTINFO.CULLINGRATE + ",PERFECTRATE=" + infos.PBE_PROJECTINFO.PERFECTRATE + ",NOPERFRATE=" + infos.PBE_PROJECTINFO.NOPERFRATE + ",NOPERFWGTRATE=" + infos.PBE_PROJECTINFO.NOPERFWGTRATE + " where project='001'";
                int allnum = db.ExecuteNoQuery(sql);

                var sql1 = "update PBE_PIGLETCOST set management = " + infos.PBE_PIGLETCOSTINFO.MANAGEMENT + ", farm =" + infos.PBE_PIGLETCOSTINFO.FARM + " where project = '001'";
                int allnum1 = db.ExecuteNoQuery(sql1);
                var sql2 = "update PBE_PIGMARKETPRICE set CONTRACT = " + infos.PBE_PIGMARKETPRICEINFO.CONTRACT + ", MARKET =" + infos.PBE_PIGMARKETPRICEINFO.MARKET + " where project = '001'";
                int allnum2 = db.ExecuteNoQuery(sql2);
                var sql3 = "update PBE_MEDICINECOST set MANAGEMENT = " + infos.PBE_MEDICINECOSTINFO.MANAGEMENT + ", FARM =" + infos.PBE_MEDICINECOSTINFO.FARM + " where project = '001'";
                  int allnum3 = db.ExecuteNoQuery(sql3);
                db.Save(); 
                return Succeed("");
            }
        }


        /// <summary>
        /// 项目详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ProjectDetail(PBE_PROJECT info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var query1 = db.PBE_VACCINECOST.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList();  //疫苗标准
                var query2 = db.PBE_FEEDCOST.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList();    //饲料标准
                var sql = "select user1,project,inday,inwgt,outday,outwgt,pigletqty,indate,cullingrate,perfectrate,noperfrate,noperfwgtrate,noperfpricerate,adjustfee from PBE_PROJECT t where t.project = '" + info.PROJECT + "'";
                var query3 = db.ExecuteSqlToList<PBE_PROJECT>(sql).ToList();

                var query4 = db.PBE_MEDICINECOST.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList();   //药品标准
                var query5 = db.PBE_PIGLETCOST.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList();   //猪苗标准
                var query6 = db.PBE_PIGMARKETPRICE.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList();   //种猪标准
                var query = new { ymlist = query1, sllist = query2, rllist = query3, yplist = query4, zmlist = query5, zzlist = query6 };
                return Succeed(query);
            }
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult EditProject(PBE_LIST info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                string projectID = "";
                if (info.Skin != "0")  //如果是编辑状态，删除在添加
                {
                    projectID = info.Skin;
                    db.PBE_PROJECT.Remove(w => w.PROJECT == projectID);
                    db.PBE_FEEDCOST.Remove(w => w.PROJECT == projectID);
                    db.PBE_VACCINECOST.Remove(w => w.PROJECT == projectID);
                    db.PBE_PIGLETCOST.Remove(w => w.PROJECT == projectID);
                    db.PBE_PIGMARKETPRICE.Remove(w => w.PROJECT == projectID);
                    db.PBE_MEDICINECOST.Remove(w => w.PROJECT == projectID);
                }
                if (projectID == "")
                {
                    var sql = "select max(project)from PBE_PROJECT where user1 = '" + info.User + "'";
                    string num = db.ExecuteScalar(sql).ToString();
                    if (num == "")
                    {
                        projectID = info.User + "0001";
                    }
                    else
                    {
                        int nnn = int.Parse(num.Substring(num.Length - 4)) + 1;
                        projectID = info.User + string.Format("{0:0000}", nnn);
                    }
                }
                //饲料
                int id = 0;
                foreach (var item in info.PBE_FEEDCOSTList)
                {
                    id++;
                    PBE_FEEDCOST infos = new PBE_FEEDCOST();
                    infos.PROJECT = projectID;
                    infos.ID = id;
                    infos.FEED = item.FEED;
                    infos.FEEDFAC = item.FEEDFAC;
                    infos.MANAGEMENT = item.MANAGEMENT;
                    infos.FARM = item.FARM;
                    db.PBE_FEEDCOST.Add(infos);
                }
                //疫苗
                id = 0;
                foreach (var item in info.PBE_VACCINECOSTList)
                {
                    id++;
                    PBE_VACCINECOST infos = new PBE_VACCINECOST();
                    infos.PROJECT = projectID;
                    infos.ID = id;
                    infos.CODE = item.CODE;
                    infos.NAME = item.NAME;
                    infos.MANAGEMENT = item.MANAGEMENT;
                    infos.FARM = item.FARM;
                    db.PBE_VACCINECOST.Add(infos);
                }
                //项目
                var query3 = db.PBE_STD.AsQuery().ToList().Where(w => w.PROJECT == "001").ToList();    //日龄相关标准  
                PBE_PROJECT pebinfo = new PBE_PROJECT();
                pebinfo.INDATE = query3[0].INDATE;
                pebinfo.INDAY = query3[0].INDAY;
                pebinfo.OUTDAY = query3[0].OUTDAY;
                pebinfo.OUTWGT = query3[0].OUTWGT;
                pebinfo.NOPERFWGTRATE = query3[0].NOPERFWGTRATE;
                pebinfo.NOPERFPRICERATE = query3[0].NOPERFPRICERATE;
                pebinfo.NOPERFRATE = 100 - info.PBE_PROJECTINFO.PERFECTRATE;

                pebinfo.USER1 = info.User;
                pebinfo.PROJECT = projectID;  // 进猪数量   代养费   死淘   一级
                pebinfo.PIGLETQTY = info.PBE_PROJECTINFO.PIGLETQTY;
                pebinfo.ADJUSTFEE = info.PBE_PROJECTINFO.ADJUSTFEE;
                pebinfo.CULLINGRATE = info.PBE_PROJECTINFO.CULLINGRATE;
                pebinfo.PERFECTRATE = info.PBE_PROJECTINFO.PERFECTRATE;
                pebinfo.INDATE = DateTime.Now;
                db.PBE_PROJECT.Add(pebinfo);
                //小猪
                PBE_PIGLETCOST pebpig = new PBE_PIGLETCOST();
                pebpig.PROJECT = projectID;
                pebpig.MANAGEMENT = info.PBE_PIGLETCOSTINFO.MANAGEMENT;
                pebpig.FARM = info.PBE_PIGLETCOSTINFO.FARM;
                db.PBE_PIGLETCOST.Add(pebpig);
                //成品猪
                PBE_PIGMARKETPRICE pebpigm = new PBE_PIGMARKETPRICE();
                pebpigm.PROJECT = projectID;
                pebpigm.MARKET = info.PBE_PIGMARKETPRICEINFO.MARKET;
                pebpigm.CONTRACT = info.PBE_PIGMARKETPRICEINFO.CONTRACT;
                db.PBE_PIGMARKETPRICE.Add(pebpigm);
                //药品 
                PBE_MEDICINECOST pebmed = new PBE_MEDICINECOST();
                pebmed.PROJECT = projectID;
                pebmed.MANAGEMENT = info.PBE_MEDICINECOSTINFO.MANAGEMENT;
                pebmed.FARM = info.PBE_MEDICINECOSTINFO.FARM;
                db.PBE_MEDICINECOST.Add(pebmed);
                db.Save();

                return Succeed(projectID);
            }
        }

        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>


        [HttpPost]
        public IHttpActionResult ProjectAll(PBE_PROJECT info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                if (info.USER1 == "admin")
                {
                    var sql = "select user1  from PBE_PROJECT t";
                    var query = db.ExecuteSqlToList<PBE_PROJECT>(sql).ToList().Count();
                    return Succeed(query);
                }
                else
                {
                    var sql = "select user1  from PBE_PROJECT t where t.user1 = '" + info.USER1 + "'";
                    var query = db.ExecuteSqlToList<PBE_PROJECT>(sql).ToList().Count();
                    return Succeed(query);
                }
            }
        }

        [HttpPost]
        public IHttpActionResult ProjectList(PBE_PROJECT info)
        {
            decimal startNUM = info.INDAY * info.OUTDAY - info.OUTDAY;
            decimal endNUM = startNUM + info.OUTDAY;

            using (ZDSYYC db = new ZDSYYC())
            {
                if (info.USER1 == "admin")
                {
                    var sql = "select tt.* ,rownum INDAY from(select user1,project,inwgt,outday,outwgt,pigletqty,indate,cullingrate,perfectrate,noperfrate,noperfwgtrate,noperfpricerate,adjustfee from PBE_PROJECT t order by  indate desc) tt";
                    var query = db.ExecuteSqlToList<PBE_PROJECT>(sql);
                    query = query.Where(s => s.INDAY > startNUM & s.INDAY <= endNUM).ToList();
                    return Succeed(query);
                }
                else
                {
                    var sql = "select tt.* ,rownum INDAY from(select user1,project,inwgt,outday,outwgt,pigletqty,indate,cullingrate,perfectrate,noperfrate,noperfwgtrate,noperfpricerate,adjustfee from PBE_PROJECT t where " +
                                " t.user1 = '" + info.USER1 + "'order by  indate desc) tt";
                    var query = db.ExecuteSqlToList<PBE_PROJECT>(sql);
                    query = query.Where(s => s.INDAY > startNUM & s.INDAY <= endNUM).ToList();
                    return Succeed(query);
                }
            }
        }

        [HttpPost]
        public IHttpActionResult ShowProfit(PBE_PROJECT info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var q = db.PBE_PROJECT.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList()[0];
                var qq = db.PBE_PIGMARKETPRICE.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList()[0];
                var query = db.PBE_V_BUDGET.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList()[0];
                var feedCost = query.Feedfac;
                var feedIncome = query.Feedmanagement;
                var feedProfit = feedIncome - feedCost;  

                var manCost = query.Feedmanagement + query.Medmanagement + query.VacMANAGEMENT + query.PigletMANA + query.Contract + query.ADJUSTFEE;
                var manIncome = query.Feedfarm + query.Medfarm + query.VacFARM + query.PigletFARM + query.Market;
                var manProfit = manIncome - manCost;

                var farmCost = query.Feedfarm + query.Medfarm + query.VacFARM + query.PigletFARM;
                var farmIncome = query.Contract + query.ADJUSTFEE;
                var farmProfit = farmIncome - farmCost;
                var result = new string[9, 2] { { "feedCost", feedCost.ToString("0.00") }, { "feedIncome", feedIncome.ToString("0.00") }, { "feedProfit", feedProfit.ToString("0.00") }, { "manCost", manCost.ToString("0.00") }, { "manIncome", manIncome.ToString("0.00") }, { "manProfit", manProfit.ToString("0.00") }, { "farmCost", farmCost.ToString("0.00") }, { "farmIncome", farmIncome.ToString("0.00") }, { "farmProfit", farmProfit.ToString("0.00") } };

                var s = new
                {
                    plist = q,
                    pm = qq,
                    slist = result 
                };
                return Succeed(s);
            }
        }


        [HttpPost]
        public IHttpActionResult EditShowProfit(PBE_PROJECT info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                var sql = "update PBE_PROJECT set CULLINGRATE= "+info.CULLINGRATE + ", PERFECTRATE=" + info.PERFECTRATE + ", NOPERFRATE=" + (100-info.PERFECTRATE) + " where PROJECT='" + info.PROJECT + "'";
                var sql1 = "update PBE_PIGMARKETPRICE set market=" + info.NOPERFRATE + " where PROJECT='" + info.PROJECT + "'";
                int allnum = db.ExecuteNoQuery(sql);
                int allnum1 = db.ExecuteNoQuery(sql1);
                db.Save();


                var q = db.PBE_PROJECT.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList()[0];
                var qq = db.PBE_PIGMARKETPRICE.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList()[0];
                var query = db.PBE_V_BUDGET.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList()[0];
                var feedCost = query.Feedfac;
                var feedIncome = query.Feedmanagement;
                var feedProfit = feedIncome - feedCost;

                var manCost = query.Feedmanagement + query.Medmanagement + query.VacMANAGEMENT + query.PigletMANA + query.Contract + query.ADJUSTFEE;
                var manIncome = query.Feedfarm + query.Medfarm + query.VacFARM + query.PigletFARM + query.Market;
                var manProfit = manIncome - manCost;

                var farmCost = query.Feedfarm + query.Medfarm + query.VacFARM + query.PigletFARM;
                var farmIncome = query.Contract + query.ADJUSTFEE;
                var farmProfit = farmIncome - farmCost;
                var result = new string[9, 2] { { "feedCost", feedCost.ToString("0.00") }, { "feedIncome", feedIncome.ToString("0.00") }, { "feedProfit", feedProfit.ToString("0.00") }, { "manCost", manCost.ToString("0.00") }, { "manIncome", manIncome.ToString("0.00") }, { "manProfit", manProfit.ToString("0.00") }, { "farmCost", farmCost.ToString("0.00") }, { "farmIncome", farmIncome.ToString("0.00") }, { "farmProfit", farmProfit.ToString("0.00") } };

                var s = new
                {
                    plist = q,
                    pm = qq,
                    slist = result
                };
                return Succeed(s);
            }
        }

    }
}