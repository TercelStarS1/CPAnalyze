using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 项目-药品表
    /// </summary> 
    public class PBE_MEDICINECOST
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string PROJECT { get; set; }

        /// <summary>
        ///  养殖公司
        /// </summary>
        public decimal MANAGEMENT { get; set; }

        /// <summary>
        ///  代养户
        /// </summary>
        public decimal FARM { get; set; }
    } 
}
