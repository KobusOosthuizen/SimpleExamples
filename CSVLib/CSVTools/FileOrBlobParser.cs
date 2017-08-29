using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CSVTools
{
	public enum AnalyzeType
	{
		File,
		Blob
	}


	public class FileOrBlobParser:Parser
	{
		private String _FromFileName;
		private String _Blob;

		private ParseAdvice advice = new ParseAdvice();

		public ParseAdvice GetParseAdvice()
		{
			return (advice);
		}

		private void SetUpAdvice()
		{
			advice.SplitWith = ",";
			advice.SkipLines = 1;
			advice.SkipBottomLines = 0;
			advice.ExpectColums = 4;
			advice.UseDoubleQuotesAsQuotes = true;
			advice.UseSingleQuotesAsQuotes = false;
			advice.SkipEmptyLines = true;

			List<ExpectedFormat> Required = new List<ExpectedFormat>();

			ExpectedFormat Col_FirstName = new ExpectedFormat();
			Col_FirstName.ColumNo = 1;
			Col_FirstName.MapToColumnNo = 1;
			Col_FirstName.MapToColumnName = "FirstName";
			Col_FirstName.MapToType = TypeCode.String;
			Col_FirstName.MinTextLength = null;
			Col_FirstName.MaxTextLength = null;
			Col_FirstName.MinValue = null;
			Col_FirstName.MaxValue = null;
			Col_FirstName.Format = null;
			Col_FirstName.OnlyAllowCharacters = null;
			Required.Add(Col_FirstName);

			ExpectedFormat Col_LastName = new ExpectedFormat();
			Col_LastName.ColumNo = 2;
			Col_LastName.MapToColumnNo = 2;
			Col_LastName.MapToColumnName = "LastName";
			Col_LastName.MapToType = TypeCode.String;
			Col_LastName.MinTextLength = null;
			Col_LastName.MaxTextLength = null;
			Col_LastName.MinValue = null;
			Col_LastName.MaxValue = null;
			Col_LastName.Format = null;
			Col_LastName.OnlyAllowCharacters = null;
			Required.Add(Col_LastName);

			ExpectedFormat Col_Address = new ExpectedFormat();
			Col_Address.ColumNo = 3;
			Col_Address.MapToColumnNo = 3;
			Col_Address.MapToColumnName = "Address";
			Col_Address.MapToType = TypeCode.String;
			Col_Address.MinTextLength = 1;
			Col_Address.MaxTextLength = 100;
			Col_Address.MinValue = null;
			Col_Address.MaxValue = null;
			Col_Address.Format = null;
			Col_Address.OnlyAllowCharacters = null;
			Required.Add(Col_Address);


			ExpectedFormat Col_PhoneNumber = new ExpectedFormat();
			Col_PhoneNumber.ColumNo = 4;
			Col_PhoneNumber.MapToColumnNo = 4;
			Col_PhoneNumber.MapToColumnName = "PhoneNumber";
			Col_PhoneNumber.MapToType = TypeCode.String;
			Col_PhoneNumber.MinTextLength = null;
			Col_PhoneNumber.MaxTextLength = null;
			Col_PhoneNumber.MinValue = null;
			Col_PhoneNumber.MaxValue = null;
			Col_PhoneNumber.Format = null;
			Col_PhoneNumber.OnlyAllowCharacters = "0123456789";
			Required.Add(Col_Address);

			advice.Mapping = Required.ToArray();

		}
		public FileOrBlobParser(String Data, AnalyzeType AType) : base()
		{
			switch (AType)
			{
				case AnalyzeType.File:
					_FromFileName = Data;
					_Blob = null;
					break;

				case AnalyzeType.Blob:
					_FromFileName = null;
					_Blob = Data ;
					break;
			}
			
			SetUpAdvice();
		}


		public bool Parse()
		{

			bool result = false;
			if (_FromFileName != null)
			{
				result = base.Parse(_FromFileName, advice);
			}
			else
			{
				List<String> Lines = _Blob.Replace("\r", "").Split('\n').ToList();
				result = base.Parse(Lines, advice);
			}

			if (result == false)
			{
				return(false);
			}
			if (base.Errors.Count > 0)
			{
			    return (false);
			}

			return (true);
		}

		public String GetErrors()
		{
			StringBuilder ErrorDescripions = new StringBuilder();
			foreach (KeyValuePair<int?, String> pair in Errors)
			{
				if (!pair.Key.HasValue)
				{
					ErrorDescripions.AppendFormat("Error : {0}", pair.Value);
					ErrorDescripions.AppendLine();
					ErrorDescripions.AppendLine();
				}
				else
				{
					ErrorDescripions.AppendFormat("Error in line {0}: {1}", pair.Key.Value, pair.Value);
					ErrorDescripions.AppendLine();
					ErrorDescripions.AppendLine();
				}
			}
			return (ErrorDescripions.ToString());
		}
	}
}
