using CSVAnalyze.Analyzer;
using CSVTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVLib.Analyzer
{
	public class Analyzer
	{


		private static bool GetStreetParts(String[] Parts, ref int StreetNo, ref String StreetName)
		{
			if (Parts.Length == 3)
			{
				if (int.TryParse(Parts[0], out StreetNo))
				{
					StreetName = Parts[1] + " " + Parts[2];
					return (true);
				}
				else return (false);
			}
			else return (false);
		}
		private static void CountFisrtNameOrLastName(Dictionary<String, FreqCounter> Counter, string key)
		{
			String Ukey = key.ToUpper();
			if (Counter.ContainsKey(Ukey))
			{
				Counter[Ukey].IncrementCounter(key);
			}
			else
			{
				Counter.Add(Ukey, new FreqCounter(key));
			}
		}
		private static bool DoAnalysis(DataTable ProccessingTable, StringBuilder TargetNameFrequencyBlob, StringBuilder TargetAddressSortBlob)
		{

			int FirstName_Ordinal = ProccessingTable.Columns["FirstName"].Ordinal;
			int LastName_Ordinal = ProccessingTable.Columns["LastName"].Ordinal;
			int Address_StreetName_Ordinal = ProccessingTable.Columns["Address_StreetName"].Ordinal;
			int Address_No_Ordinal = ProccessingTable.Columns["Address_No"].Ordinal;
			int Address_Ordinal = ProccessingTable.Columns["Address"].Ordinal;

			Dictionary<String, FreqCounter> Counter = new Dictionary<string, FreqCounter>();

			foreach (DataRow dr in ProccessingTable.Rows)
			{
				CountFisrtNameOrLastName(Counter, dr[FirstName_Ordinal].ToString());
				CountFisrtNameOrLastName(Counter, dr[LastName_Ordinal].ToString());
			}

			foreach (FreqCounter fc in Counter.Values)
			{
				fc.CalculateTheMostUsedFormat();
			}

			List<FreqCounter> NamesInOrderedFrequency = Counter.Values.OrderByDescending(a => a.OverAllUsedTimes).ThenBy(b => b.CalculatedMostUsedFormat).ToList();
			foreach (FreqCounter fc in NamesInOrderedFrequency)
			{
				String l = String.Format("{0},{1}", fc.CalculatedMostUsedFormat, fc.OverAllUsedTimes);
				TargetNameFrequencyBlob.AppendLine(l);
			}
			
			
			List<DataRow> SortedByAddress = ProccessingTable.AsEnumerable().OrderBy(r => r[Address_StreetName_Ordinal].ToString()).ThenBy(b => b[Address_No_Ordinal]).ToList();
			foreach (DataRow dr in SortedByAddress)
			{
				TargetAddressSortBlob.AppendLine(dr[Address_Ordinal].ToString());
			}
			return (true);
		}


		public static bool ProccessData(FileOrBlobParser TheFileParser, StringBuilder TargetNameFrequencyBlob, StringBuilder TargetAddressSortBlob)
		{
			using (DataTable ProccessingTable = TheFileParser.GetParseAdvice().GetTargetSchema())
			{
				DataColumn Address_No = new DataColumn("Address_No", typeof(Int32));
				ProccessingTable.Columns.Add(Address_No);

				DataColumn Address_StreetName = new DataColumn("Address_StreetName", typeof(String));
				ProccessingTable.Columns.Add(Address_StreetName);

				int AdressOrdinal = ProccessingTable.Columns["Address"].Ordinal;

				int ErrorCount = 0;
				//copy existing Rows;
				foreach (DataRow dr in TheFileParser.ParsedData.Rows)
				{
					DataRow ProccessingTableRow = ProccessingTable.NewRow();

					//copy existing columns;
					foreach (DataColumn dc in TheFileParser.ParsedData.Columns)
					{
						ProccessingTableRow[dc.Ordinal] = dr[dc.Ordinal];
					}

					string[] Parts = dr[AdressOrdinal].ToString().Split(' ');

					String StreetName = "";
					int StreetNo = -1;
					if (GetStreetParts(Parts, ref StreetNo, ref StreetName))
					{
						ProccessingTableRow[Address_No.Ordinal] = StreetNo;
						ProccessingTableRow[Address_StreetName.Ordinal] = StreetName;

						ProccessingTable.Rows.Add(ProccessingTableRow);
					}
					else
					{
						int Original_LineNo = Convert.ToInt32(dr["Original_LineNo"]);
						String Original_Data = dr["Original_Data"].ToString();
						Console.WriteLine("Error in CSV, line {0}, Expected a number and name in Address ->{1}", Original_LineNo, Original_Data);

						ErrorCount++;
					}
				}

				if (ErrorCount == 0)
				{
					return (DoAnalysis(ProccessingTable, TargetNameFrequencyBlob,TargetAddressSortBlob));
				}

				return (ErrorCount == 0);
			}

		}
	}
}
