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
        private static string num5sales = "";  //开发能力分页用 暂存 开发员编码
        private static string num5saleslost = ""; //丢失能力分页用 暂存 开发员编码
        private static string areaNumNew = "";  //地区开发能力分页用 暂存 地区编码
        private static string areaNumLost = "";  //地区丢失能力分页用 暂存 地区编码

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
                string startDate = "";
                int numjg = (int)info.Num;
                if (numjg == 1)
                {
                    startDate = DateTime.Now.AddDays(-365).ToShortDateString();
                }
                else {
                    startDate = DateTime.Now.AddDays(-30 * numjg).ToShortDateString();
                }
                string beginDate = DateTime.Now.AddDays(-60).ToShortDateString(); 
                
                var sqlNum = "select COMPANY,name,Num from( select count(customer) num , company from  (select * from(select customer, company,min(doc_date) doc_date from zb_feed_sale " +
                    " where customer not like '9%' group by customer, company ) "+
                    " ) where doc_date > to_date('" + startDate + "', 'YYYY/MM/DD')  group by company ) t1 join ZB_FEED_COMPANY t2 on(t1.company = t2.code) order by company";

                var lastNum = "select count(t1.customer) from (select * from( select customer, company,min(doc_date) doc_date from zb_feed_sale  where customer not like '9%' group by customer, company "+
                    " ) where doc_date > to_date('"+startDate+"', 'YYYY/MM/DD')"+
                    " )t1 join (select customer, company from(select customer, company, max(doc_date)doc_date from zb_feed_sale  where customer not like '9%' group by customer, company"+
                    " ) where doc_date > to_date('"+ beginDate + "', 'YYYY/MM/DD')  ) t2 on(t1.customer = t2.customer and t1.company = t2.company)";
                var query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum).ToList();
                int num = query.Sum(g => g.Num);
                string allnum = db.ExecuteScalar(lastNum).ToString();
                return Succeed(query, num, allnum);
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

                var sqlNum1 = "select * from(select salesperson code, num, CUSTOMERValue, name , rownum rn from (  select salesperson, count(salesperson) num ,sum(wgt) CUSTOMERValue from(select ccc.*, row_number() over(partition by  customer, company order by eff_date desc) cn from(select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join" +
                    " (select * from(select bbb.*, row_number() over(partition by customer, company order by doc_num) cn from zb_feed_sale bbb where company = '" + company + "' and customer not like '9%') where cn = 1 and doc_date > to_date('" + sss + "', 'YYYY/MM/DD'))" +
                    " bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date)  ) ccc ) " +
                    " where cn = 1 and doc_date between to_date('"+ startDate + "', 'yyyy-mm-dd')  and to_date('" + endDate + "', 'yyyy-mm-dd')  group by salesperson order by num desc ,CUSTOMERValue desc  ) " +
                    " ddd left join zb_feed_salesperson fff on(ddd.salesperson = fff.code) and fff.company = '"+ company + "' order by num desc, CUSTOMERValue desc " +
                    " ) ";

                var sqlNum = "select  code,num ,name , rownum rn from (select code,num ,name from(select salesperson, count(salesperson) num from(select *  from(select t3.*,row_number() over(partition by  customer,company order by eff_date desc) cn from  (select t1.customer,t1.company,t1.doc_date,t2.eff_date,salesperson from(select * from  ( " +
                  " select * from(select customer, company, min(doc_date)doc_date from zb_feed_sale   where company = '" + company + "' and customer not like '9%'  group by customer, company ) " +
                  " ) where doc_date >= to_date('" + startDate + "', 'YYYY/MM/DD')  and doc_date < to_date('" + endDate + "', 'YYYY/MM/DD') " +
                  " ) t1 join (select * from zb_feed_salesp_cust where company = '" + company + "') t2 on(t1.company = t2.company and t1.customer = t2.customer and t2.eff_date <= t1.doc_date) ) t3  ) where cn = 1  ) group by salesperson )t5 join zb_feed_salesperson t6 "+
                  " on(t5.salesperson = t6.code and t6.company='" + company + "') order by num desc )";

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
                var sqlNum1 = "select r.*, zc.name from (SELECT T.customer, TO_CHAR(T.doc_date, 'YYYY-MM') monthall, sum(wgt) wgt, TT.salesperson, T.company FROM zb_feed_sale T "+
                        " right join(select salesperson, customer from (  select ccc.*, row_number() over(partition by  customer, company order by eff_date desc) cn from(" +
                        " select aaa.salesperson, aaa.customer, aaa.company, aaa.eff_date, bbb.doc_date, bbb.wgt from zb_feed_salesp_cust aaa join" +
                        " (select* from (select bbb.*, row_number() over(partition by customer, company order by doc_num) cn from zb_feed_sale bbb where company = '"+ company + "' and customer not like '9%') where cn = 1 and doc_date > to_date('"+sss+"', 'YYYY/MM/DD')) " +
                        " bbb on(aaa.customer = bbb.customer and aaa.company = bbb.company and aaa.eff_date <= bbb.doc_date)  ) ccc ) " +
                        " where cn = 1 and doc_date between to_date('"+ startDate + "', 'yyyy-mm')  and to_date('"+ endDate + "', 'yyyy-mm')   and salesperson in("+ num5sales + ") " +
                        " ) TT ON(T.CUSTOMER = TT.CUSTOMER) group by TO_CHAR(T.doc_date,'YYYY-MM') ,T.company,T.customer,TT.salesperson) r join zb_feed_customer zc on(r.customer = zc.code and r.company= zc.company) order by r.customer,r.monthall";

                var sqlNum = "select * from (select t5.customer,monthall,wgt,t5.name,t6.salesperson from(select customer,monthall,wgt,name from(SELECT customer, TO_CHAR(doc_date,'YYYY-MM') monthall,sum(wgt) wgt   from  (select t1.* from zb_feed_sale t1 join (select * from  ( " +
                            " select * from(select customer, company, min(doc_date)doc_date from zb_feed_sale   where company = '" + company + "' and customer not like '9%'  group by customer, company ) " +
                            " ) where doc_date >= to_date('" + startDate + "', 'YYYY/MM')  and doc_date < to_date('" + endDate + "', 'YYYY/MM')) t2 " +
                            " on t1.customer = t2.customer and t1.company = t2.company  where t1.doc_date >= to_date('" + startDate + "', 'YYYY/MM')  and t1.doc_date < to_date('" + endDate + "', 'YYYY/MM'))" +
                            " group by TO_CHAR(doc_date, 'YYYY-MM') ,customer) t3 join zb_feed_customer t4 on(t3.customer = t4.code and t4.company = '" + company + "')" +
                            " ) t5 join(select customer, salesperson, name from (select * from(select t3.*, row_number() over(partition by  customer, company order by eff_date desc) cn from(select t1.customer, t1.company, t1.doc_date, t2.eff_date, salesperson from(select * from(" +
                            " select * from(select customer, company, min(doc_date) doc_date from zb_feed_sale   where company = '" + company + "' and customer not like '9%'  group by customer, company)" +
                            " ) where doc_date >= to_date('" + startDate + "', 'YYYY/MM')  and  doc_date < to_date('" + endDate + "', 'YYYY/MM')" +
                            " ) t1 join(select * from zb_feed_salesp_cust where company = '" + company + "') t2 on(t1.company = t2.company and t1.customer = t2.customer and t2.eff_date <= t1.doc_date)) t3) where cn = 1 ) t5 join zb_feed_salesperson t6 on(t5.salesperson = t6.code))t6 on (t5.customer = t6.customer)order by customer, monthall" +
                            " )where salesperson in ("+ num5sales + ")";
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



        /// <summary>
        /// 根据分页返回当前页面 地区新客户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult NewCoustomerByArea(ZB_FEED_CUSTOMER info)   //当前页码，显示条数
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

                var sqlNum = "select num,code,name , rownum rn  from  (select num,code,name from (select salesarea, count(salesarea) num from(select *  from(select t3.*,row_number() over(partition by  customer,company order by eff_date desc) cn from  (select t1.customer,t1.company,t1.doc_date,t2.eff_date,substr(t2.salesarea,0,9) salesarea from " +
                    " (select * from(select * from(select customer, company, min(doc_date) doc_date from zb_feed_sale   where company = '"+ company + "' and customer not like '9%'  group by customer, company) "+
                    " ) where doc_date >= to_date('"+ startDate + "', 'YYYY/MM/DD')  and  doc_date < to_date('"+ endDate + "', 'YYYY/MM/DD') "+
                    ") t1 join (select * from zb_feed_salesa_cust where company = '"+ company + "') t2 on(t1.company = t2.company and t1.customer = t2.customer and t2.eff_date <= t1.doc_date)) t3  ) where cn = 1) t4 group by salesarea  ) t5 join zb_feedv_salesarea t6 on(t5.salesarea = substr(t6.code, 0, 9)) order by num desc )";
                 
                var query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum);
                int num = query.Count();

                query = query.Where(s => s.RN > startNUM & s.RN <= endNUM);

                List<ZB_FEED_CUSTOMER> result = query.ToList();
                areaNumNew = "";
                foreach (var ZB_FEED_CUSTOMER in result)
                {
                    areaNumNew += "'" + ZB_FEED_CUSTOMER.CODE + "',";
                }
                areaNumNew = (areaNumNew.Length > 1) ? areaNumNew.Substring(0, areaNumNew.Length - 1) : "";
                return Succeed(result, num, "");
            }
        }


        /// <summary>
        /// 获取某公司前五名销售统计
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AreaDetailByPage(ZB_FEED_CUSTOMER info)
        {
            using (StarOracle db = new StarOracle())
            {

                string company = info.COMPANY; //公司ID
                string startDate = info.startDate.Year + "-" + info.startDate.Month;
                System.DateTime end = info.endDate.AddMonths(1);
                string endDate = end.Year + "-" + end.Month;
                var sqlNum = "select * from ( select t5.customer,monthall,wgt,t5.name,t6.code from(select customer,monthall,wgt,name from(SELECT customer, TO_CHAR(doc_date,'YYYY-MM') monthall,sum(wgt) wgt   from  (select t1.* from zb_feed_sale t1 join (select * from  ( " +
                        " select * from(select customer, company, min(doc_date)doc_date from zb_feed_sale   where company = '"+ company + "' and customer not like '9%'  group by customer, company ) "+  
                        " ) where doc_date >= to_date('"+ startDate + "', 'YYYY/MM')  and doc_date < to_date('" + endDate + "', 'YYYY/MM')) t2" +
                        " on t1.customer = t2.customer and t1.company = t2.company  where t1.doc_date >= to_date('" + startDate + "', 'YYYY/MM')  and t1.doc_date < to_date('" + endDate + "', 'YYYY/MM'))" +
                        " group by TO_CHAR(doc_date, 'YYYY-MM') ,customer) t3 join zb_feed_customer t4 on(t3.customer = t4.code and t4.company = '" + company + "')) t5 join(select customer, code, name from (select * from(select t3.*, row_number() over(partition by  customer, company order by eff_date desc) cn from(select t1.customer, t1.company, t1.doc_date, t2.eff_date, substr(t2.salesarea, 0, 9) salesarea from(select * from(" +
                        "   select * from(select customer, company, min(doc_date) doc_date from zb_feed_sale   where company = '" + company + "' and customer not like '9%'  group by customer, company)" +
                        "   ) where doc_date >= to_date('" + startDate + "', 'YYYY/MM')  and  doc_date < to_date('" + endDate + "', 'YYYY/MM') " +
                        "   ) t1 join(select * from zb_feed_salesa_cust where company = '" + company + "') t2 on(t1.company = t2.company and t1.customer = t2.customer and t2.eff_date <= t1.doc_date)) t3) where cn = 1 ) t5 join zb_feedv_salesarea t6 on(t5.salesarea = substr(t6.code, 0, 9)))t6 on (t5.customer = t6.customer) order by customer,monthall"+
                        " ) where code in ("+ areaNumNew + ")";

                List < ZB_FEED_CUSTOMER> query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum).ToList();

                List<ZB_FEED_CUSTOMER> num = query.GroupBy(r => r.Customer).Select(r => r.First()).ToList();  //統計新客户个数

                var aaa = query.GroupBy(r => r.Customer).Select(g => (new { Customer = g.Key, Num = g.Sum(a => a.WGT) })).ToList();

                var bbbb = query.Where(s => s.monthall == info.endDate.ToString("yyyy-MM") || s.monthall == info.endDate.AddMonths(-1).ToString("yyyy-MM")).Select(r => new { CODE = r.CODE, Customer = r.Customer }).Distinct().ToList();

                var ccc = bbbb.GroupBy(g => g.CODE).Select(g => (new { Customer = g.Key, Num = g.Count() })).ToList();

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
                        zbinfo.CODE = num[i].CODE;  //客户编号
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
                            result[i].NAME = query[j].NAME;  //客户姓名
                            result[i].Customer = query[j].Customer;  //客户姓名
                            result[i].CODE = query[j].CODE;  //客户姓名
                            break;
                        }
                    }
                }

                int wgt = (int)query.OrderByDescending(m => m.WGT).First().WGT;
                return Succeed(result, data2, wgt, "");

            }
        }


        /// <summary>
        /// 根据分页返回当前页面 地区新客户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult LostCoustomerByArea(ZB_FEED_CUSTOMER info)   //当前页码，显示条数
        {
            using (StarOracle db = new StarOracle())
            {
                string company = info.COMPANY; //公司ID
                string startDate = DateTime.Now.AddDays(-60).ToShortDateString();
                string beginDate = info.startDate.ToShortDateString();  

                //根据页数展示显示数据
                int startNUM = info.PageNum * info.ShowNum - info.ShowNum;
                int endNUM = startNUM + info.ShowNum;

                var sqlNum = "select num,code,name , rownum rn  from (select num,code,name from (select salesarea, count(salesarea) num from( select *  from(select t3.*,row_number() over(partition by  customer,company order by eff_date desc) cn from  " +
                            " (select t1.customer, t1.company, t1.doc_date, t2.eff_date, substr(t2.salesarea, 0, 9) salesarea from(select * from(select customer, company, max(doc_date) doc_date from zb_feed_sale" +
                            " where company = '"+ company + "' and doc_date >= to_date('" + beginDate + "', 'YYYY/MM/DD') and customer not like '9%'  group by customer, company" +
                            " ) where  doc_date < to_date('"+ startDate + "', 'yyyy-mm-dd')" +
                            " ) t1 join(select * from zb_feed_salesa_cust where company = '" + company + "') t2 on(t1.company = t2.company and t1.customer = t2.customer and t2.eff_date <= t1.doc_date)) t3  ) where cn = 1) t4 group by salesarea  ) t5 join zb_feedv_salesarea t6 on(t5.salesarea = substr(t6.code, 0, 9)) order by num desc  )";

                var query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum);
                int num = query.Count();

                query = query.Where(s => s.RN > startNUM & s.RN <= endNUM);

                List<ZB_FEED_CUSTOMER> result = query.ToList();
                areaNumLost = "";
                foreach (var ZB_FEED_CUSTOMER in result)
                {
                    areaNumLost += "'" + ZB_FEED_CUSTOMER.CODE + "',";
                }
                areaNumLost = (areaNumLost.Length > 1) ? areaNumLost.Substring(0, areaNumLost.Length - 1) : "";
                return Succeed(result, num, "");
            }
        }

        /// <summary>
        /// 获取某公司前五名销售统计
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult LostAreaDetailByPage(ZB_FEED_CUSTOMER info)
        {
            using (StarOracle db = new StarOracle())
            {

                string company = info.COMPANY; //公司ID 
                string startDate = DateTime.Now.AddDays(-60).ToShortDateString();
                string beginDate = info.startDate.ToShortDateString();  //

                var sqlNum = "select * from (   select t5.customer,monthall,wgt,t5.name,t6.code from(select customer,monthall,wgt,name from (SELECT customer, TO_CHAR(doc_date,'YYYY-MM') monthall,sum(wgt) wgt   from (select t1.* from zb_feed_sale t1 join (select * from (select customer, company,max(doc_date) doc_date from zb_feed_sale"+
                        " where company = '" + company + "' and doc_date >= to_date('" + beginDate + "', 'YYYY/MM/DD') and customer not like '9%'  group by customer, company" +
                        " ) where doc_date< to_date('"+ startDate + "', 'yyyy-mm-dd')   ) t2 on t1.customer = t2.customer and t1.company = t2.company" +
                        " where t1.doc_date >= to_date('" + beginDate + "', 'YYYY/MM/DD')  )" +
                        " group by TO_CHAR(doc_date, 'YYYY-MM') ,customer) t3 join zb_feed_customer t4 on(t3.customer = t4.code and t4.company = '" + company + "')) t5 join(select customer, code, name from (select * from(select t3.*, row_number() over(partition by  customer, company order by eff_date desc) cn from" +
                        "   (select t1.customer, t1.company, t1.doc_date, t2.eff_date, substr(t2.salesarea, 0, 9) salesarea from(select * from(select customer, company, max(doc_date) doc_date from zb_feed_sale" +
                        "   where company = '" + company + "' and doc_date >= to_date('" + beginDate + "', 'YYYY/MM/DD') and customer not like '9%'  group by customer, company" +
                        "   ) where  doc_date < to_date('" + startDate + "', 'yyyy-mm-dd')" +
                        "   ) t1 join(select * from zb_feed_salesa_cust where company = '" + company + "') t2 on(t1.company = t2.company and t1.customer = t2.customer and t2.eff_date <= t1.doc_date)) t3) where cn = 1) t5 join zb_feedv_salesarea t6 on(t5.salesarea = substr(t6.code, 0, 9)))t6 on (t5.customer = t6.customer)) where code in (" + areaNumLost + ")";

                List<ZB_FEED_CUSTOMER> query = db.ExecuteSqlToList<ZB_FEED_CUSTOMER>(sqlNum).ToList();

                List<ZB_FEED_CUSTOMER> num = query.GroupBy(r => r.Customer).Select(r => r.First()).ToList();  //統計新客户个数

                var aaa = query.GroupBy(r => r.Customer).Select(g => (new { Customer = g.Key, Num = g.Sum(a => a.WGT) })).ToList();

                //var bbbb = query.Where(s => s.monthall == info.endDate.ToString("yyyy-MM") || s.monthall == info.endDate.AddMonths(-1).ToString("yyyy-MM")).Select(r => new { CODE = r.CODE, Customer = r.Customer }).Distinct().ToList();

                //var ccc = bbbb.GroupBy(g => g.CODE).Select(g => (new { Customer = g.Key, Num = g.Count() })).ToList();

                  


                List<ZB_FEED_CUSTOMER> data2 = new List<ZB_FEED_CUSTOMER>();
                //for (int i = 0; i < ccc.Count; i++)
                //{
                //    ZB_FEED_CUSTOMER c1 = new ZB_FEED_CUSTOMER();
                //    c1.Salesperson = ccc[i].Customer;
                //    c1.Num = ccc[i].Num;
                //    data2.Add(c1);
                //}
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
                        zbinfo.CODE = num[i].CODE;  //客户编号
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
                            result[i].NAME = query[j].NAME;  //客户姓名
                            result[i].Customer = query[j].Customer;  //客户姓名
                            result[i].CODE = query[j].CODE;  //客户姓名
                            break;
                        }
                    }
                }

                int wgt = (int)query.OrderByDescending(m => m.WGT).First().WGT;
                return Succeed(result, data2, wgt, "");

            }
        }



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


   
}