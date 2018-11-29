using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 疫苗使用标准
    /// </summary> 
    public class PBE_STDVACCINE  
    { 
        /// <summary>
        /// 项目名
        /// </summary>
        public string PROJECT { get; set; }
        /// <summary>
        /// 日龄
        /// </summary>
        public decimal DAYS { get; set; }
        /// <summary>
        /// 疫苗名
        /// </summary>
        public string VACCINE { get; set; }  
        /// <summary>
        /// 使用量
        /// </summary>
        public decimal QTY { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string VARIETY { get; set; } 
       

    } 
}
