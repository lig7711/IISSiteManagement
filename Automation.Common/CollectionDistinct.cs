using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Common
{
    public delegate bool EqualsComparer<T>(T x, T y);

    /// <summary>
    /// 集合<T>的Distinct
    /// </summary>
    public class CollectionDistinct<T> : IEqualityComparer<T>
    {

        private EqualsComparer<T> _equalsComparer;

        public CollectionDistinct(EqualsComparer<T> equalsComparer)
        {
            this._equalsComparer = equalsComparer;
        }

        public bool Equals(T x, T y)
        {
            if (null != this._equalsComparer)
                return this._equalsComparer(x, y);
            else
                return false;
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }


    //去重复 实例

    //使用匿名委托方法
    //data.DataDefineColumns = data.DataDefineColumns.Distinct(new CollectionDistinct<DataDefineColumn>(
    //    delegate(DataDefineColumn x, DataDefineColumn y)
    //    {
    //        if (null == x || null == y) return false;
    //        return x.ColumnCode == y.ColumnCode;
    //    }
    //    )).ToList();


    ////使用 Lambda 表达式
    //data.DataDefineColumns = data.DataDefineColumns.Distinct(new CollectionDistinct<DataDefineColumn>(
    //    (x, y) => (null != x && null != y) && (x.ColumnCode == y.ColumnCode))).ToList();
}
