using System; 
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 用户  
    /// </summary>

    public class UserInfo:Basic
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [SZColumn(MaxLength = 50)]
        public string UserName { get; set; }
        /// <summary>
        /// 姓名拼音头
        /// </summary>
        [SZColumn(MaxLength = 2000)]
        public string UserNameSpell { get; set; }
        /// <summary>
        /// 用户电话
        /// </summary> 
        [SZColumn(MaxLength = 50)]
        public string UserPhone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [SZColumn(MaxLength = 50)]
        public string UserPassword { get; set; } 
        /// <summary>
        /// 是否允许登陆 BoolDict
        /// </summary>
        [SZColumn(MaxLength = 2)]
        public string IsLogin { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role_Key { get; set; }
        /// <summary>
        /// 登陆次数
        /// </summary>
        public int? LoginCount { get; set; }
        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime? LastLoginDay { get; set; }
        /// <summary>
        /// 最后登陆IP
        /// </summary>
        public string LastLoginIP { get; set; } 

    }
    /// <summary>
    /// 用户搜索模型
    /// </summary>
    public class UserSearchModel
    {
        /// <summary>
        /// 按照姓名查询
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 按照角色查询
        /// </summary>
        public string Role { get; set; }
    }
}
