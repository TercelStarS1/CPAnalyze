using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 饲料表
    /// </summary> 
    [SZTable(IsView = true)]
    public class PBE_FEED 
    {
        /// <summary>
        /// ID
        /// </summary>
        public decimal ID { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal COST { get; set; }  
        /// <summary>
        /// 开始使用日
        /// </summary>
        public decimal DAY_BEGIN { get; set; }
        /// <summary>
        /// 结束使用日
        /// </summary>
        public decimal DAY_END { get; set; }
        /// <summary>
        /// 使用天数
        /// </summary>
        public decimal DAYS { get; set; }
        /// <summary>
        /// 使用公斤数
        /// </summary>
        public decimal WGT { get; set; }

        /// <summary>
        /// 饲料厂
        /// </summary>
        public decimal FEEDFAC { get; set; }
        /// <summary>
        /// 养殖公司
        /// </summary>
        public decimal MANAGEMENT { get; set; }

        /// <summary>
        /// 代养户
        /// </summary>
        public decimal FARM { get; set; }
    } 
}
