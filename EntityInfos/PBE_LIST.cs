using System;
using System.Collections.Generic;
using SZORM;

namespace EntityInfos
{
    [SZTable(IsView = true)]
    public class PBE_LIST
    {
        //项目参数保存
        public PBE_PROJECT PBE_PROJECTINFO { get; set; }
         

        public List<PBE_FEEDCOST> PBE_FEEDCOSTList { get; set; }

        /// <summary>
        /// 项目表--疫苗价格表
        /// </summary>
        public List<PBE_VACCINECOST> PBE_VACCINECOSTList { get; set; }

        /// <summary>
        /// 项目表--小猪表
        /// </summary>
        public PBE_PIGLETCOST PBE_PIGLETCOSTINFO { get; set; }

        /// <summary>
        /// 项目表--成品猪
        /// </summary>
        public PBE_PIGMARKETPRICE PBE_PIGMARKETPRICEINFO { get; set; }

        /// <summary>
        /// 项目表--药品表
        /// </summary>
        public PBE_MEDICINECOST PBE_MEDICINECOSTINFO { get; set; }

        /// <summary>
        /// 编辑用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 标识  0新建  项目ID
        /// </summary>
        public string Skin { get; set; }
    } 
}
