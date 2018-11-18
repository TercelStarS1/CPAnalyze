using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    ///  用户表
    /// </summary>
    [SZTable(IsView = true)]  ///   [SZTable(IsView =true)]这个特性的意思是如果你这个表已经存在了。就不创建了，如果需要我给你自动创建表。就不带这个
    public class AccountInfo
    {
        //主键id  
        public int USERId { get; set; }

        //用户编号
        [SZColumn(MaxLength = 255)]
        public string USERCODE { get; set; }

        //用户姓名
        [SZColumn(MaxLength = 255)]
        public string USERNAME { get; set; }
        //用户公司 
        [SZColumn(MaxLength = 255)]
        public string company { get; set; } 
        //用户角色
        public int RoleId { get; set; }
        //用户父id
        public int Parentid { get; set; }
        //用户密码
        [SZColumn(MaxLength = 255)]
        public string Psw { get; set; }

        //用户创建时间
        public DateTime Createtime { get; set; }

    }
    /// <summary>
    /// 用户搜索模型
    /// </summary>
    public class AccountInfoModel
    {
        /// <summary>
        ///  
        /// </summary>
        public string NAME { get; set; } 
    }
}
