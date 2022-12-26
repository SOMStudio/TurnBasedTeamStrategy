using System;

namespace Help
{
    public static class EnumExtension
    {
        public static String ConvertToString(this Enum enumValue)
        {
            return Enum.GetName(enumValue.GetType(), enumValue);
        }

        public static TEnumType ConvertToEnum<TEnumType>(this String stringValue)  
        {
            return (TEnumType)Enum.Parse(typeof(TEnumType), stringValue);
        }
        
        public static int ConvertToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }
        
        public static TEnumType ConvertToEnum<TEnumType>(this int intValue)  
        {
            return (TEnumType)Enum.Parse(typeof(TEnumType), intValue.ToString());
        }
    }
}
