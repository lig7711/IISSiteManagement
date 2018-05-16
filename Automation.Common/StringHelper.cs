using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Automation.Common
{
    /// <summary>
    /// 字符串扩展处理
    /// Add Magic
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 锁
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// 替换内容中的换行符
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string StrReplace(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;

            return inputString.Replace("'", "\'").Replace("\r", "").Replace("\n", "");
        }
        
        /// <summary>
        /// 非法字符转换
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string ReplaceStr(string inputString)
        {
            inputString = inputString.Replace(" ", "");
            inputString = inputString.Replace("'", "");
            inputString = inputString.Replace("&", "");
            inputString = inputString.Replace("#", "");
            inputString = inputString.Replace("=", "");
            inputString = inputString.Replace(";", "");
            inputString = inputString.Replace(":", "");
            inputString = inputString.Replace("/", "");
            inputString = inputString.Replace("?", "");
            inputString = inputString.Replace("<", "");
            inputString = inputString.Replace(">", "");
            inputString = inputString.Replace(".", "");
            inputString = inputString.Replace("#", "");
            inputString = inputString.Replace("%", "");
            inputString = inputString.Replace("%", "");
            inputString = inputString.Replace("^", "");
            inputString = inputString.Replace("//", "");
            inputString = inputString.Replace("@", "");
            inputString = inputString.Replace("(", "");
            inputString = inputString.Replace(")", "");
            inputString = inputString.Replace("*", "");
            inputString = inputString.Replace("~", "");
            inputString = inputString.Replace("`", "");
            inputString = inputString.Replace("$", "");
            inputString = inputString.Replace("!", "");
            inputString = inputString.Replace("+", "");
            inputString = inputString.Replace("！", "");
            inputString = inputString.Replace("？", "");
            inputString = inputString.Replace("~", "");
            inputString = inputString.Replace("（", "");
            inputString = inputString.Replace("）", "");
            inputString = inputString.Replace("；", "");
            inputString = inputString.Replace("：", "");
            inputString = inputString.Replace("，", "");
            inputString = inputString.Replace("。", "");
            inputString = inputString.Replace("【", "");
            inputString = inputString.Replace("】", "");
            inputString = inputString.Replace("[", "");
            inputString = inputString.Replace("]", "");
            inputString = inputString.Replace("{", "");
            inputString = inputString.Replace("}", "");
            inputString = inputString.Replace("{", "");
            inputString = inputString.Replace("}", "");
            inputString = inputString.Replace("“", "");
            inputString = inputString.Replace("”", "");
            inputString = inputString.Replace("\"", "");
            inputString = inputString.Replace("……", "");
            inputString = inputString.Replace("^", "");
            inputString = inputString.Replace("|", "");
            inputString = inputString.Replace("\\", "");
            inputString = inputString.Replace("、", "");
            inputString = inputString.Replace("`", "");
            return inputString;
        }
                
        /// <summary>
        /// 生成指定长度的随机码（大小字母、数字)
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetRandomCode(int len)
        {
            string codes = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789";

            return GetRandomCode(codes, len);
        }

        /// <summary>
        /// 根据指定字符串，生成指定长度的随机码
        /// </summary>
        /// <param name="templateString">字符串模板</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetRandomCode(string templateString, int len)
        {
            if (string.IsNullOrEmpty(templateString) || len < 1) return string.Empty;

            StringBuilder tempSB = new StringBuilder(len);
            Random random = new Random();

            int i = 0;

            while (i < len)
            {
                tempSB.Append(templateString[random.Next(templateString.Length)]);
                i++;
            }

            return (tempSB.ToString());
        }

        /// <summary>
        /// 生成唯一数
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueCode()
        {
            lock (_lock) return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }

        /// <summary>
        /// 生成SubTable唯一数
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueSubCode()
        {
            lock (_lock) return DateTime.Now.Ticks.ToString();
        }

        /// <summary>
        /// 获取不重复数字
        /// </summary>
        /// <param name="total"></param>
        /// <returns></returns>
        public static int[] GetRandomSequence(int total)
        {
            int[] sequence = new int[total];
            int[] output = new int[total];

            for (int i = 0; i < total; i++)
            {
                sequence[i] = i;
            }

            Random random = new Random();

            int end = total - 1;

            for (int i = 0; i < total; i++)
            {
                int num = random.Next(0, end + 1);
                output[i] = sequence[num];
                sequence[num] = sequence[end];
                end--;
            }

            return output;
        }

        /// <summary>
        /// 根据中英文自动截取指定长度字符串，中文算两个字符
        /// </summary>
        public static string Cut(this string str, int len)
        {
            return Cut(str, len, string.Empty);
        }

        /// <summary>
        /// 根据中英文自动截取指定长度字符串，中文算两个字符
        /// </summary>
        public static string Cut(this string str, int len, string endStr)
        {
            // 待截取的字符为空，或待截取长度小于0，返回空字符串。
            if (str == null || str.Length == 0 || len <= 0) return "";

            // 过滤所有Html代码
            str = Regex.Replace(str, "<.+?>", "", RegexOptions.Singleline);

            // 替换换行和回车
            //str = str.Replace("<br />", "").Replace("<p>", "").Replace("</p>", "");

            // 替换空格
            str = str.Replace("&nbsp;", "");

            // 假定待截取字符全部为汉字，仍不够截取，直接返回。
            if (str.Length * 2 <= len) return str;

            // 将汉字作为英文对待，截取多余部分，针对输入字符串太长时提高效率。
            if (str.Length > len) str = str.Substring(0, len);

            ASCIIEncoding ascii = new ASCIIEncoding();

            StringBuilder cutString = new StringBuilder(len);

            int cutLen = 0;

            byte[] bytes = ascii.GetBytes(str);

            for (int i = 0; i < bytes.Length; i++)
            {
                if ((int)bytes[i] == 63)
                {
                    cutLen += 2;    //汉字
                }
                else
                {
                    cutLen += 1;    //英文
                }

                cutString.Append(str.Substring(i, 1));

                if (cutLen >= len) break;
            }

            return cutString.Append(endStr).ToString();
        }

        /// <summary>
        /// 去除字符串数组中的重复项
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string[] RemoveRepeat(this string[] strs)
        {
            StringCollection sc = new StringCollection();

            for (int i = 0; i < strs.Length; i++)
            {
                if (!sc.Contains(strs[i]))
                {
                    sc.Add(strs[i]);
                }
            }

            string[] results = new string[sc.Count];

            sc.CopyTo(results, 0);

            return results;
        }

        /// <summary>
        /// 获取简体中文的拼音首字母
        /// </summary>
        public static string GetFirstSpell(this string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;

            int len = str.Length;

            StringBuilder spell = new StringBuilder(len);

            for (int i = 0; i < len; i++)
            {
                spell.Append(str.Substring(i, 1).GetFirstSpellForSingleChar());
            }

            return spell.ToString();
        }

        /// <summary>
        /// 获取单个简体中文的拼音首字母
        /// </summary>
        public static string GetFirstSpellForSingleChar(this string str)
        {
            byte[] sbytes = Encoding.Default.GetBytes(str);

            int code = (((short)sbytes[0]) << 8) + (short)sbytes[1];

            //特殊字处理
            string result = String.Empty;
            switch (code)
            {
                case 60595: result = "f"; break;
                case 58791: result = "p"; break;
                case 56054: result = "q"; break;
                default: break;
            }
            if (!String.IsNullOrEmpty(result)) return result;
            //处理普通字符
            int[] codeArray = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481, 55290 };

            for (int i = 0; i < 26; i++)
            {
                if (codeArray[i] <= code && code < codeArray[i + 1])
                {
                    return Encoding.Default.GetString(new byte[] { (byte)(97 + i) });
                }
            }
            return string.Empty;
        }
    }
}
