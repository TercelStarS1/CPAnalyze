using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 用户表
    /// </summary> 
    [SZTable(IsView = true)]
    public class PBE_USER
    {  
        /// <summary>
        ///  
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string NAME { get; set; }  
        /// <summary>
        ///  
        /// </summary>
        public string PASSWORD { get; set; } 
    } 
}
