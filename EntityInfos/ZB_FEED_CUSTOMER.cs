using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 销售客户编码数据
    /// </summary>
    [SZTable(IsView = true)]  ///   [SZTable(IsView =true)]这个特性的意思是如果你这个表已经存在了。就不创建了，如果需要我给你自动创建表。就不带这个
    public class ZB_FEED_CUSTOMER :Basic
    { 
        [SZColumn(MaxLength = 10)]
        public int ID { get; set; }
        [SZColumn(MaxLength = 255)]
        public string CODE { get; set; }
        [SZColumn(MaxLength = 255)]
        public string NAME { get; set; }
        [SZColumn(MaxLength = 255)]
        public string COMPANY { get; set; }


        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        /// <summary>
        /// 开发月份
        /// </summary>
        public string monthall { get; set; }

        /// <summary>
        /// 开发数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 提货重量
        /// </summary>
        public decimal WGT { get; set; }

        public string Customer { get; set; }

        public string Salesperson { get; set; }
    }
    /// <summary>
    /// 搜索模型
    /// </summary>
    public class ZB_FEED_CUSTOMERModel
    {
        /// <summary>
        ///  
        /// </summary>
        public string PROV_COUNT { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string CO_COUNT { get; set; }
    }
}
