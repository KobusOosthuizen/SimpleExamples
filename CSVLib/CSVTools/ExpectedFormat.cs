using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace CSVTools
{
    [Serializable]
    public class ExpectedFormat
    {
        public int ColumNo;
        public int MapToColumnNo;
        public String MapToColumnName;
        public TypeCode MapToType;
        public int? MinTextLength;
        public int? MaxTextLength;
        public String MinValue;
        public String MaxValue;
        public String Format;
        public String OnlyAllowCharacters;
        public List<String> OneOf;

        [XmlIgnore]
        private List<char> AllowCharacresList=null;
        [XmlIgnore]
        private StringBuilder AllowCharactersText=new StringBuilder();

        public ExpectedFormat()
        {
            
        }

        public Object IsOk(String Text, ref bool Result, ref string ErrorReason)
        {
            
            if (OnlyAllowCharacters != null)
            {
                if (AllowCharacresList == null)
                {
                    AllowCharacresList = new List<char>();
                    AllowCharacresList.AddRange(OnlyAllowCharacters);
                    AllowCharactersText.Append(OnlyAllowCharacters);
                }
                foreach(char c in Text)
                {
                    if (!AllowCharacresList.Contains(c))
                    {
                        Result = false;
                        ErrorReason = String.Format("{0}:Field may ony contain digits or characters [{1}].", MapToColumnName, AllowCharactersText);
                        return (null);
                    }
                }
            }

            if (OneOf != null)
            {
                if (!OneOf.Contains(Text))
                {
                    Result = false;
                    ErrorReason = String.Format("{0}:Field may only be one of the specific values.[{0}]", MapToColumnName, Text);
                    return (null);
                }
            }


            if (MinTextLength.HasValue)
            {
                if (Text.Length < MinTextLength.Value)
                {
                    Result = false;
                    ErrorReason = String.Format("{0}:Field requires at least {1} digit(s) or characters.", MapToColumnName, MinTextLength.Value);
                    return (null);
                }
            }
            if (MaxTextLength.HasValue)
            {
                if (Text.Length > MaxTextLength.Value)
                {
                    Result = false;
                    ErrorReason = String.Format("{0}:Field requires not more than {1} digit(s) or characters.", MapToColumnName, MaxTextLength.Value);
                    return (null);
                }
            }
            
            if (MinValue!=null)
            {
                if (TypeCodeParser.IsLess(Text, MapToType, Format, MinValue))
                {
                    Result = false;
                    ErrorReason = String.Format("{0}:Field must exceed {1}", MapToColumnName,MinValue);
                    return (null);
                }
            }

            if (MaxValue!=null)
            {
                if (TypeCodeParser.IsMore(Text, MapToType, Format, MaxValue))
                {
                    Result = false;
                    ErrorReason = String.Format("{0}:Field must be less than {1}", MapToColumnName,MaxValue);
                    return (null);
                }
            }

            Object o = TypeCodeParser.Parse(Text, MapToType, Format);
            Result = true;
            return (o);
        }
    }

    public static class TypeCodeParser
    {
        public static String Serialize(object theObj, Type theType)
        {
            String AsText = "";
            XmlSerializer serializer = new XmlSerializer(theType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            using (MemoryStream mStream = new MemoryStream())
            {
                XmlWriter writer = new XmlTextWriter(mStream, new UnicodeEncoding());
                serializer.Serialize(writer, theObj, ns);

                long count = mStream.Length;
                byte[] data = new byte[count];
                mStream.Position = 0;
                mStream.Read(data, 0, Convert.ToInt32(count));
                UnicodeEncoding ascii = new UnicodeEncoding();
                AsText = ascii.GetString(data, 0, Convert.ToInt32(count));
            }
            return (AsText);
        }

        public static object Deserialize(String from, Type theType)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(theType);
                byte[] data = Encoding.Unicode.GetBytes(from);
                mStream.Write(data, 0, data.Length);
                mStream.Position = 0;
                XmlTextReader reader = new XmlTextReader(mStream);
                object Load = (object)serializer.Deserialize(reader);
                return (Load);
            }
        }

        public static Object Parse(String Text, TypeCode ToTypeCode, String Format)
        {
            switch (ToTypeCode)
            {
                case TypeCode.Empty   : return(null);
                case TypeCode.Object  : return(Deserialize(Text,Type.GetType(Format)));
                case TypeCode.DBNull  : return(System.DBNull.Value);
                case TypeCode.Boolean : return (Boolean.Parse(Text));
                case TypeCode.Char    : return(Char.Parse(Text));
                case TypeCode.SByte   :

                                        SByte sub = 0;
                                        if (Text.Length == 0) return (sub);
                                        return (SByte.Parse(Text));
                case TypeCode.Byte    :
                                        Byte ub = 0;
                                        if (Text.Length == 0) return (ub);
                                        return (Byte.Parse(Text));
                case TypeCode.Int16   :
                                        Int16 uis0 = 0;
                                        if (Text.Length == 0) return (uis0);
                                        return(Int16.Parse(Text));
                case TypeCode.UInt16  :
                                        UInt16 si0 = 0;
                                        if (Text.Length == 0) return (si0);
                                        return(UInt16.Parse(Text));
                case TypeCode.Int32   :
                                        Int32 i0 = 0;
                                        if (Text.Length == 0) return (i0);
                                        return(Int32.Parse(Text));
                case TypeCode.UInt32  :
                                        UInt32 ui0 = 0;
                                        if (Text.Length == 0) return (ui0);
                                        return(UInt32.Parse(Text));
                case TypeCode.Int64   :
                                        Int64 l0 = 0;
                                        if (Text.Length == 0) return (l0);
                                        return(Int64.Parse(Text));
                case TypeCode.UInt64  :
                                        UInt64 ul0 = 0;
                                        if (Text.Length == 0) return (ul0);
                                        return(UInt64.Parse(Text));

                case TypeCode.Single  : Single s = 0;
                                        if (Text.Length == 0) return (s);
                                        return(Single.Parse(Text));

                case TypeCode.Double  : Double d = 0;
                                        if (Text.Length == 0) return (d);
                                        return(Double.Parse(Text));

                case TypeCode.Decimal :
                                        Decimal de = 0;
                                        if (Text.Length == 0) return (de);
                                        return(Decimal.Parse(Text));

                case TypeCode.DateTime: if (Text.Trim().Length == 0)
                                        {
                                            return (System.DBNull.Value);
                                        }
                                        else
                                        {
                                            return (DateTime.ParseExact(Text, Format, null));
                                        }
                case TypeCode.String  : return (Text);
             }
            return (null);
        }

        public static bool IsLess(String Text, TypeCode ToTypeCode, String Format,String Than)
        {
            switch (ToTypeCode)
            {
                case TypeCode.Empty: return (false);
                case TypeCode.Object: return (false);
                case TypeCode.DBNull: return (false);
                case TypeCode.Boolean: return (false);
                case TypeCode.Char: return(Char.Parse(Text) < Char.Parse(Than));
                case TypeCode.SByte: return (SByte.Parse(Text) < SByte.Parse(Than));
                case TypeCode.Byte: return (Byte.Parse(Text) < Byte.Parse(Than));
                case TypeCode.Int16: return (Int16.Parse(Text) < Int16.Parse(Than));
                case TypeCode.UInt16: return (UInt16.Parse(Text) < UInt16.Parse(Than));
                case TypeCode.Int32: return (Int32.Parse(Text) < Int32.Parse(Than));
                case TypeCode.UInt32: return (UInt32.Parse(Text) < UInt32.Parse(Than));
                case TypeCode.Int64: return (Int64.Parse(Text) < Int64.Parse(Than));
                case TypeCode.UInt64: return (UInt64.Parse(Text) < UInt64.Parse(Than));
                case TypeCode.Single: return (Single.Parse(Text) < Single.Parse(Than));
                case TypeCode.Double: return (Double.Parse(Text) < Double.Parse(Than));
                case TypeCode.Decimal: return (Decimal.Parse(Text) < Decimal.Parse(Than));
                case TypeCode.DateTime: return (DateTime.ParseExact(Text, Format, null) < DateTime.ParseExact(Than, Format, null));
                case TypeCode.String: return (false);
            }
            return (false);
        }

        public static bool IsMore(String Text, TypeCode ToTypeCode, String Format, String Than)
        {
            switch (ToTypeCode)
            {
                case TypeCode.Empty: return (false);
                case TypeCode.Object: return (false);
                case TypeCode.DBNull: return (false);
                case TypeCode.Boolean: return (false);
                case TypeCode.Char: return (Char.Parse(Text) > Char.Parse(Than));
                case TypeCode.SByte: return (SByte.Parse(Text) > SByte.Parse(Than));
                case TypeCode.Byte: return (Byte.Parse(Text) > Byte.Parse(Than));
                case TypeCode.Int16: return (Int16.Parse(Text) > Int16.Parse(Than));
                case TypeCode.UInt16: return (UInt16.Parse(Text) > UInt16.Parse(Than));
                case TypeCode.Int32: return (Int32.Parse(Text) > Int32.Parse(Than));
                case TypeCode.UInt32: return (UInt32.Parse(Text) > UInt32.Parse(Than));
                case TypeCode.Int64: return (Int64.Parse(Text) > Int64.Parse(Than));
                case TypeCode.UInt64: return (UInt64.Parse(Text) > UInt64.Parse(Than));
                case TypeCode.Single: return (Single.Parse(Text) > Single.Parse(Than));
                case TypeCode.Double: return (Double.Parse(Text) > Double.Parse(Than));
                case TypeCode.Decimal: return (Decimal.Parse(Text) > Decimal.Parse(Than));
                case TypeCode.DateTime: return (DateTime.ParseExact(Text, Format, null) > DateTime.ParseExact(Than, Format, null));
                case TypeCode.String: return (false);
            }
            return (false);
        }

        public static Type GetTypeOf(TypeCode ToTypeCode, String Format)
        {
            switch (ToTypeCode)
            {
                case TypeCode.Empty: return (typeof(String));
                case TypeCode.Object: return (Type.GetType(Format));
                case TypeCode.DBNull: return (typeof(DBNull));
                case TypeCode.Boolean: return (typeof(Boolean));
                case TypeCode.Char: return (typeof(Char));
                case TypeCode.SByte: return (typeof(SByte));
                case TypeCode.Byte: return (typeof(Byte));
                case TypeCode.Int16: return (typeof(Int16));
                case TypeCode.UInt16: return (typeof(UInt16));
                case TypeCode.Int32: return (typeof(Int32));
                case TypeCode.UInt32: return (typeof(UInt32));
                case TypeCode.Int64: return (typeof(Int64));
                case TypeCode.UInt64: return (typeof(UInt64));
                case TypeCode.Single: return (typeof(Single));
                case TypeCode.Double: return (typeof(Double));
                case TypeCode.Decimal: return (typeof(Decimal));
                case TypeCode.DateTime: return (typeof(DateTime));
                case TypeCode.String: return (typeof(String));
            }
            return (typeof(String));
        }

    }
}
