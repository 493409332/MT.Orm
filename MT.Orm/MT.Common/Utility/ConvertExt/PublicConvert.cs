using System;

namespace MT.Common.Utility.ConvertExt
{
    public static class PublicConvert
    {
        public static T ReferenceFromType<TK, T>(this TK text) where TK : class   
        { 
            try
            {
                return (T) Convert.ChangeType(text, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(T);
            }
        }



        public static T ValueFromType<TK, T>(this TK text) where TK : struct
        {

            try
            {
                return (T) Convert.ChangeType(text, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
