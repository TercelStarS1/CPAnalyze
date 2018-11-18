using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 客户与销售员关系
    /// </summary>
    [SZTable(IsView = true)] 
    public class ZB_FEED_SALESP_CUST
    {
        [SZColumn(MaxLength = 255)]
        public int ID { get; set; }
        /// <summary>
        /// 销售员编号
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string SALESPERSON { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string CUSTOMER { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public DateTime EFF_DATE { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string COMPANY { get; set; }
    }
    /// <summary>
    /// 搜索模型
    /// </summary>
    public class ZB_FEED_SALESP_CUSTModel
    {
        /// <summary>
        ///  
        /// </summary>
        public string SALESPERSON { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string CUSTOMER { get; set; }
    }
}
