using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;

namespace MergeDocuments
{
	internal class Program
	{
		public static void Main()
		{
			string[] documentPaths = { "C:\\path1", "C:\\path2" };
			string outputFilePath = "mergedDocument.pdf";

			if (IsDocumentPathValid(documentPaths))
			{
				MergeDocumentsIntoSinglePDF(outputFilePath, documentPaths);

				byte[] mergedDocumentBytes = File.ReadAllBytes(outputFilePath);
				string base64String = Convert.ToBase64String(mergedDocumentBytes);

				Console.WriteLine(base64String);
			}


		}

		private static bool IsDocumentPathValid(string[] documentPaths)
		{
			var isValid = true;
			foreach (string item in documentPaths)
			{
				if (File.Exists(item)) continue;

				Console.WriteLine($"Path {item} is invalid");
				isValid = false;
				break;

			}

			return isValid;
			;
		}

		private static void MergeDocumentsIntoSinglePDF(string outputFilePath, string[] documentPaths)
		{
			Console.WriteLine("Merging started.....");
			PdfDocument outputPDFDocument = new PdfDocument();
			foreach (string documentPath in documentPaths)
			{
				PdfDocument inputPDFDocument = PdfReader.Open(documentPath, PdfDocumentOpenMode.Import);
				outputPDFDocument.Version = inputPDFDocument.Version;
				foreach (PdfPage page in inputPDFDocument.Pages)
				{
					outputPDFDocument.AddPage(page);
				}
			}

			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
			outputPDFDocument.Save(outputFilePath);
			Console.WriteLine("Merging Completed");
		}
	}
}