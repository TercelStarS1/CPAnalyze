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
                var query6 = db.PBE_PIGMARKETPRICE.AsQuery().Where(w=>w.PROJECT=="001").ToList();   //种猪标准
                var query = new { ymlist = query1, sllist = query2, rllist = query3, yplist = query4, zmlist = query5, zzlist = query6 };
                return Succeed(query);
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
                var query3 = db.PBE_PROJECT.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList();    //日龄相关标准  
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
                    db.PBE_PROJECT.Remove(w =>w.PROJECT ==projectID);
                    db.PBE_FEEDCOST.Remove(w => w.PROJECT == projectID);
                    db.PBE_VACCINECOST.Remove(w => w.PROJECT == projectID);
                    db.PBE_PIGLETCOST.Remove(w => w.PROJECT == projectID);
                    db.PBE_PIGMARKETPRICE.Remove(w => w.PROJECT == projectID);
                    db.PBE_MEDICINECOST.Remove(w => w.PROJECT == projectID); 
                }
                if (projectID == "")
                {
                    var sql = "select max(project)from PBE_PROJECT where user1 = '"+info.User+"'";
                    string num = db.ExecuteScalar(sql).ToString();
                    if (num == "")
                    {
                        projectID = info.User + "0001";
                    }
                    else {
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
                pebinfo.NOPERFRATE = 100- info.PBE_PROJECTINFO.PERFECTRATE;
                 
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
                    var query = db.PBE_PROJECT.AsQuery().Count();
                    return Succeed(query);
                }
                else
                {
                    var query = db.PBE_PROJECT.AsQuery().Where(m => m.USER1 == info.USER1).Count();
                    return Succeed(query);
                }
            }
        }

        [HttpPost]
        public IHttpActionResult ProjectList(PBE_PROJECT info)
        {
            using (ZDSYYC db = new ZDSYYC())
            {
                if (info.USER1 == "admin")
                {
                    var query = db.PBE_PROJECT.AsQuery().OrderByDesc(m=>m.PROJECT).ToList();
                    return Succeed(query);
                }
                else
                {
                    var query = db.PBE_PROJECT.AsQuery().Where(m=>m.USER1 == info.USER1).OrderByDesc(m => m.PROJECT).ToList();
                    return Succeed(query);
                } 
            }
        }

        [HttpPost]
        public IHttpActionResult ShowProfit(PBE_PROJECT info)
        {
            using (ZDSYYC db = new ZDSYYC())
            { 
                var query = db.PBE_V_BUDGET.AsQuery().Where(w => w.PROJECT == info.PROJECT).ToList()[0];  
                var feedCost = query.Feedfac; 
                var feedIncome = query.Feedmanagement;
                var feedProfit = feedIncome- feedCost;
                //
                var manCost = query.Feedmanagement + query.Medmanagement + query.VacMANAGEMENT + query.PigletMANA + query.Contract;
                var manIncome = query.Feedfarm + query.Medfarm + query.VacFARM + query.PigletFARM + query.Market;
                var manProfit = manIncome - manCost;

                var farmCost = query.Feedfarm + query.Medfarm + query.VacFARM + query.PigletFARM + query.Market;
                var farmIncome = query.Contract + query.Contract;
                var farmProfit = farmIncome-farmCost;
                var result = new string [9,2]{ { "feedCost", feedCost.ToString("0.00") }, { "feedIncome", feedIncome.ToString("0.00") }, { "feedProfit", feedProfit.ToString("0.00") }, { "manCost", manCost.ToString("0.00") }, { "manIncome", manIncome.ToString("0.00") }, { "manProfit", manProfit.ToString("0.00") }, { "farmCost", farmCost.ToString("0.00") }, { "farmIncome", farmIncome.ToString("0.00") }, { "farmProfit", farmProfit.ToString("0.00") } };
  
                //Dictionary<string, string> dictionary = new Dictionary<string, string>();
                //dictionary.Add("feedCost", feedCost.ToString("0.00"));
                //dictionary.Add("feedIncome", feedIncome.ToString("0.00"));
                //dictionary.Add("feedProfit", feedProfit.ToString("0.00"));
                //dictionary.Add("manCost", manCost.ToString("0.00"));
                //dictionary.Add("manIncome", manIncome.ToString("0.00"));
                //dictionary.Add("manProfit", manProfit.ToString("0.00"));
                //dictionary.Add("farmCost", farmCost.ToString("0.00"));
                //dictionary.Add("farmIncome", farmIncome.ToString("0.00"));
                //dictionary.Add("farmProfit", farmProfit.ToString("0.00"));

                //var result = dictionary.ToList(); 
                return Succeed(result);
            }
        }
    }
}