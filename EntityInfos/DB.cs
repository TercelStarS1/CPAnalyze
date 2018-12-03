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

    public partial class ZDSYYC : DbContext
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<PBE_USER> PBE_USER { get; set; }

        /// <summary>
        /// 生长情况
        /// </summary>
        public DbSet<PBE_STDWITHAGE> PBE_STDWITHAGE { get; set; }

        /// <summary>
        /// 疫苗使用标准
        /// </summary>
        public DbSet<PBE_STDVACCINE> PBE_STDVACCINE { get; set; }

        /// <summary>
        /// 饲料表
        /// </summary>
        public DbSet<PBE_FEED> PBE_FEED { get; set; }

        /// <summary>
        /// 疫苗表
        /// </summary>
        public DbSet<PBE_VACCINE> PBE_VACCINE { get; set; }

        /// <summary>
        /// 与日龄相关标准
        /// </summary>
        public DbSet<PBE_STD> PBE_STD { get; set; }


        /// <summary>
        /// 项目表
        /// </summary>
        public DbSet<PBE_PROJECT> PBE_PROJECT { get; set; }

        /// <summary>
        /// 项目--饲料价格表
        /// </summary>
        public DbSet<PBE_FEEDCOST> PBE_FEEDCOST { get; set; }

        /// <summary>
        /// 项目表--疫苗价格表
        /// </summary>
        public DbSet<PBE_VACCINECOST> PBE_VACCINECOST { get; set; }

        /// <summary>
        /// 项目表--小猪表
        /// </summary>
        public DbSet<PBE_PIGLETCOST> PBE_PIGLETCOST { get; set; }

        /// <summary>
        /// 项目表--成品猪
        /// </summary>
        public DbSet<PBE_PIGMARKETPRICE> PBE_PIGMARKETPRICE { get; set; }

        /// <summary>
        /// 项目表--药品表
        /// </summary>
        public DbSet<PBE_MEDICINECOST> PBE_MEDICINECOST { get; set; }


        public DbSet<PBE_LIST> PBE_LIST { get; set; }

        /// <summary>
        /// 费用总统计
        /// </summary>
        public DbSet<PBE_V_BUDGET> PBE_V_BUDGET { get; set; }

    }
}
