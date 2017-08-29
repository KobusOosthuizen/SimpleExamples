using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;

namespace CSVTools
{
    [Serializable]
    public class ParseAdvice
    {
        public String SplitWith;
        public int SkipLines;
        public int SkipBottomLines;
        public int ExpectColums;
        public bool UseDoubleQuotesAsQuotes;
        public bool UseSingleQuotesAsQuotes;
		public bool SkipEmptyLines;
		public ExpectedFormat[] Mapping;
		

        [XmlIgnore]
        public Dictionary<int,ExpectedFormat> ColumMappings
        {
          get
          {
              if (m_MappingDcitionary==null)
              {
                  m_MappingDcitionary = new Dictionary<int, ExpectedFormat>();
                  foreach (ExpectedFormat f in Mapping)
                  {
                      if (!m_MappingDcitionary.ContainsKey(f.ColumNo))
                      {
                          m_MappingDcitionary.Add(f.MapToColumnNo, f);
                      }
                  }
              }
              return (m_MappingDcitionary);
          }
        }

        public DataTable GetTargetSchema()
        {
            int MaxC = 0;
            foreach (ExpectedFormat f in ColumMappings.Values)
            {
                if (f.MapToColumnNo > MaxC) MaxC = f.MapToColumnNo;
            }

            DataTable ParsedData = new DataTable();
            for (int n = 1; n <= MaxC; n++)
            {
                if (ColumMappings.ContainsKey(n))
                {
                    ExpectedFormat mapthis = ColumMappings[n];
                    DataColumn dc = new DataColumn(mapthis.MapToColumnName, TypeCodeParser.GetTypeOf(mapthis.MapToType, mapthis.Format));
                    ParsedData.Columns.Add(dc);
                }
                else
                {
                    DataColumn dc = new DataColumn();
                    ParsedData.Columns.Add(dc);
                }
            }


            DataColumn dcOlineNo = new DataColumn("Original_LineNo", typeof(Int32));
            ParsedData.Columns.Add(dcOlineNo);

            DataColumn dcOline = new DataColumn("Original_Data", typeof(String));
            ParsedData.Columns.Add(dcOline);
            return (ParsedData);
        }

        [XmlIgnore]
        private Dictionary<int, ExpectedFormat> m_MappingDcitionary = null;

        public ParseAdvice()
        {
        }

        public void HandleSpecialCharacters()
        {
            SplitWith = SplitWith.Replace("##TAB##", "\t").Replace("##tab##", "\t");
        }
    }
}
