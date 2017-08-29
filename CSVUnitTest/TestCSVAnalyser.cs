using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSVAnalyze.Analyzer;
using System.Text;
using System.IO;

namespace CSVUnitTest
{
	[TestClass]
	public class TestCSVAnalyser
	{
		[TestMethod]
		public void Test_File_ToFiles()
		{
			CSVAnalyzer ana = new CSVAnalyzer(@"..\..\..\CSVAnalyze\SampleData\Sample.csv", "NameFrequency.txt", "Address.txt");
			if (!ana.Run())
			{
				throw (new Exception("Failed to Analyze the CSV File"));
			}

			StringBuilder ExpectedAddress = new StringBuilder();
			ExpectedAddress.AppendLine("65 Ambling Way");
			ExpectedAddress.AppendLine("8 Crimson Rd");
			ExpectedAddress.AppendLine("12 Howard St");
			ExpectedAddress.AppendLine("102 Long Lane");
			ExpectedAddress.AppendLine("94 Roland St");
			ExpectedAddress.AppendLine("78 Short Lane");
			ExpectedAddress.AppendLine("82 Stewart St");
			ExpectedAddress.AppendLine("49 Sutherland St");

			String LoadedAddress = File.ReadAllText("Address.txt");
			Assert.AreEqual(ExpectedAddress.ToString(), LoadedAddress, "The output written to Address.txt is incorrect");


			StringBuilder ExpectedNames = new StringBuilder();
			ExpectedNames.AppendLine("Brown,2");
			ExpectedNames.AppendLine("Clive,2");
			ExpectedNames.AppendLine("Graham,2");
			ExpectedNames.AppendLine("Howe,2");
			ExpectedNames.AppendLine("James,2");
			ExpectedNames.AppendLine("Owen,2");
			ExpectedNames.AppendLine("Smith,2");
			ExpectedNames.AppendLine("Jimmy,1");
			ExpectedNames.AppendLine("John,1");

			String LoadedNames = File.ReadAllText("NameFrequency.txt");
			Assert.AreEqual(ExpectedNames.ToString(), LoadedNames, "The output written to NameFrequency.txt is incorrect");



		}

		private void BlobTest_1(StringBuilder Blob)
		{
			CSVAnalyzer ana = new CSVAnalyzer(Blob.ToString());
			if (!ana.Run())
			{
				throw (new Exception("BlobTest_1:Failed to Analyze Blob"));
			}

			StringBuilder ExpectedAddress = new StringBuilder();
			ExpectedAddress.AppendLine("102 Long Lane");
			Assert.AreEqual(ExpectedAddress.ToString(), ana.TargetAddressSortBlob.ToString(), "BlobTest_1:The Address Blob is incorrect.");


			StringBuilder ExpectedNames = new StringBuilder();
			ExpectedNames.AppendLine("Jimmy,1");
			ExpectedNames.AppendLine("Smith,1");
			Assert.AreEqual(ExpectedNames.ToString(), ana.TargetNameFrequencyBlob.ToString(), "BlobTest_1:The NameFrequency Blob is incorrect.");
		}

		private void BlobTest_2(StringBuilder Blob)
		{
			CSVAnalyzer ana = new CSVAnalyzer(Blob.ToString());
			if (!ana.Run())
			{
				throw (new Exception("BlobTest_2:Failed to Analyze Blob"));
			}

			StringBuilder ExpectedAddress = new StringBuilder();
			ExpectedAddress.AppendLine("65 Ambling Way");
			ExpectedAddress.AppendLine("102 Long Lane");
			Assert.AreEqual(ExpectedAddress.ToString(), ana.TargetAddressSortBlob.ToString(), "BlobTest_2:The Address Blob is incorrect.");


			StringBuilder ExpectedNames = new StringBuilder();
			ExpectedNames.AppendLine("Clive,1");
			ExpectedNames.AppendLine("Jimmy,1");
			ExpectedNames.AppendLine("Owen,1");
			ExpectedNames.AppendLine("Smith,1");
			Assert.AreEqual(ExpectedNames.ToString(), ana.TargetNameFrequencyBlob.ToString(), "BlobTest_2:The NameFrequency Blob is incorrect.");
		}

		private void BlobTest_3(StringBuilder Blob)
		{
			CSVAnalyzer ana = new CSVAnalyzer(Blob.ToString());
			if (!ana.Run())
			{
				throw (new Exception("BlobTest_3:Failed to Analyze Blob"));
			}

			StringBuilder ExpectedAddress = new StringBuilder();
			ExpectedAddress.AppendLine("65 Ambling Way");
			ExpectedAddress.AppendLine("102 Long Lane");
			ExpectedAddress.AppendLine("82 Stewart St");
			Assert.AreEqual(ExpectedAddress.ToString(), ana.TargetAddressSortBlob.ToString(), "BlobTest_3:The Address Blob is incorrect.");


			StringBuilder ExpectedNames = new StringBuilder();
			ExpectedNames.AppendLine("Brown,1");
			ExpectedNames.AppendLine("Clive,1");
			ExpectedNames.AppendLine("James,1");
			ExpectedNames.AppendLine("Jimmy,1");
			ExpectedNames.AppendLine("Owen,1");
			ExpectedNames.AppendLine("Smith,1");
			Assert.AreEqual(ExpectedNames.ToString(), ana.TargetNameFrequencyBlob.ToString(), "BlobTest_3:The NameFrequency Blob is incorrect.");
		}

