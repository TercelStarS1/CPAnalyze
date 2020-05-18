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
    public class CustomerController : BasicController
    {
        public CustomerController()
        {
        }

        private static string num5customer = "";

        private static string num5customercy = "";

        [HttpGet]
        public IHttpActionResult GetCompany()
        {
            using (StarOracle db = new StarOracle())
            {
                //获取公司列表
                //var sqlNum = "select * from ZB_FEED_COMPANY where code in(select distinct company from zb_feed_sale t)"; 
                var sqlNum = "select * from ZB_FEED_COMPANY t where t.portion='huo'";
                var query = db.ExecuteSqlToList<ZB_FEED_COMPANY>(sqlNum);
                //return query.ToList();
                return Succeed(query.ToList());
            }
        }

         
        [HttpPost]
        public IHttpActionResult CompanyList()
        { 
            using (StarOracle db = new StarOracle())
            {
                //获取公司列表
                //var sqlNum = "select * from ZB_FEED_COMPANY where code in(select distinct company from zb_feed_sale t)";
                var sqlNum = "select * from ZB_FEED_COMPANY t where t.portion='huo' ";
                var query = db.ExecuteSqlToList<ZB_FEED_COMPANY>(sqlNum);
                return Succeed(query.ToList());
            }
        }
        #region  废弃方法
        /// <summary>
        /// 获取前五名客户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CustomerNum5(ZB_FEED_SALE info)
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.CUSTOMERValue;
                string startDate = info.startDate.ToShortDateString();
                string endDate = info.endDate.ToShortDateString();
                var sqlNum = "select customer,wgt from (" +
                    "SELECT  S.CUSTOMER customer, sum(S.WGT) wgt  FROM zb_feed_sale S where S.doc_date between to_date('" + startDate + "', 'yyyy-mm-dd') and to_date('" + endDate + "', 'yyyy-mm-dd')  and  company = '" + company + "' and customer not like '9%' GROUP BY S.CUSTOMER ORDER BY sum(S.WGT) DESC" +
                    ") where rownum < 6  ";
                var query = db.ExecuteSqlToList<ZB_FEED_SALE>(sqlNum).ToList();
                num5customer = "";
                foreach (var user in query)
                {
                    num5customer += "'" + user.CUSTOMER + "',";
                }
                num5customer = (num5customer.Length > 1) ? num5customer.Substring(0, num5customer.Length - 1) : "";
                return Succeed(query);
            }
        }
        /// <summary>
        /// 根据客户编码时间段 返回该时间段内客户的详细购货信息
        /// </summary>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">截止时间</param>
        /// <param name="company">公司编码</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CustomerList(ZB_FEED_SALE info)
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.CUSTOMERValue;
                string startDate = info.startDate.ToShortDateString();
                string endDate = info.endDate.ToShortDateString();
                var sqlNum = "select zf.customer customer,zf.doc_date doc_date, zf.company company,sum(zf.wgt) wgt,zc.name CUSTOMERName from zb_feed_sale zf  join zb_feed_customer zc on(zf.customer = zc.code and zf.company=zc.company )" +
                    "where zf.customer in(select sss from (" +
                    "SELECT  S.CUSTOMER SSS, sum(S.WGT) WGT  FROM zb_feed_sale S where S.doc_date between to_date('" + startDate + "', 'yyyy-mm-dd') and to_date('" + endDate + "', 'yyyy-mm-dd')  and  company = '" + company + "' and customer not like '9%' GROUP BY S.CUSTOMER ORDER BY sum(S.WGT) DESC" +
                    ") where rownum < 6) and zf.doc_date between to_date('" + startDate + "', 'yyyy-mm-dd') and to_date('" + endDate + "', 'yyyy-mm-dd') " +
                    "group by  zf.customer,zf.doc_date  ,zf.company,zc.name order by zf.doc_date ";


                List<ZB_FEED_SALE> query = db.ExecuteSqlToList<ZB_FEED_SALE>(sqlNum).ToList();
                return Succeed(query);
            }
        }
        #endregion
         

        /// <summary>
        /// 根据公司id返回客户总记录数
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult CustomerNumByPage(ZB_FEED_SALE info)   //当前页码，显示条数
        //{
        //    using (StarOracle db = new StarOracle())
        //    {
        //        string company = info.CUSTOMERValue;   //公司id
        //        string startDate = info.startDate.ToShortDateString(); //开始日期
        //        string endDate = info.endDate.ToShortDateString();  //结束日期
        //        var sqlNum = "select count(customer) from (" +
        //            "SELECT  S.CUSTOMER customer, sum(S.WGT) wgt  FROM zb_feed_sale S where S.doc_date between to_date('" + startDate + "', 'yyyy-mm-dd') and to_date('" + endDate + "', 'yyyy-mm-dd')  and  company = '" + company + "' and customer not like '9%' GROUP BY S.CUSTOMER ORDER BY sum(S.WGT) DESC ) ";
        //        string allnum = db.ExecuteScalar(sqlNum).ToString();  //获取总数据
                 

        //        return Succeed(allnum);
        //    }
        //} 

        /// <summary>
        /// 根据分页返回当前页面展示客户编码  按照销量多少排序
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CustomerShowByPage(ZB_FEED_SALE info)   //当前页码，显示条数
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.CUSTOMERValue;   //公司id
                string startDate = info.startDate.ToShortDateString(); //开始日期
                string endDate = info.endDate.ToShortDateString();  //结束日期
                //根据页数展示显示数据
                int startNUM = info.PageNum*info.ShowNum - info.ShowNum;
                int endNUM = startNUM + info.ShowNum;
                var sql = "select  * from (select customer,wgt,rownum rn  from (" +
                    "SELECT  S.CUSTOMER customer, sum(S.WGT) wgt  FROM zb_feed_sale S where S.doc_date between to_date('" + startDate + "', 'yyyy-mm-dd') and to_date('" + endDate + "', 'yyyy-mm-dd')  and  company = '" + company + "' and customer not like '9%' GROUP BY S.CUSTOMER ORDER BY sum(S.WGT) DESC" +
                    ")  )";

                var query = db.ExecuteSqlToList<ZB_FEED_SALE>(sql); 
                int num = query.Count(); 
                query = query.Where(s => s.RN > startNUM & s.RN <= endNUM).ToList();
                 
                num5customer = "";
                foreach (var user in query)
                {
                    num5customer += "'" + user.CUSTOMER + "',";
                }
                num5customer = (num5customer.Length > 1) ? num5customer.Substring(0, num5customer.Length - 1) : "";

                return Succeed(query,num,"");
            }
        }



        /// <summary>
        /// 根据分页返回当前页面展示客户编码  按照查询时间一分为二差异量排序
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CustomerShowByCYLPage(ZB_FEED_SALE info)   //当前页码，显示条数
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.CUSTOMERValue;   //公司id
                int day = (info.endDate - info.startDate).Days/2;
                string startDate = info.startDate.ToShortDateString(); //开始日期

                string end = info.startDate.AddDays(day).ToShortDateString(); //开始日期
                string start = info.startDate.AddDays(day+1).ToShortDateString(); //开始日期
                string endDate = info.endDate.ToShortDateString();  //结束日期
                string srot = info.Sort;  //排序
                //根据页数展示显示数据
                int startNUM = info.PageNum * info.ShowNum - info.ShowNum;
                int endNUM = startNUM + info.ShowNum;
                var sql = " select   customer, wgt,rn from ( select customer,wgt, px, rownum rn from  (  select nvl(aaa.customer,bbb.customer) customer, nvl(aaa.wgt,0)+nvl(bbb.wgt,0) wgt, nvl(aaa.wgt,0)-nvl(bbb.wgt,0) px  from( (SELECT  S.CUSTOMER customer, sum(S.WGT) wgt  FROM zb_feed_sale S " +
                    " where S.doc_date between to_date('"+ startDate + "', 'yyyy-mm-dd') and to_date('" + end + "', 'yyyy-mm-dd') and company = '"+ company + "' and customer not like '9%' GROUP BY S.CUSTOMER ORDER BY sum(S.WGT)DESC) aaa full join  (SELECT  S.CUSTOMER customer, sum(S.WGT) wgt  FROM zb_feed_sale S " +
                    " where S.doc_date between to_date('"+ start + "', 'yyyy-mm-dd') and to_date('" + endDate + "', 'yyyy-mm-dd')  and  company = '" + company + "' and customer not like '9%' GROUP BY S.CUSTOMER ORDER BY sum(S.WGT) DESC) bbb on (aaa.customer = bbb.customer) " +
                    " ) order by px "+ srot + "))";
                var query = db.ExecuteSqlToList<ZB_FEED_SALE>(sql);

                int num = query.Count();
                if (num == 0)
                {
                    return Succeed(query.ToList(), num, "");
                }
                else
                {
                    query = query.Where(s => s.RN > startNUM & s.RN <= endNUM).ToList();

                    num5customercy = "";
                    foreach (var user in query)
                    {
                        num5customercy += "'" + user.CUSTOMER + "',";
                    }
                    num5customercy = (num5customercy.Length > 1) ? num5customercy.Substring(0, num5customercy.Length - 1) : "";

                    var sql1 = " SELECT salesperson,customer,a2.name SalesName  FROM  ( select salesperson,customer,company from ( select ccc.*,row_number() over(partition by  customer order by eff_date desc) cn from ( " +
                         " select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join " +
                         " (select * from(select bbb.*, row_number() over(partition by customer, company order by doc_date desc) cn " +
                         " from zb_feed_sale bbb where company = '" + company + "' and customer  in(" + num5customercy + ")) where cn = 1 )   bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date)  " +
                         " ) ccc) where cn = 1) A1 JOIN  zb_feed_salesperson A2 ON(a1.salesperson = a2.code) and a2.company = '" + company + "'";
                    var result = db.ExecuteSqlToList<ZB_FEED_SALE>(sql1).ToList();

                    foreach (var user in query)
                    { 
                        for (int j = 0; j < result.Count; j++)
                        {
                            if (user.CUSTOMER == result[j].CUSTOMER)
                            {
                                user.SalesName = result[j].SalesName;
                                break;
                            }
                        }
                    } 
                    return Succeed(query, num, "");
                } 
            }
        }



        /// <summary>
        /// 根据客户编码获得该段时间内客户销量统计
        /// </summary>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">截止时间</param>
        /// <param name="company">公司编码</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CustomerListZero(ZB_FEED_SALE info)
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.CUSTOMERValue;
                string startDate = info.startDate.ToShortDateString();
                string endDate = info.endDate.ToShortDateString();

                var sqlNum = "select zf.customer customer,zf.doc_date doc_date, zf.company company,sum(zf.wgt) wgt,zc.name CUSTOMERName from zb_feed_sale zf  join zb_feed_customer zc on(zf.customer = zc.code and zf.company=zc.company )" +
                   "where zf.customer in(" + num5customer + ")  AND ZC.COMPANY ='"+company+"' and zf.doc_date between to_date('" + startDate + "', 'yyyy-mm-dd') and to_date('" + endDate + "', 'yyyy-mm-dd') " +
                   "group by  zf.customer,zf.doc_date  ,zf.company,zc.name order by zf.doc_date "; 

                List<ZB_FEED_SALE> query = db.ExecuteSqlToList<ZB_FEED_SALE>(sqlNum).ToList();

                List<ZB_FEED_SALE> result = new List<ZB_FEED_SALE>();
                string[] user = num5customer.Split(',');
                for (int i = 0; i < user.Length; i++)
                {
                    for (System.DateTime j = info.startDate; j <= info.endDate; j = j.AddDays(1))
                    {
                        ZB_FEED_SALE zbinfo = new ZB_FEED_SALE();
                        zbinfo.CUSTOMER = user[i].Replace("'", "");
                        zbinfo.DOC_DATE = j;
                        result.Add(zbinfo);
                    }
                }
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < query.Count; j++)
                    {
                        if (result[i].CUSTOMER == query[j].CUSTOMER && result[i].DOC_DATE.ToShortDateString() == query[j].DOC_DATE.ToShortDateString())
                        {
                            result[i].CUSTOMER = query[j].CUSTOMER;
                            result[i].DOC_DATE = query[j].DOC_DATE;
                            result[i].COMPANY = query[j].COMPANY;
                            result[i].WGT = query[j].WGT;
                            result[i].CUSTOMERName = query[j].CUSTOMERName;
                            break;
                        }
                    }
                } 
                int wgt = (int)query.OrderByDescending(m => m.WGT).First().WGT;
                return Succeed(result, wgt, "");
            }
        }


        /// <summary>
        /// 根据客户编码获得该段时间内客户销量统计
        /// </summary>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">截止时间</param>
        /// <param name="company">公司编码</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CustomerListZeroCY(ZB_FEED_SALE info)
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.CUSTOMERValue;
                string startDate = info.startDate.ToShortDateString();
                string endDate = info.endDate.ToShortDateString();

                var sqlNum = "select zf.customer customer,zf.doc_date doc_date, zf.company company,sum(zf.wgt) wgt,zc.name CUSTOMERName from zb_feed_sale zf  join zb_feed_customer zc on(zf.customer = zc.code and zf.company=zc.company )" +
                   "where zf.customer in(" + num5customercy + ") and Zf.COMPANY ='" + company + "' and  zf.doc_date between to_date('" + startDate + "', 'yyyy-mm-dd') and to_date('" + endDate + "', 'yyyy-mm-dd') " +
                   "group by  zf.customer,zf.doc_date  ,zf.company,zc.name order by zf.doc_date ";


                List<ZB_FEED_SALE> query = db.ExecuteSqlToList<ZB_FEED_SALE>(sqlNum).ToList();

                List<ZB_FEED_SALE> result = new List<ZB_FEED_SALE>();
                string[] user = num5customercy.Split(',');
                for (int i = 0; i < user.Length; i++)
                {
                    for (System.DateTime j = info.startDate; j <= info.endDate; j = j.AddDays(1))
                    {
                        ZB_FEED_SALE zbinfo = new ZB_FEED_SALE();
                        zbinfo.CUSTOMER = user[i].Replace("'", "");
                        zbinfo.DOC_DATE = j;
                        result.Add(zbinfo);
                    }
                }
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < query.Count; j++)
                    {
                        if (result[i].CUSTOMER == query[j].CUSTOMER && result[i].DOC_DATE.ToShortDateString() == query[j].DOC_DATE.ToShortDateString())
                        {
                            result[i].CUSTOMER = query[j].CUSTOMER;
                            result[i].DOC_DATE = query[j].DOC_DATE;
                            result[i].COMPANY = query[j].COMPANY;
                            result[i].WGT = query[j].WGT;
                            result[i].CUSTOMERName = query[j].CUSTOMERName;
                            break;
                        }
                    }
                }

                int wgt = (int)query.OrderByDescending(m => m.WGT).First().WGT;
                return Succeed(result, wgt, "");
            }
        }

    }
}