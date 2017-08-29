using System;
using System.Collections.Generic;
using System.IO;
using System.Data;

namespace CSVTools
{
    public class Parser:IDisposable
    {
        public  Parser()
        {
        }

        
        private List<String> m_Lines = new List<string>();
        private List<int> m_ErrorLines = new List<int>();
        public List<KeyValuePair<int?, String>> Errors = new List<KeyValuePair<int?, String>>();

        public List<String>AllLines
        {
            get
            {
                return (m_Lines);
            }
        }
        public Dictionary<int,String>ErrorLines
        {
            get
            {
                Dictionary<int,String> Result = new Dictionary<int,string>();
                foreach (int l in m_ErrorLines)
                {
                    if (!Result.ContainsKey(l))
                    {
                        Result.Add(l, m_Lines[l]);
                    }
                }
                return (Result);
            }
        }
        public DataTable ParsedData;
        

        public string[] SplitLine(String Line, ParseAdvice Advice)
        {
            List<KeyValuePair<String,String>> Replacements=new List<KeyValuePair<String,String>>();
            List<KeyValuePair<int,char>> NonSeperators = new List<KeyValuePair<int,char>>();

            int OpenSingleQuotes = 0;
            int OpenDoubleQuotes = 0;

            for (int pos = 0; pos < Line.Length; pos++)
            {

                if (Line[pos] == '\"')
                {
                    if (Advice.UseDoubleQuotesAsQuotes)
                    {
                        if (OpenDoubleQuotes == 0)
                        {
                            OpenDoubleQuotes = 1;
                        }
                        else
                        {
                            OpenDoubleQuotes = 0;
                        }
                    }
                }
                else if (Line[pos] == '\'')
                {
                    if (Advice.UseSingleQuotesAsQuotes)
                    {
                        if (OpenSingleQuotes == 0)
                        {
                            OpenSingleQuotes = 1;
                        }
                        else
                        {
                            OpenSingleQuotes = 0;
                        }
                    }
                }
                  
                if ((OpenSingleQuotes+OpenDoubleQuotes)>0)
                {
                    //we are in a currentQuote;
                    if (Line[pos] ==  Advice.SplitWith[0])
                    {
                        NonSeperators.Add(new KeyValuePair<int,char>(pos, Line[pos]));
                    }
                }
            }
           
            for (int index = NonSeperators.Count; index > 0; index--)
            {
                KeyValuePair<int,char> pair=NonSeperators[index-1];
                char CharacterAtIndex=pair.Value;
                int PositionAtIndex=pair.Key;

                String rep=String.Format("<<#-{0}-#>>", Convert.ToInt16(CharacterAtIndex).ToString().PadLeft(5,'0'));
                
                String ForCharString=""+CharacterAtIndex;
                Replacements.Add(new KeyValuePair<string,string>(rep,ForCharString));
                Line = Line.Remove(PositionAtIndex,1).Insert(PositionAtIndex, rep);
            }

            String[] Parts= Line.Split(new char[] { Advice.SplitWith[0] });

            for (int index = 0; index < Parts.Length; index++)
            {
                foreach (KeyValuePair<string, string> pair in Replacements)
                {
                    Parts[index] = Parts[index].Replace(pair.Key, pair.Value);
                }

                String Temp = Parts[index].TrimStart().TrimEnd();
                if (Temp.Length > 1)
                {
                    if ((Temp[0] == Temp[Temp.Length - 1]) && ((Temp[0] == '\'') || (Temp[0] == '\"')))
                    {
                        char Seperator = Temp[0];
                        Temp = Temp.Remove(Temp.Length - 1);
                        if (Temp.Length>0)
                        {
                            Temp =  Temp.Remove(0, 1);
                        }
                    }
                    else
                    {
                        Temp = Parts[index];
                    }
                }
                Parts[index] = Temp;
            }

            return (Parts);
        }

        private void AddError(int? LineNo,String Description,String Text)
        {
            Errors.Add(new KeyValuePair<int?,String>(LineNo,String.Format("{0}->{1}",Description,Text)));
            if (LineNo.HasValue)
            {
                m_ErrorLines.Add(LineNo.Value);
            }
        }

