using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 产品编码
    /// </summary>
    [SZTable(IsView = true)]  
    public class ZB_FEED_PRODUCT
    {
        [SZColumn(MaxLength = 255)]
        public int ID { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        [SZColumn(MaxLength = 100)]
        public string CODE { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [SZColumn(MaxLength = 100)]
        public string NAME { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        [SZColumn(MaxLength = 100)]
        public string COMPANY { get; set; } 


    }
    /// <summary>
    /// 搜索模型
    /// </summary>
    public class ZB_FEED_PRODUCTModel
    {
        /// <summary>
        ///  
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string NAME { get; set; }

        public string COMPANY { get; set; }
    }
}