		private void BlobTest_4(StringBuilder Blob)
		{
			CSVAnalyzer ana = new CSVAnalyzer(Blob.ToString());
			if (!ana.Run())
			{
				throw (new Exception("BlobTest_4:Failed to Analyze Blob"));
			}

			StringBuilder ExpectedAddress = new StringBuilder();
			ExpectedAddress.AppendLine("65 Ambling Way");
			ExpectedAddress.AppendLine("12 Howard St");
			ExpectedAddress.AppendLine("102 Long Lane");
			ExpectedAddress.AppendLine("82 Stewart St");
			Assert.AreEqual(ExpectedAddress.ToString(), ana.TargetAddressSortBlob.ToString(), "BlobTest_4:The Address Blob is incorrect.");


			StringBuilder ExpectedNames = new StringBuilder();
			ExpectedNames.AppendLine("Brown,1");
			ExpectedNames.AppendLine("Clive,1");
			ExpectedNames.AppendLine("Graham,1");
			ExpectedNames.AppendLine("Howe,1");
			ExpectedNames.AppendLine("James,1");
			ExpectedNames.AppendLine("Jimmy,1");
			ExpectedNames.AppendLine("Owen,1");
			ExpectedNames.AppendLine("Smith,1");
			Assert.AreEqual(ExpectedNames.ToString(), ana.TargetNameFrequencyBlob.ToString(), "BlobTest_4:The NameFrequency Blob is incorrect.");
		}

		private void BlobTest_5(StringBuilder Blob)
		{
			CSVAnalyzer ana = new CSVAnalyzer(Blob.ToString());
			if (!ana.Run())
			{
				throw (new Exception("BlobTest_5:Failed to Analyze Blob"));
			}

			StringBuilder ExpectedAddress = new StringBuilder();
			ExpectedAddress.AppendLine("65 Ambling Way");
			ExpectedAddress.AppendLine("12 Howard St");
			ExpectedAddress.AppendLine("102 Long Lane");
			ExpectedAddress.AppendLine("78 Short Lane");
			ExpectedAddress.AppendLine("82 Stewart St");
			Assert.AreEqual(ExpectedAddress.ToString(), ana.TargetAddressSortBlob.ToString(), "BlobTest_5:The Address Blob is incorrect.");


			StringBuilder ExpectedNames = new StringBuilder();
			ExpectedNames.AppendLine("Howe,2");
			ExpectedNames.AppendLine("Brown,1");
			ExpectedNames.AppendLine("Clive,1");
			ExpectedNames.AppendLine("Graham,1");
			ExpectedNames.AppendLine("James,1");
			ExpectedNames.AppendLine("Jimmy,1");
			ExpectedNames.AppendLine("John,1");
			ExpectedNames.AppendLine("Owen,1");
			ExpectedNames.AppendLine("Smith,1");
			Assert.AreEqual(ExpectedNames.ToString(), ana.TargetNameFrequencyBlob.ToString(), "BlobTest_5:The NameFrequency Blob is incorrect.");
		}

		private void BlobTest_6(StringBuilder Blob)
		{
			CSVAnalyzer ana = new CSVAnalyzer(Blob.ToString());
			if (!ana.Run())
			{
				throw (new Exception("BlobTest_6:Failed to Analyze Blob"));
			}

			StringBuilder ExpectedAddress = new StringBuilder();
			ExpectedAddress.AppendLine("65 Ambling Way");
			ExpectedAddress.AppendLine("12 Howard St");
			ExpectedAddress.AppendLine("102 Long Lane");
			ExpectedAddress.AppendLine("78 Short Lane");
			ExpectedAddress.AppendLine("82 Stewart St");
			ExpectedAddress.AppendLine("49 Sutherland St");
			Assert.AreEqual(ExpectedAddress.ToString(), ana.TargetAddressSortBlob.ToString(), "BlobTest_6:The Address Blob is incorrect.");


			StringBuilder ExpectedNames = new StringBuilder();
			ExpectedNames.AppendLine("Clive,2");
			ExpectedNames.AppendLine("Howe,2");
			ExpectedNames.AppendLine("Smith,2");
			ExpectedNames.AppendLine("Brown,1");
			ExpectedNames.AppendLine("Graham,1");
			ExpectedNames.AppendLine("James,1");
			ExpectedNames.AppendLine("Jimmy,1");
			ExpectedNames.AppendLine("John,1");
			ExpectedNames.AppendLine("Owen,1");
			Assert.AreEqual(ExpectedNames.ToString(), ana.TargetNameFrequencyBlob.ToString(), "BlobTest_6:The NameFrequency Blob is incorrect.");
		}