        public bool Parse(List<String> Lines, ParseAdvice Advice)
        {
            try
            {
                m_Lines = Lines;
                if (Advice.SkipLines > 0)
                {
                    if (m_Lines.Count < 0)
                    {
                        AddError(null, String.Format("Could not skip the first {0} lines as the file only contain {0} lines.", Advice.SkipLines, m_Lines.Count),"");
                        return (false);
                    }
                }

                foreach (ExpectedFormat exf in Advice.ColumMappings.Values)
                {
                    if ((exf.ColumNo < 1))
                    {
                        AddError(null, String.Format("Problem in ColumMappings. Expected Colums can only be a value from 1 to {0}.", Advice.ExpectColums),"");
                    }

                    if ((exf.ColumNo > Advice.ExpectColums) || (exf.ColumNo < 1))
                    {
                        AddError(null, String.Format("Problem in ColumMappings. Expected Colums is {0} but a mapping is required for column {1}", Advice.ExpectColums, exf.ColumNo), "");
                    }
                }


                ParsedData = Advice.GetTargetSchema();
                for (int line = Advice.SkipLines; line < m_Lines.Count; line++)
                {
                    int lines_remaining = m_Lines.Count - line;
                    if  (    (!Advice.SkipEmptyLines)
                          || (Advice.SkipEmptyLines) && (m_Lines[line].Trim().Length > 0)
                        )

                    {
                        if (lines_remaining > Advice.SkipBottomLines)
                        {
                            String[] Parts = SplitLine(m_Lines[line], Advice);
                            if (Parts.Length != Advice.ExpectColums)
                            {
                                AddError(line, String.Format("Found {0} column(s), but expecting {1} column(s).", Parts.Length, Advice.ExpectColums), m_Lines[line]);
                            }
                            else
                            {

                                DataRow dr = ParsedData.NewRow();
                                dr["Original_LineNo"] = line;
                                dr["Original_Data"] = m_Lines[line];
                                ParsedData.Rows.Add(dr);
                                foreach (ExpectedFormat exf in Advice.ColumMappings.Values)
                                {
                                    try
                                    {
                                        bool Result = false;
                                        String ErrorReason = "";
                                        Object Value = exf.IsOk(Parts[exf.ColumNo - 1], ref Result, ref ErrorReason);
                                        if (Result)
                                        {
                                            dr[exf.MapToColumnNo - 1] = Value;
                                        }
                                        else
                                        {
                                            AddError(line, String.Format("Error parsing field {0}.", ErrorReason), m_Lines[line]);
                                        }
                                    }
                                    catch (Exception exinner)
                                    {
                                        AddError(line, String.Format("UnExpected Error: {0}", exinner.ToString()), m_Lines[line]);
                                    }
                                }
                            }
                        }
                    }
                }
                if (Errors.Count > 0) return (false);
                return (true);
            }
            catch (Exception ex)
            {
                AddError(null, String.Format("UnExpected Error: {0}", ex.ToString()),"");
                return (false);
            }   
        }

        public bool Parse(String FileName, ParseAdvice Advice)
        {
            try
            {
                if (!File.Exists(FileName))
                {
                    AddError(null, String.Format("Could not locate file : {0}", FileName), "");
                    return (false);
                }
                List<String> Lines = new List<string>();
                Lines.AddRange(File.ReadAllLines(FileName));
                return (Parse(Lines, Advice));

            }
            catch (Exception ex)
            {
                AddError(null, String.Format("UnExpected Error: {0}", ex.ToString()), "");
                return (false);
            }
        }

        public bool ParseLines(String[] Lines, ParseAdvice Advice)
        {
            try
            {
                List<String> L = new List<string>();
                L.AddRange(Lines);
                return (Parse(L, Advice));
            }
            catch (Exception ex)
            {
                AddError(null, String.Format("UnExpected Error: {0}", ex.ToString()), "");
                return (false);
            }
        }

        public void Dispose()
        {
            if (ParsedData != null)
            {
                ParsedData.Dispose();
                ParsedData = null;
            }
        }
    }
}
