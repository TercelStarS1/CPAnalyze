using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 销售数据
    /// </summary>
    [SZTable(IsView = true)] 
    public class ZB_FEED_SALE: Basic
    {
        [SZColumn(MaxLength = 255)]
        public int ID { get; set; }
      
        /// <summary>
        /// 客户
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string CUSTOMER { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string PRODUCT { get; set; }
        /// <summary>
        /// 发票号
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string DOC_NUM { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DOC_DATE { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public decimal WGT { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public decimal AMT { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string COMPANY { get; set; }


        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        public string CUSTOMERName { get; set; }

        public string CUSTOMERValue { get; set; }

        public string SalesName { get; set; }
    }
    /// <summary>
    /// 搜索模型
    /// </summary>
    public class ZB_FEED_SALEModel
    {
        /// <summary>
        ///  
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string NAME { get; set; }
    }
}
