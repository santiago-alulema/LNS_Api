using System.ComponentModel;
using System.Reflection;

namespace LNS_API.Clases
{
    public class EndPointLNS
    {
        public string LOGIN = "databases/[DATABASE]/sessions";
        public string SEARCH_PRODUCT = "databases/[DATABASE]/layouts/[LAYOUT]/_find";
    }

    //public static string GetEnumDescription(Enum value)
    //{
    //    FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

    //    DescriptionAttribute[] attributes =
    //        (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
    //            typeof(DescriptionAttribute), false);

    //    return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    //}
}
