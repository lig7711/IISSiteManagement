using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Automation.Common
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumerationExtension
    {
        /// <summary>
        /// 获取枚举值的描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        /// <summary>
        /// 获取枚举值和描述
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumValueAndDesc(this Type enumType)
        {
            if (!enumType.IsEnum) throw new ArgumentException("enumType");
            var dic = new Dictionary<int, string>();
            foreach (var value in Enum.GetValues(enumType))
            {
                object[] objAttrs = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objAttrs != null && objAttrs.Length > 0)
                {
                    DescriptionAttribute descAttr = objAttrs[0] as DescriptionAttribute;
                    dic.Add(Convert.ToInt32(value), descAttr.Description);
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取枚举和描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetEnumTextAndDesc(this Type enumType)
        {
            if (!enumType.IsEnum) throw new ArgumentException("enumType");
            var dic = new Dictionary<string, string>();
            foreach (var value in Enum.GetValues(enumType))
            {
                object[] objAttrs = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objAttrs != null && objAttrs.Length > 0)
                {
                    DescriptionAttribute descAttr = objAttrs[0] as DescriptionAttribute;
                    dic.Add(value.ToString(), descAttr.Description);
                }
            }
            return dic;
        }

        //foreach (var value in Enum.GetValues(typeof(EMyType)))  
        //{  
        //    Console.WriteLine(string.Format("{0}={1}", value.ToString(), Convert.ToInt32(value)));  
        //}  

        // foreach (var name in Enum.GetNames(typeof(EMyType)))  
        //{  
        //    Console.WriteLine(name);  
        //} 

    }
}
