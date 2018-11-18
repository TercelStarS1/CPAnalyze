using Helpers;
using System;
using SZORM;

namespace EntityInfos
{
    /// <summary>
    /// 这个名字很重要，这个名字跟连接的名字是一致的
    /// </summary>
    public partial class DB : DbContext
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public DbSet<UserInfo> UserInfo { get; set; }
        

    }
    /// <summary>
    /// 这个就是使用的oracle连接
    /// </summary>
    public partial class StarOracle: DbContext
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public DbSet<AccountInfo> AccountInfo { get; set; }

        /// <summary>
        /// 公司信息
        /// </summary>
        public DbSet<ZB_FEED_COMPANY> ZB_FEED_COMPANY { get; set; }
        /// <summary>
        /// 销售客户编码
        /// </summary>
        public DbSet<ZB_FEED_CUSTOMER> ZB_FEED_CUSTOMER { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public DbSet<ZB_FEED_PRODUCT> ZB_FEED_PRODUCT { get; set; }
        /// <summary>
        /// 销售数据
        /// </summary>
        public DbSet<ZB_FEED_SALE> ZB_FEED_SALE { get; set; }
        /// <summary>
        /// 销售员编码数据
        /// </summary>
        public DbSet<ZB_FEED_SALESPERSON> ZB_FEED_SALESPERSON { get; set; }
        /// <summary>
        /// 客户与销售员关系
        /// </summary>
        public DbSet<ZB_FEED_SALESP_CUST> ZB_FEED_SALESP_CUST { get; set; }
         
    } 
}
