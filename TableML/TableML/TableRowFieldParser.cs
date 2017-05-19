using System;
using System.Collections.Generic;

namespace TableML
{
    /// <summary>
    /// Field parser, string to something, user can custom it with extensions
    /// </summary>
    public partial class TableRowFieldParser
    {
        public byte Get_byte(string value, string defaultValue)
        {
            var str = Get_string(value, defaultValue);
            return string.IsNullOrEmpty(str) ? default(byte) : str.ToByte_();
        }

        public byte Get_Byte(string value, string defaultValue)
        {
            return Get_byte(value, defaultValue);
        }

        public sbyte Get_sbyte(string value, string defaultValue)
        {
            var str = Get_string(value, defaultValue);
            return string.IsNullOrEmpty(str) ? default(sbyte) : str.ToSByte_();
        }

        public sbyte Get_SByte(string value, string defaultValue)
        {
            return Get_sbyte(value, defaultValue);
        }

        public char Get_char(string value, string defaultValue)
        {
            var str = Get_string(value, defaultValue);
            return string.IsNullOrEmpty(str) ? default(char) : str.ToChar_();
        }

        public char Get_Char(string value, string defaultValue)
        {
            return Get_char(value, defaultValue);
        }

        public string Get_String(string value, string defaultValue)
        {
            return Get_string(value, defaultValue);
        }

        public string Get_string(string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return value;
        }

        /// <summary>
        /// 字符串中包含一些要去掉的分隔符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splits">要去掉的符号</param>
        /// <returns></returns>
        public string Get_String(string value,string defaultValue, params string[] splits)
        {
            var str = Get_string(value, defaultValue);
            if (splits != null && splits.Length > 0)
            {
                foreach (var split in splits)
                {
                    if (str.Contains(split))
                    {
                        str = str.Replace(split, "");
                    }
                }
            }
            return str;
        }

        public string Get_StringByArray(string value, string defaultValue)
        {
            return Get_String(value, defaultValue, new string[] { "{", "}" ,"|"});
        }

        public double Get_number(string value, string defaultValue)
        {
            return Get_double(value, defaultValue);
        }

        public double Get_Number(string value, string defaultValue)
        {
            return Get_double(value, defaultValue);
        }

        public int Get_Int32(string value, string defaultValue)
        {
            return Get_int(value, defaultValue);
        }

        public bool Get_bool(string value, string defaultValue)
        {
            return Get_Boolean(value, defaultValue);
        }

        public bool Get_Boolean(string value, string defaultValue)
        {
            var str = Get_string(value, defaultValue);
            if (string.IsNullOrEmpty(str)) return false;
            return str.ToBool_();
        }

        public long Get_long(string value, string defaultValue)
        {
            var str = Get_string(value, defaultValue);
            return string.IsNullOrEmpty(str) ? default(long) : str.ToInt64_();
        }
        public int Get_int(string value, string defaultValue)
        {
            var str = Get_string(value, defaultValue);
            return string.IsNullOrEmpty(str) ? default(int) : str.ToInt32_();
        }

        public double Get_double(string value, string defaultValue)
        {
            var str = Get_string(value, defaultValue);
            return string.IsNullOrEmpty(str) ? default(double) : str.ToDouble_();
        }

        public float Get_float(string value, string defaultValue)
        {
            var str = Get_string(value, defaultValue);
            return string.IsNullOrEmpty(str) ? default(float) : str.ToFloat_();
        }
        public uint Get_uint(string value, string defaultValue)
        {
            var str = Get_string(value, defaultValue);
            return string.IsNullOrEmpty(str) ? default(int) : str.ToUInt_();
        }

        public string[] Get_string_array(string value, string defaultValue)
        {
            var str = Get_StringByArray(value, defaultValue);
            if (string.IsNullOrEmpty(str)) return null;
            if (str.Contains(",") == false) return new string[] {str};
            return str.Split(',');
        }

        public short[] Get_short_array(string value, string defaultValue)
        {
            var str = Get_StringByArray(value, defaultValue);
            if (string.IsNullOrEmpty(str)) return null;
            if (str.Contains(",") == false) return new short[] {str.ToShort_()};
            var strs = str.Split(',');
            if (strs != null && strs.Length > 0)
            {
                List<short> array = new List<short>();
                var max = strs.Length;
                for (int idx = 0; idx < max; idx++)
                {
                    array.Add(strs[idx].ToShort_());
                }
                return array.ToArray();
            }
            return null;
        }

        public int[] Get_int_array(string value, string defaultValue)
        {
            var str = Get_StringByArray(value, defaultValue);
            if (string.IsNullOrEmpty(str)) return null;
            if (str.Contains(",") == false) return new int[] {str.ToInt32_()};
            var strs= str.Split(',');
            if (strs != null && strs.Length > 0)
            {
                List<int> array = new List<int>();
                var max = strs.Length;
                for (int idx = 0; idx < max; idx++)
                {
                    array.Add(strs[idx].ToInt32_());
                }
                return array.ToArray();
            }
            return null;
        }

        public float[] Get_float_array(string value, string defaultValue)
        {
            var str = Get_StringByArray(value, defaultValue);
            if (string.IsNullOrEmpty(str)) return null;
            if (str.Contains(",") == false) return new float[] {str.ToFloat_()};
            var strs = str.Split(',');
            if (strs != null && strs.Length > 0)
            {
                List<float> array = new List<float>();
                var max = strs.Length;
                for (int idx = 0; idx < max; idx++)
                {
                    array.Add(strs[idx].ToFloat_());
                }
                return array.ToArray();
            }
            return null;
        }

        public Dictionary<string, int> Get_Dictionary_string_int(string value, string defaultValue)
        {
            return GetDictionary<string, int>(value, defaultValue);
        }

        public Dictionary<TKey, TValue> GetDictionary<TKey, TValue>(string value, string defaultValue)
        {
            var dict = new Dictionary<TKey, TValue>();
            var str = Get_String(value, defaultValue);
            if (str.Contains(",") == false) return null;
            var arr = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in arr)
            {
                var kv = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                var itemKey = ConvertString<TKey>(kv[0]);
                var itemValue = ConvertString<TValue>(kv[1]);
                dict[itemKey] = itemValue;
            }
            return dict;
        }

        protected T ConvertString<T>(string value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

    }

}
