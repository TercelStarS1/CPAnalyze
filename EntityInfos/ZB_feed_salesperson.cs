using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 销售员编码数据
    /// </summary>
    [SZTable(IsView = true)]  ///   [SZTable(IsView =true)]这个特性的意思是如果你这个表已经存在了。就不创建了，如果需要我给你自动创建表。就不带这个
    public class ZB_FEED_SALESPERSON
    { 
        [SZColumn(MaxLength = 10)]
        public int ID { get; set; }
        /// <summary>
        /// 销售员编码
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string CODE { get; set; }
        /// <summary>
        /// 销售员姓名
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string NAME { get; set; }
        /// <summary>
        /// 公司编码
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string COMPANY { get; set; }
    }
    /// <summary>
    /// 搜索模型
    /// </summary>
    public class ZB_FEED_SALESPERSONModel
    {
        /// <summary>
        ///  
        /// </summary>
        public string COMPANY { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string NAME { get; set; }

        public string CODE { get; set; }
    }
}
