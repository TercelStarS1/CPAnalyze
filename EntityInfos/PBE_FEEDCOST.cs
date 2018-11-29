using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 项目饲料表
    /// </summary> 
    public class PBE_FEEDCOST
    { 
        /// <summary>
        ///  
        /// </summary>
        public decimal ID { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string PROJECT { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string FEED { get; set; }  
        /// <summary>
        ///  
        /// </summary>
        public DateTime FEEDNAME { get; set; }
        /// <summary>
        ///  成本
        /// </summary>
        public decimal COST { get; set; }
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
