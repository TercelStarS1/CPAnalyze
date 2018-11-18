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
        public int PageNum { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public int ShowNum { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        [SZColumn(MaxLength = 255)]
        public string Sort
        {
            get { return sort; }
            set { sort = value; }
        }

        private static string sort = "asc";
    }
    public class BasicSearch
    {
        //public 
    }
}
