using System.Collections.Generic;
using System.Web.Http;

using EntityInfos;
using SZORM;
using Helpers;
using System.Linq;
using System;
using ZDAPI.Controllers;
using System.Configuration;
using System.Web;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [Authorize]
    public class SaleController : BasicController
    {
        public SaleController()
        {
        } 
        private static string num5sales = "";
        private static string num5saleslost = "";
        
        private static string sss = GetWebConfigValueByKey("DataStartDate");

        /// <summary>
        /// 获取某个时间段内所有公司新客户数量
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CustomerNewAll(ZB_FEED_CUSTOMER info)
        {
            using (StarOracle db = new StarOracle())
            { 
                string startDate = DateTime.Now.AddDays(info.Num).ToShortDateString();  
                
                var sqlNum = "select COMPANY,name,Num from( select count(customer) num , company from  (select * from(select customer, company,min(doc_date) doc_date from zb_feed_sale " +
                    " where customer not like '9%' group by customer, company ) "+
                    " ) where doc_date > to_date('" + startDate + "', 'YYYY/MM/DD')  group by company ) t1 join ZB_FEED_COMPANY t2 on(t1.company = t2.code)";

                var query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum).ToList();
                int num = query.Sum(g => g.Num); 
                return Succeed(query, num, "");
            }
        } 

        /// <summary>
        /// 根据分页返回当前页面 销售员开发新客户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult NewCoustomerByPage(ZB_FEED_CUSTOMER info)   //当前页码，显示条数
        {
            using (StarOracle db = new StarOracle())
            {
                string sss = GetWebConfigValueByKey("DataStartDate");

                string company = info.COMPANY; //公司ID
                string startDate = info.startDate.ToShortDateString();
                string endDate = info.endDate.AddMonths(1).ToShortDateString();

                //根据页数展示显示数据
                int startNUM = info.PageNum * info.ShowNum - info.ShowNum;
                int endNUM = startNUM + info.ShowNum;

                var sqlNum = "select * from(select salesperson code, num, CUSTOMERValue, name , rownum rn from (  select salesperson, count(salesperson) num ,sum(wgt) CUSTOMERValue from(select ccc.*, row_number() over(partition by  customer, company order by eff_date desc) cn from(select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join" +
                    " (select * from(select bbb.*, row_number() over(partition by customer, company order by doc_num) cn from zb_feed_sale bbb where company = '" + company + "' and customer not like '9%') where cn = 1 and doc_date > to_date('" + sss + "', 'YYYY/MM/DD'))" +
                    " bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date)  ) ccc ) " +
                    " where cn = 1 and doc_date between to_date('"+ startDate + "', 'yyyy-mm-dd')  and to_date('" + endDate + "', 'yyyy-mm-dd')  group by salesperson order by num desc ,CUSTOMERValue desc  ) " +
                    " ddd left join zb_feed_salesperson fff on(ddd.salesperson = fff.code) and fff.company = '"+ company + "' order by num desc, CUSTOMERValue desc " +
                    " ) ";

                var query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum);
                int num = query.Count();

                query = query.Where(s => s.RN > startNUM & s.RN <= endNUM);

                List<ZB_FEED_CUSTOMER> result = query.ToList();
                num5sales = "";
                foreach (var ZB_FEED_CUSTOMER in result)
                {
                    num5sales += "'" + ZB_FEED_CUSTOMER.CODE + "',";
                }
                num5sales = (num5sales.Length > 1) ? num5sales.Substring(0, num5sales.Length - 1) : "";
                return Succeed(result, num, "");
            }
        }

         
         
        /// <summary>
        /// 获取某公司前五名销售统计
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SalseDetailByPage(ZB_FEED_CUSTOMER info)
        {
            using (StarOracle db = new StarOracle())
            { 

                string company = info.COMPANY; //公司ID
                string startDate = info.startDate.Year + "-" + info.startDate.Month;
                System.DateTime end = info.endDate.AddMonths(1);
                string endDate = end.Year + "-" + end.Month;
                var sqlNum = "select r.*, zc.name from (SELECT T.customer, TO_CHAR(T.doc_date, 'YYYY-MM') monthall, sum(wgt) wgt, TT.salesperson, T.company FROM zb_feed_sale T "+
                        " right join(select salesperson, customer from (  select ccc.*, row_number() over(partition by  customer, company order by eff_date desc) cn from(" +
                        " select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join" +
                        " (select* from (select bbb.*, row_number() over(partition by customer, company order by doc_num) cn from zb_feed_sale bbb where company = '"+ company + "' and customer not like '9%') where cn = 1 and doc_date > to_date('"+sss+"', 'YYYY/MM/DD')) " +
                        " bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date)  ) ccc ) " +
                        " where cn = 1 and doc_date between to_date('"+ startDate + "', 'yyyy-mm')  and to_date('"+ endDate + "', 'yyyy-mm')   and salesperson in("+ num5sales + ") " +
                        " ) TT ON(T.CUSTOMER = TT.CUSTOMER) group by TO_CHAR(T.doc_date,'YYYY-MM') ,T.company,T.customer,TT.salesperson) r join zb_feed_customer zc on(r.customer = zc.code and r.company= zc.company) order by r.customer,r.monthall"; 

                    List<ZB_FEED_CUSTOMER> query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum).ToList();

                List<ZB_FEED_CUSTOMER> num = query.GroupBy(r => r.Customer).Select(r => r.First()).ToList();  //統計新客户个数

                var aaa =  query.GroupBy(r => r.Customer).Select(g => (new { Customer = g.Key, Num = g.Sum(a => a.WGT) })).ToList();
                 
                var bbbb = query.Where(s => s.monthall == info.endDate.ToString("yyyy-MM") || s.monthall == info.endDate.AddMonths(-1).ToString("yyyy-MM")).Select(r => new { Salesperson = r.Salesperson, Customer = r.Customer}).Distinct().ToList();

                var ccc =  bbbb.GroupBy(g => g.Salesperson).Select(g => (new { Customer = g.Key, Num = g.Count() })).ToList();

                List<ZB_FEED_CUSTOMER> data2 = new List<ZB_FEED_CUSTOMER>();
                for (int i = 0; i < ccc.Count; i++)
                {
                    ZB_FEED_CUSTOMER c1 = new ZB_FEED_CUSTOMER();
                    c1.Salesperson = ccc[i].Customer;
                    c1.Num = ccc[i].Num;
                    data2.Add(c1);
                }
                for (int i = 0; i < num.Count; i++)
                {
                    for (int j = 0; j < aaa.Count; j++)
                    {
                        if (num[i].Customer == aaa[j].Customer)
                        {
                            num[i].Num = (int)aaa[j].Num;
                            break;
                        }
                    }
                }

                List<ZB_FEED_CUSTOMER> result = new List<ZB_FEED_CUSTOMER>();
                                 
                for (int i = 0; i < num.Count; i++)
                {
                    for (System.DateTime j = info.startDate; j <= info.endDate; j = j.AddMonths(1))
                    {
                        ZB_FEED_CUSTOMER zbinfo = new ZB_FEED_CUSTOMER();
                        zbinfo.Customer = num[i].Customer;  //客户编号
                        zbinfo.Salesperson = num[i].Salesperson;
                        zbinfo.Num = num[i].Num;
                        zbinfo.monthall = j.ToString("yyyy-MM");  //销售月份
                        zbinfo.WGT = 0; 

                        result.Add(zbinfo);
                    }
                }
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < query.Count; j++)
                    {
                        if (result[i].Customer == query[j].Customer && result[i].monthall == query[j].monthall)
                        {
                            result[i].WGT = query[j].WGT;  //该月销售重量
                            result[i].Salesperson = query[j].Salesperson;  //业务员
                            result[i].NAME = query[j].NAME;  //客户姓名
                            break;
                        }
                    }
                }

                int wgt = (int)query.OrderByDescending(m => m.WGT).First().WGT;
                return Succeed(result, data2, wgt, "");
                 
            }
        }
        /*
        #region 
        /// <summary>
        /// 获取某公司前五名销售信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SalesNum5(ZB_FEED_CUSTOMER info)
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.COMPANY; //公司ID
                string startDate = info.startDate.ToShortDateString();
                string endDate = info.endDate.ToShortDateString();
                var sqlNum = "select salesperson code,num,CUSTOMERValue,name from ( select * from (select salesperson, count(salesperson) num ,sum(wgt) CUSTOMERValue from (  select ccc.*,row_number() over(partition by  customer,company order by eff_date desc) cn from " +
                    " (select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join" +
                    " (select * from (select bbb.*, row_number() over(partition by customer, company order by rownum) cn from zb_feed_sale bbb where company = '" + company + "' and customer not like '9%') where cn = 1 and doc_date > to_date('2017/04/01', 'YYYY/MM/DD')) bbb" +
                    " on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date) ) ccc ) where cn = 1 and doc_date between to_date('" + startDate + "', 'yyyy-mm-dd') " +
                    " and to_date('" + endDate + "', 'yyyy-mm-dd') group by salesperson order by num desc ,CUSTOMERValue desc)where ROWNUM  < 6  ) ddd join zb_feed_salesperson fff on(ddd.salesperson = fff.code) and fff.company = '" + company + "'" +
                    " order by num desc, CUSTOMERValue desc";

                var query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum);
                List<ZB_FEED_CUSTOMER> result = query.ToList();
                num5sales = "";
                foreach (var ZB_FEED_CUSTOMER in result)
                {
                    num5sales += "'" + ZB_FEED_CUSTOMER.CODE + "',";
                }
                num5sales = (num5sales.Length > 1) ? num5sales.Substring(0, num5sales.Length - 1) : "";
                return Succeed(result);
            }
        }

        /// <summary>
        /// 获取某公司前五名销售统计
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SalseDetail(ZB_FEED_CUSTOMER info)
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.COMPANY; //公司ID
                string startDate = info.startDate.Year + "-" + info.startDate.Month;
                System.DateTime end = info.endDate.AddMonths(1);
                string endDate = end.Year + "-" + end.Month;
                var sqlNum = "select salesperson code,to_char(doc_date,'yyyy-mm') monthall,count( salesperson) Num,sum(wgt) CUSTOMERValue from ( select salesperson,customer,company,doc_date,wgt from (  " +
                            " select ccc.*,row_number() over(partition by  customer, company order by eff_date desc) cn from (select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join" +
                             " (select * from (select bbb.*, row_number() over(partition by customer, company order by rownum) cn from zb_feed_sale bbb where company = '" + company + "' and customer not like '9%') where cn = 1 and doc_date > to_date('2017/04/01', 'YYYY/MM/DD')) bbb" +
                            " on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date) ) ccc ) where cn = 1)   where doc_date between to_date('" + startDate + "', 'yyyy-mm') and to_date('" + endDate + "', 'yyyy-mm') and" +
                                " salesperson in (" + num5sales + ")  group by salesperson, to_char(doc_date, 'yyyy-mm')order by salesperson desc, monthall";

                List<ZB_FEED_CUSTOMER> query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum).ToList();

                List<ZB_FEED_CUSTOMER> result = new List<ZB_FEED_CUSTOMER>();
                string[] user = num5sales.Split(',');
                for (int i = 0; i < user.Length; i++)
                {
                    for (System.DateTime j = info.startDate; j <= info.endDate; j = j.AddMonths(1))
                    {
                        ZB_FEED_CUSTOMER zbinfo = new ZB_FEED_CUSTOMER();
                        zbinfo.CODE = user[i].Replace("'", "");
                        zbinfo.monthall = j.ToString("yyyy-MM");
                        zbinfo.Num = 0;
                        zbinfo.CUSTOMERValue = 0;
                        result.Add(zbinfo);
                    }
                }
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < query.Count; j++)
                    {
                        if (result[i].CODE == query[j].CODE && result[i].monthall == query[j].monthall)
                        {
                            result[i].Num = query[j].Num;
                            result[i].CUSTOMERValue = query[j].CUSTOMERValue;
                            break;
                        }
                    }
                }
                return Succeed(result);
            }
        }
        #endregion
        */

        #region  丢失客户统计   向前推三个月

        /// <summary>
        /// 某个时间段内某公司销售丢失客户统计
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SalesLostNumAll(ZB_FEED_CUSTOMER info)
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.COMPANY; //公司ID
                string startDate = info.startDate.ToShortDateString();  //统计开始时间
                string beginDate = info.startDate.AddMonths(-3).ToShortDateString();  //向前推三个月

                var sqlNum = "select  salesperson,   num, rownum rn from ( select salesperson, count(salesperson) num  from (  select ccc.*,row_number() over(partition by  customer, company order by eff_date desc) cn from  "+
                        " (select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join   (  select * from (select customer, doc_date, wgt, amt, company, row_number() over(partition by  customer, company order by doc_date desc) cn from zb_feed_sale bbb where customer in "+
                        " (select  customer from(select  distinct customer from zb_feed_sale where company = '" + company + "' and  doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd'))aaa" +
                        "  where customer not in (select distinct customer  from zb_feed_sale where company = '" + company + "' and doc_date > to_date('" + startDate + "', 'yyyy-mm-dd')) and customer not like '9%' " +
                        "  ) and bbb.company = '"+ company + "' and bbb.doc_date > to_date('"+ beginDate + "', 'yyyy-mm-dd')  order by bbb.customer, bbb.company, bbb.doc_date desc ) where cn = 1 ) bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date)   )ccc ) where cn = 1  group by salesperson order by num desc   ) ";

                var query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum);

                return Succeed(query.Count());
            }
        }



        /// <summary>
        /// 根据分页返回当前页面 销售员丢失客户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult LostCoustomerByPage(ZB_FEED_CUSTOMER info)   //当前页码，显示条数
        {
            using (StarOracle db = new StarOracle())
            {
                string sss = GetWebConfigValueByKey("DataStartDate");

                string company = info.COMPANY; //公司ID
                string startDate = info.startDate.ToShortDateString();  //统计开始时间
                string beginDate = info.startDate.AddMonths(-3).ToShortDateString();  //向前推三个月

                //根据页数展示显示数据
                int startNUM = info.PageNum * info.ShowNum - info.ShowNum;
                int endNUM = startNUM + info.ShowNum;

                var sqlNum = "select a1.salesperson code ,a1.num,a1.rn,a2.name from ( select  salesperson,   num, rownum rn from ( select salesperson, count(salesperson) num  from (  select ccc.*,row_number() over(partition by  customer, company order by eff_date desc) cn from  " +
                      " (select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join   (  select * from (select customer, doc_date, wgt, amt, company, row_number() over(partition by  customer, company order by doc_date desc) cn from zb_feed_sale bbb where customer in " +
                      " (select  customer from(select  distinct customer from zb_feed_sale where company = '" + company + "' and  doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd'))aaa" +
                      "  where customer not in (select distinct customer  from zb_feed_sale where company = '" + company + "' and doc_date > to_date('" + startDate + "', 'yyyy-mm-dd')) and customer not like '9%' " +
                      "  ) and bbb.company = '" + company + "' and bbb.doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd')  order by bbb.customer, bbb.company, bbb.doc_date desc ) where cn = 1 ) bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date)   )ccc ) where cn = 1  group by salesperson order by num desc )   ) a1 join zb_feed_salesperson a2 on(a1.salesperson = a2.code) and " +
                      " a2.company = '" + company + "'  where rn >" + startNUM + " and rn<=" + endNUM +" order by a1.rn";
                 
                var query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum);
                List<ZB_FEED_CUSTOMER> result = query.ToList();
                num5saleslost = "";
                foreach (var ZB_FEED_CUSTOMER in result)
                {
                    num5saleslost += "'" + ZB_FEED_CUSTOMER.CODE + "',";
                }
                num5saleslost = (num5saleslost.Length > 1) ? num5saleslost.Substring(0, num5saleslost.Length - 1) : "";
                return Succeed(result);
            }
        }


        /// <summary>
        /// 获取业务员丢失客户详细统计
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaleLostDetailByPage(ZB_FEED_CUSTOMER info)
        {
            using (StarOracle db = new StarOracle())
            {

                string company = info.COMPANY; //公司ID
                string startDate = info.startDate.ToShortDateString();  //统计开始时间
                string beginDate = info.startDate.AddMonths(-3).ToShortDateString();  //向前推三个月 
                var sqlNum = "select customer, monthall,wgt ,company,name,salesperson   from (SELECT t.customer, t1.salesperson, TO_CHAR(T.doc_date,'YYYY-MM') monthall, sum(wgt) wgt  "+
                        " from zb_feed_sale t join (select customer,salesperson,company from (select ccc.*, row_number() over(partition by  customer, company order by eff_date desc) cn from(" +
                        " select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join" +
                        " ( select * from (select customer, doc_date, wgt, amt, company, row_number() over(partition by  customer, company order by doc_date desc) cn from zb_feed_sale bbb where customer in  " +
                            " (select  customer from(select  distinct customer from zb_feed_sale where company = '" + company + "' and  doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd'))aaa" +
                          "   where customer not in (select distinct customer  from zb_feed_sale where company = '" + company + "' and doc_date > to_date('" + startDate + "', 'yyyy-mm-dd')) and customer not like '9%'" +
                         "    )    and bbb.company = '" + company + "' and bbb.doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd')  order by bbb.customer, bbb.company, bbb.doc_date desc" +
                        "  ) where cn = 1 ) bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date) )ccc   ) " +
                        "  where cn = 1    and salesperson in ("+ num5saleslost + ") ) t1 on(t.customer = t1.customer and t.company = t1.company)  " +
                         " where doc_date >= to_date('"+ beginDate + "', 'YYYY-MM-dd') group by t.customer, t1.salesperson, TO_CHAR(T.doc_date, 'YYYY-MM') )aaa join zb_feed_customer bbb on aaa.customer = bbb.code and company = '" + company + "'";

                List<ZB_FEED_CUSTOMER> query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum).ToList();

                List<ZB_FEED_CUSTOMER> num = query.GroupBy(r => r.Customer).Select(r => r.First()).ToList();  //統計客户个数

                var aaa = query.GroupBy(r => r.Customer).Select(g => (new { Customer = g.Key, Num = g.Sum(a => a.WGT) })).ToList();

                for (int i = 0; i < num.Count; i++)
                {
                    for (int j = 0; j < aaa.Count; j++)
                    {
                        if (num[i].Customer == aaa[j].Customer)
                        {
                            num[i].Num = (int)aaa[j].Num;
                            break;
                        }
                    }
                }

                List<ZB_FEED_CUSTOMER> result = new List<ZB_FEED_CUSTOMER>();

                for (int i = 0; i < num.Count; i++)
                {
                    for (System.DateTime j = info.startDate.AddMonths(-3); j <= DateTime.Now; j = j.AddMonths(1))
                    {
                        ZB_FEED_CUSTOMER zbinfo = new ZB_FEED_CUSTOMER();
                        zbinfo.Customer = num[i].Customer;  //客户编号
                        zbinfo.Salesperson = num[i].Salesperson;
                        zbinfo.Num = num[i].Num;
                        zbinfo.monthall = j.ToString("yyyy-MM");  //销售月份
                        zbinfo.WGT = 0;

                        result.Add(zbinfo);
                    }
                }
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < query.Count; j++)
                    {
                        if (result[i].Customer == query[j].Customer && result[i].monthall == query[j].monthall)
                        {
                            result[i].WGT = query[j].WGT;  //该月销售重量
                            result[i].Salesperson = query[j].Salesperson;  //业务员
                            result[i].NAME = query[j].NAME;  //客户姓名
                            break;
                        }
                    }
                }

                int wgt = (int)query.OrderByDescending(m => m.WGT).First().WGT;
                return Succeed(result, wgt, "");

            }
        }
        #endregion

        #region 丢失客户统计  近两个月没提货

        /// <summary>
        /// 某个时间段内某公司销售丢失客户统计
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SalesLostNumAllNew(ZB_FEED_CUSTOMER info)
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.COMPANY; //公司ID
                string startDate = DateTime.Now.AddDays(-60).ToShortDateString() ;  
                string beginDate = info.startDate.ToShortDateString();  //

                var sqlNum = "select  salesperson,   num, rownum rn from ( select salesperson, count(salesperson) num  from (  select ccc.*,row_number() over(partition by  customer, company order by eff_date desc) cn from  " +
                        " (select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join   (  select * from (select customer, doc_date, wgt, amt, company, row_number() over(partition by  customer, company order by doc_date desc) cn from zb_feed_sale bbb where customer in " +
                        " (select  customer from(select  distinct customer from zb_feed_sale where company = '" + company + "' and  doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd'))aaa" +
                        "  where customer not in (select distinct customer  from zb_feed_sale where company = '" + company + "' and doc_date > to_date('" + startDate + "', 'yyyy-mm-dd')) and customer not like '9%' " +
                        "  ) and bbb.company = '" + company + "' and bbb.doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd')  order by bbb.customer, bbb.company, bbb.doc_date desc ) where cn = 1 ) bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date)   )ccc ) where cn = 1  group by salesperson order by num desc   ) ";

                var query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum);

                return Succeed(query.Count());
            }
        }



        /// <summary>
        /// 根据分页返回当前页面 销售员丢失客户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult LostCoustomerByPageNew(ZB_FEED_CUSTOMER info)   //当前页码，显示条数
        {
            using (StarOracle db = new StarOracle())
            {
                string sss = GetWebConfigValueByKey("DataStartDate");

                string company = info.COMPANY; //公司ID
                string startDate = DateTime.Now.AddDays(-60).ToShortDateString();
                string beginDate = info.startDate.ToShortDateString();  //

                //根据页数展示显示数据
                int startNUM = info.PageNum * info.ShowNum - info.ShowNum;
                int endNUM = startNUM + info.ShowNum;

                var sqlNum = "select a1.salesperson code ,a1.num,a1.rn,a2.name from ( select  salesperson,   num, rownum rn from ( select salesperson, count(salesperson) num  from (  select ccc.*,row_number() over(partition by  customer, company order by eff_date desc) cn from  " +
                      " (select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join   (  select * from (select customer, doc_date, wgt, amt, company, row_number() over(partition by  customer, company order by doc_date desc) cn from zb_feed_sale bbb where customer in " +
                      " (select  customer from(select  distinct customer from zb_feed_sale where company = '" + company + "' and  doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd'))aaa" +
                      "  where customer not in (select distinct customer  from zb_feed_sale where company = '" + company + "' and doc_date > to_date('" + startDate + "', 'yyyy-mm-dd')) and customer not like '9%' " +
                      "  ) and bbb.company = '" + company + "' and bbb.doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd')  order by bbb.customer, bbb.company, bbb.doc_date desc ) where cn = 1 ) bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date)   )ccc ) where cn = 1  group by salesperson order by num desc )   ) a1 join zb_feed_salesperson a2 on(a1.salesperson = a2.code) and " +
                      " a2.company = '" + company + "' order by a1.rn";   //+ "'  where rn >" + startNUM + " and rn<=" + endNUM 

                var query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum);
                int num = query.Count();

                query = query.Where(s => s.RN > startNUM  & s.RN<= endNUM);
                List<ZB_FEED_CUSTOMER> result = query.ToList();
                num5saleslost = "";
                foreach (var ZB_FEED_CUSTOMER in result)
                {
                    num5saleslost += "'" + ZB_FEED_CUSTOMER.CODE + "',";
                }
                num5saleslost = (num5saleslost.Length > 1) ? num5saleslost.Substring(0, num5saleslost.Length - 1) : "";
                

                return Succeed(result,num,"");
            }
        }


        /// <summary>
        /// 获取业务员丢失客户详细统计
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaleLostDetailByPageNew(ZB_FEED_CUSTOMER info)
        {
            using (StarOracle db = new StarOracle())
            {

                string company = info.COMPANY; //公司ID
                string startDate = DateTime.Now.AddDays(-60).ToShortDateString();
                string beginDate = info.startDate.ToShortDateString();  //
                var sqlNum = "select customer, monthall,wgt ,company,name,salesperson   from (SELECT t.customer, t1.salesperson, TO_CHAR(T.doc_date,'YYYY-MM') monthall, sum(wgt) wgt  " +
                        " from zb_feed_sale t join (select customer,salesperson,company from (select ccc.*, row_number() over(partition by  customer, company order by eff_date desc) cn from(" +
                        " select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join" +
                        " ( select * from (select customer, doc_date, wgt, amt, company, row_number() over(partition by  customer, company order by doc_date desc) cn from zb_feed_sale bbb where customer in  " +
                            " (select  customer from(select  distinct customer from zb_feed_sale where company = '" + company + "' and  doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd'))aaa" +
                          "   where customer not in (select distinct customer  from zb_feed_sale where company = '" + company + "' and doc_date > to_date('" + startDate + "', 'yyyy-mm-dd')) and customer not like '9%'" +
                         "    )    and bbb.company = '" + company + "' and bbb.doc_date > to_date('" + beginDate + "', 'yyyy-mm-dd')  order by bbb.customer, bbb.company, bbb.doc_date desc" +
                        "  ) where cn = 1 ) bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date) )ccc   ) " +
                        "  where cn = 1    and salesperson in (" + num5saleslost + ") ) t1 on(t.customer = t1.customer and t.company = t1.company)  " +
                         " where doc_date >= to_date('" + beginDate + "', 'YYYY-MM-dd') group by t.customer, t1.salesperson, TO_CHAR(T.doc_date, 'YYYY-MM') )aaa join zb_feed_customer bbb on aaa.customer = bbb.code and company = '" + company + "'";

                List<ZB_FEED_CUSTOMER> query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum).ToList();

                List<ZB_FEED_CUSTOMER> num = query.GroupBy(r => r.Customer).Select(r => r.First()).ToList();  //統計客户个数

                var aaa = query.GroupBy(r => r.Customer).Select(g => (new { Customer = g.Key, Num = g.Sum(a => a.WGT) })).ToList();

                for (int i = 0; i < num.Count; i++)
                {
                    for (int j = 0; j < aaa.Count; j++)
                    {
                        if (num[i].Customer == aaa[j].Customer)
                        {
                            num[i].Num = (int)aaa[j].Num;
                            break;
                        }
                    }
                }

                List<ZB_FEED_CUSTOMER> result = new List<ZB_FEED_CUSTOMER>();

                for (int i = 0; i < num.Count; i++)
                {
                    for (System.DateTime j = info.startDate; j <= DateTime.Now; j = j.AddMonths(1))
                    {
                        ZB_FEED_CUSTOMER zbinfo = new ZB_FEED_CUSTOMER();
                        zbinfo.Customer = num[i].Customer;  //客户编号
                        zbinfo.Salesperson = num[i].Salesperson;
                        zbinfo.Num = num[i].Num;
                        zbinfo.monthall = j.ToString("yyyy-MM");  //销售月份
                        zbinfo.WGT = 0;

                        result.Add(zbinfo);
                    }
                }
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < query.Count; j++)
                    {
                        if (result[i].Customer == query[j].Customer && result[i].monthall == query[j].monthall)
                        {
                            result[i].WGT = query[j].WGT;  //该月销售重量
                            result[i].Salesperson = query[j].Salesperson;  //业务员
                            result[i].NAME = query[j].NAME;  //客户姓名
                            break;
                        }
                    }
                }

                int wgt = (int)query.OrderByDescending(m => m.WGT).First().WGT;
                return Succeed(result, wgt, "");

            }
        }

        #endregion


    }


    public class LS
    {
        public int Num { get; set; }
         
        public string Customer { get; set; }
    }
}