using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 生长耗料标准
    /// </summary> 
    [SZTable(IsView = true)]
    public class PBE_STDWITHAGE 
    { 
        /// <summary>
        /// 日龄
        /// </summary>
        public decimal DAYS { get; set; }
        /// <summary>
        /// 标准重
        /// </summary>
        public decimal PIG_WGT { get; set; }
        /// <summary>
        /// 日增重
        /// </summary>
        public decimal PIG_WGTINC { get; set; }  
        /// <summary>
        /// 饲料
        /// </summary>
        public string FEED { get; set; }
        /// <summary>
        /// 日耗料
        /// </summary>
        public decimal FEED_WGTINC { get; set; }
        /// <summary>
        /// 日耗料
        /// </summary>
        public decimal FEED_WGT { get; set; }

        /// <summary>
        ///  当日淘汰占总淘汰的百分比----总淘汰率在每个测算项目中指定，设为X
        /// </summary>
        public decimal OUTRATEDAY { get; set; }
        /// <summary>
        ///  累计淘汰占总淘汰的百分比
        /// </summary>
        public decimal OUTRATEACCU { get; set; }
        /*
        /// <summary>
        ///  当日淘汰占总体的百分比       outrateday*x
        /// </summary>
        public double EX_PIGOUTRATEDAY { get; set; }
        /// <summary>
        ///  累计淘汰占总体的百分比       outrateaccu*x
        /// </summary>
        public double EX_PIGOUTRATEACCU { get; set; }
        /// <summary>
        /// 头数
        /// </summary>
        public double EX_PIGOUTQTYDAY { get; set; }
        /// <summary>
        ///  累计头数
        /// </summary>
        public double EX_PIGOUTQTYACCU { get; set; }
        
        /// <summary>
        ///  当日用药金额
        /// </summary>
        public decimal EX_MEDIAMTDAY { get; set; }
        /// <summary>
        ///  累计用药金额
        /// </summary>
        public decimal EX_MEDIAMTACCU { get; set; }

        /// <summary>
        ///  累计耗料
        /// </summary>
        public decimal EX_FEEDWGTACCU { get; set; }
        */
    }
}
