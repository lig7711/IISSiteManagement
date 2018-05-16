using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Common
{
    /// <summary>
    /// 分页查询结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedListResult<T>
    {
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 总条数
        /// </summary>
        public int Total
        {
            get;
            set;
        }
        /// <summary>
        /// 排序
        /// </summary>
        private string _orderBy;

        public string OrderBy
        {
            get
            {
                return string.IsNullOrEmpty(_orderBy) ? "Id" : _orderBy;
            }
            set
            {
                _orderBy = value;
            }
        }

        /// <summary>
        /// true代表asc，false代表desc
        /// </summary>
        public bool Order
        {
            get;
            set;
        }
        public IList<T> Data
        {
            get;
            set;
        }
    }
}
