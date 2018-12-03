using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 项目表
    /// </summary> 
    [SZTable(IsView = true)]
    public class PBE_PROJECT 
    { 
        /// <summary>
        /// 编码
        /// </summary>
        public decimal ID { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string USER1 { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string PROJECT { get; set; }
        /// <summary>
        /// 进舍日龄21
        /// </summary>
        public decimal INDAY { get; set; }
        /// <summary>
        /// 进舍重量6.5kg
        /// </summary>
        public decimal INWGT { get; set; }
        /// <summary>
        /// 出栏日龄174
        /// </summary>
        public decimal OUTDAY { get; set; }
        /// <summary>
        /// 出栏标准重120kg
        /// </summary>
        public decimal OUTWGT { get; set; }

        /// <summary>
        /// 进猪数量
        /// </summary>
        public decimal PIGLETQTY { get; set; }

        /// <summary>
        /// 进猪日期
        /// </summary>
        public DateTime INDATE { get; set; }

        /// <summary>
        /// 淘汰率
        /// </summary>
        public decimal CULLINGRATE { get; set; }

        /// <summary>
        /// 一级猪率
        /// </summary>
        public decimal PERFECTRATE { get; set; }


        /// <summary>
        /// 非一级猪率
        /// </summary>
        public decimal NOPERFRATE { get; set; }

        /// <summary>
        /// 非一级猪均重与标准重的比率
        /// </summary>
        public decimal NOPERFWGTRATE { get; set; }

        /// <summary>
        /// 非一级猪均价与市场价的比率
        /// </summary>
        public decimal NOPERFPRICERATE { get; set; }

        /// <summary>
        /// 代养费
        /// </summary>
        public decimal ADJUSTFEE { get; set; } 
    } 
}
