using CSVAnalyze.Analyzer;
using System;


namespace CSVAnalyze
{
	class Program
	{
		static void Main(string[] args)
		{

			CSVAnalyzer ana = new CSVAnalyzer(@"SampleData\Sample.csv","NameFrequency.txt","Address.txt");
			bool result = ana.Run();

			Console.WriteLine("Press enter to exit...");
			Console.ReadLine();
		}
	}
}
