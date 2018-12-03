using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 项目--疫苗价格表
    /// </summary> 
    [SZTable(IsView = true)]
    public class PBE_VACCINECOST
    {
        /// <summary>
        /// 编码
        /// </summary>
        public decimal ID { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string PROJECT { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 编码
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
