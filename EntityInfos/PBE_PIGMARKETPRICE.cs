using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 项目-成品猪
    /// </summary> 
    [SZTable(IsView = true)]
    public class PBE_PIGMARKETPRICE
    { 
        /// <summary>
        /// 编码
        /// </summary>
        public string PROJECT { get; set; }
        
        /// <summary>
        /// 合同价
        /// </summary>
        public decimal CONTRACT { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal MARKET { get; set; }
    } 
}
