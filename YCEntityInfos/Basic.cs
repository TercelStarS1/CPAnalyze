using System;
using SZORM;

namespace EntityInfos
{
    public class Basic
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public int? PageNum { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public int? ShowNum { get; set; } 

    }
    public class BasicSearch
    {
         
    }
}