		private void BlobTest_7(StringBuilder Blob)
		{
			CSVAnalyzer ana = new CSVAnalyzer(Blob.ToString());
			if (!ana.Run())
			{
				throw (new Exception("BlobTest_7:Failed to Analyze Blob"));
			}

			StringBuilder ExpectedAddress = new StringBuilder();
			ExpectedAddress.AppendLine("65 Ambling Way");
			ExpectedAddress.AppendLine("8 Crimson Rd");
			ExpectedAddress.AppendLine("12 Howard St");
			ExpectedAddress.AppendLine("102 Long Lane");
			ExpectedAddress.AppendLine("78 Short Lane");
			ExpectedAddress.AppendLine("82 Stewart St");
			ExpectedAddress.AppendLine("49 Sutherland St");
			Assert.AreEqual(ExpectedAddress.ToString(), ana.TargetAddressSortBlob.ToString(), "BlobTest_7:The Address Blob is incorrect.");

			
			StringBuilder ExpectedNames = new StringBuilder();
			ExpectedNames.AppendLine("Clive,2");
			ExpectedNames.AppendLine("Howe,2");
			ExpectedNames.AppendLine("James,2");
			ExpectedNames.AppendLine("Owen,2");
			ExpectedNames.AppendLine("Smith,2");
			ExpectedNames.AppendLine("Brown,1");
			ExpectedNames.AppendLine("Graham,1");
			ExpectedNames.AppendLine("Jimmy,1");
			ExpectedNames.AppendLine("John,1");
			Assert.AreEqual(ExpectedNames.ToString(), ana.TargetNameFrequencyBlob.ToString(), "BlobTest_7:The NameFrequency Blob is incorrect.");
		}

		private void BlobTest_8(StringBuilder Blob)
		{
			CSVAnalyzer ana = new CSVAnalyzer(Blob.ToString());
			if (!ana.Run())
			{
				throw (new Exception("BlobTest_8:Failed to Analyze Blob"));
			}

			StringBuilder ExpectedAddress = new StringBuilder();
			ExpectedAddress.AppendLine("65 Ambling Way");
			ExpectedAddress.AppendLine("8 Crimson Rd");
			ExpectedAddress.AppendLine("12 Howard St");
			ExpectedAddress.AppendLine("102 Long Lane");
			ExpectedAddress.AppendLine("94 Roland St");
			ExpectedAddress.AppendLine("78 Short Lane");
			ExpectedAddress.AppendLine("82 Stewart St");
			ExpectedAddress.AppendLine("49 Sutherland St");
			Assert.AreEqual(ExpectedAddress.ToString(), ana.TargetAddressSortBlob.ToString(), "BlobTest_8:The Address Blob is incorrect.");


			StringBuilder ExpectedNames = new StringBuilder();
			ExpectedNames.AppendLine("Brown,2");
			ExpectedNames.AppendLine("Clive,2");
			ExpectedNames.AppendLine("Graham,2");
			ExpectedNames.AppendLine("Howe,2");
			ExpectedNames.AppendLine("James,2");
			ExpectedNames.AppendLine("Owen,2");
			ExpectedNames.AppendLine("Smith,2");
			ExpectedNames.AppendLine("Jimmy,1");
			ExpectedNames.AppendLine("John,1");
			Assert.AreEqual(ExpectedNames.ToString(), ana.TargetNameFrequencyBlob.ToString(), "BlobTest_8:The NameFrequency Blob is incorrect.");
		}

		[TestMethod]
		public void Test_Blobs()
		{
			StringBuilder Blob = new StringBuilder();
			Blob.AppendLine("FirstName,LastName,Address,PhoneNumber");
			Blob.AppendLine("Jimmy,Smith,102 Long Lane,29384857");
			BlobTest_1(Blob);

			Blob.AppendLine("Clive,Owen,65 Ambling Way,31214788");
			BlobTest_2(Blob);

			Blob.AppendLine("James,Brown,82 Stewart St,32114566");
			BlobTest_3(Blob);

			Blob.AppendLine("Graham,Howe,12 Howard St,8766556");
			BlobTest_4(Blob);

			Blob.AppendLine("John,Howe,78 Short Lane,29384857");
			BlobTest_5(Blob);

			Blob.AppendLine("Clive,Smith,49 Sutherland St,31214788");
			BlobTest_6(Blob);

			Blob.AppendLine("James,Owen,8 Crimson Rd,32114566");
			BlobTest_7(Blob);

			Blob.AppendLine("Graham,Brown,94 Roland St,8766556");
			BlobTest_8(Blob);




		}
	}
}
