using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Grabzujuan.Common
{
    public static class EnumExtensions
    {
        public static string GetDisplay(this Enum value)
        {
            var attr = value.GetAttribute<DisplayAttribute>();
            return attr == null ? "" : attr.Name;
        }

        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            return Attribute.GetCustomAttribute(field, typeof(T)) as T;
        }

        public static int GetValue(this Enum value)
        {
            return Convert.ToInt32(value);
        }


        public static List<Tuple<string, string>> GetEnumSource(this Type type)
        {
            if (!type.IsEnum)
                throw new Exception("type 类型必须为枚举类型!");

            var list = new List<Tuple<string, string>>();

            foreach (var value in Enum.GetValues(type))
            {
                var fieldName = Enum.GetName(type, value);
                var field = type.GetField(fieldName);
                var display = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
                if (display != null)
                    list.Add(new Tuple<string, string>(Convert.ToInt32(value) + "", display.Name));
                else
                    list.Add(new Tuple<string, string>(Convert.ToInt32(value) + "", fieldName));
            }
            return list;
        }

        public static List<Tuple<string, string>> GetEnumSource2(Type type)
        {
            if (!type.IsEnum)
                throw new Exception("type 类型必须为枚举类型!");

            var list = new List<Tuple<string, string>>();

            foreach (var value in Enum.GetValues(type))
            {
                var fieldName = Enum.GetName(type, value);
                var field = type.GetField(fieldName);
                var display = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
                if (display != null)
                    list.Add(new Tuple<string, string>(Convert.ToInt32(value) + "", display.Name));
                else
                    list.Add(new Tuple<string, string>(Convert.ToInt32(value) + "", fieldName));
            }
            return list;
        }
    }
}
