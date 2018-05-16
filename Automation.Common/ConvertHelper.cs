using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Common
{
    public static class ConvertHelper
    {
        /// <summary>
        /// Convert IList to List &lt T &gt
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<T> ConvertToList<T>(this IList list)
        {
            if (list == null)
                return null;

            var result = new List<T>();
            foreach (var source in list)
            {
                result.Add((T)source);
            }

            return result;
        }
    }
}
