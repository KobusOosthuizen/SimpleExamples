using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVAnalyze.Analyzer
{
	public class FreqCounter
	{
		public String UpperCaseKey;
		public int OverAllUsedTimes;
		public Dictionary<String, int> ExactCount = new Dictionary<string, int>();
		public String CalculatedMostUsedFormat;

		public FreqCounter(String key)
		{
			UpperCaseKey = key.ToUpper();
			if (ExactCount.ContainsKey(key))
			{
				ExactCount[key] = ExactCount[key] + 1;
			}
			else
			{
				ExactCount.Add(key, 1);
			}
			OverAllUsedTimes = 1;
		}

		public void IncrementCounter(String key)
		{
			if (ExactCount.ContainsKey(key))
			{
				ExactCount[key] = ExactCount[key] + 1;
			}
			else
			{
				ExactCount.Add(key, 1);
			}
			OverAllUsedTimes = OverAllUsedTimes + 1;
		}

		private String MostUsedFormat
		{
			get
			{
				int MostUsedCount = 0;
				String MostUsedText = "";
				foreach (KeyValuePair<String, int> pair in ExactCount)
				{
					if (pair.Value > MostUsedCount)
					{
						MostUsedCount = pair.Value;
						MostUsedText = pair.Key;
					}
				}
				return (MostUsedText);
			}
		}

		public void CalculateTheMostUsedFormat()
		{
			CalculatedMostUsedFormat = MostUsedFormat;
		}

	}
}
