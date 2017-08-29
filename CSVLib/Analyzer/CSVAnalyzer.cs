using CSVTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CSVAnalyze.Analyzer
{
	public class CSVAnalyzer
	{
		private bool _IsFileBased;

		private String _FileToParse;
		private String _TargetFile_ForNamesFrequency;
		private String _TargetFile_ForAddresses;

		private String _BlobToParse;

		public StringBuilder TargetNameFrequencyBlob = new StringBuilder();
		public StringBuilder TargetAddressSortBlob = new StringBuilder();


		public CSVAnalyzer(String FileToParse, String TargetFile_ForNamesFrequency, String TargetFile_ForAddresses)
		{
			_IsFileBased = true;

			_FileToParse = FileToParse;
			_TargetFile_ForNamesFrequency = TargetFile_ForNamesFrequency;
			_TargetFile_ForAddresses = TargetFile_ForAddresses;
		}

		public CSVAnalyzer(String BlobToParse)
		{
			_IsFileBased = false;
			_BlobToParse = BlobToParse;
		}


		public bool RunFile()
		{
			try
			{
				using (FileOrBlobParser TheFileParser = new FileOrBlobParser(_FileToParse, AnalyzeType.File))
				{
					if (TheFileParser.Parse())
					{
						bool DidProccess = CSVLib.Analyzer.Analyzer.ProccessData(TheFileParser, TargetNameFrequencyBlob, TargetAddressSortBlob);
						if (DidProccess)
						{
							File.WriteAllText(_TargetFile_ForNamesFrequency, TargetNameFrequencyBlob.ToString());
							File.WriteAllText(_TargetFile_ForAddresses, TargetAddressSortBlob.ToString());
						}
						return (DidProccess);

					}
					else
					{
						Console.WriteLine(TheFileParser.GetErrors());
						return (false);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return (false);
			}
		}



		public bool RunBlob()
		{
			try
			{
				using (FileOrBlobParser TheFileParser = new FileOrBlobParser(_BlobToParse, AnalyzeType.Blob))
				{
					if (TheFileParser.Parse())
					{
						bool DidProccess = CSVLib.Analyzer.Analyzer.ProccessData(TheFileParser, TargetNameFrequencyBlob, TargetAddressSortBlob);
						return (DidProccess);

					}
					else
					{
						String Errors = TheFileParser.GetErrors();
						Console.WriteLine(Errors);
						return (false);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return (false);
			}
		}

		public bool Run()
		{
			if (_IsFileBased)
			{
				return (RunFile());
			}
			else
			{
				return (RunBlob());
			}

		}
	}
}
