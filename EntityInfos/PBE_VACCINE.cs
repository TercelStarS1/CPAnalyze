using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 疫苗表
    /// </summary> 
    public class PBE_VACCINE 
    { 
        /// <summary>
        /// 编号
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
        ///养殖公司
        /// </summary>
        public decimal MANAGEMENT { get; set; }
        /// <summary>
        ///  代养户
        /// </summary>
        public decimal FARM { get; set; }

    } 
}
