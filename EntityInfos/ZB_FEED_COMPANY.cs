using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    ///  公司编码
    /// </summary>
    [SZTable(IsView = true)]  ///   [SZTable(IsView =true)]这个特性的意思是如果你这个表已经存在了。就不创建了，如果需要我给你自动创建表。就不带这个
    public class ZB_FEED_COMPANY
    { 
        [SZColumn(MaxLength = 255)]
        public string CODE { get; set; }
        [SZColumn(MaxLength = 255)]
        public string NAME { get; set; }
        [SZColumn(MaxLength = 255)]
        public string DB { get; set; }
        [SZColumn(MaxLength = 255)]
        public string SCHEMA { get; set; }

        [SZColumn(MaxLength = 255)]
        public string PASSWORD { get; set; }

    }
    /// <summary>
    /// 用户搜索模型
    /// </summary>
    public class ZB_COMPANYModel
    {
        /// <summary>
        ///  
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string DB { get; set; }
    }
}
