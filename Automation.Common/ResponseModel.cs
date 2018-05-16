using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Common
{
    public class ResponseModel
    {
        /// <summary>
        /// false 失败 true 成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// data
        /// </summary>
        public Object Result { get; set; }

        /// <summary>
        /// 扩展 data
        /// </summary>
        public Object ResultExt { get; set; }

        /// <summary>
        /// 扩展(用于同为成功或失败的不处理)
        /// </summary>
        public int ResultType
        {
            get;
            set;
        }
    }
}
