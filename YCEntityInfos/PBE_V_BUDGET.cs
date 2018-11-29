using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    ///  利润
    /// </summary> 
    [SZTable(IsView = true)]
    public class PBE_V_BUDGET
    { 
        /// <summary>
        ///  项目id
        /// </summary>
        public string PROJECT { get; set; }
        

        /// <summary>
        /// 饲料厂-饲料价格
        /// </summary>
        public decimal Feedfac { get; set; }

        /// <summary>
        /// 养殖公司-饲料价格 
        /// </summary>
        public decimal Feedmanagement { get; set; }

        /// <summary>
        /// 代养户-饲料价格  
        /// </summary>
        public decimal Feedfarm { get; set; }


        /// <summary>
        ///  养殖公司-药品价格 
        /// </summary>
        public decimal Medmanagement { get; set; }


        /// <summary>
        /// 代养户-药品价格   
        /// </summary>
        public decimal Medfarm { get; set; }


        /// <summary>
        /// 养殖公司-疫苗价格 
        /// </summary>
        public decimal VacMANAGEMENT { get; set; }

        /// <summary>
        ///  代养户-疫苗价格
        /// </summary>
        public decimal VacFARM { get; set; }

        /// <summary>
        /// 养殖公司-小猪价格 
        /// </summary>
        public decimal PigletMANA { get; set; }

        /// <summary>
        ///  代养户-小猪价格 
        /// </summary>
        public decimal PigletFARM { get; set; }

        /// <summary>
        /// 合同总价   
        /// </summary>
        public decimal Contract { get; set; }

        /// <summary>
        /// 市场总价  
        /// </summary>
        public decimal Market { get; set; }


        /// <summary>
        /// 代养费  
        /// </summary>
        public decimal ADJUSTFEE { get; set; }
        
    } 
}
