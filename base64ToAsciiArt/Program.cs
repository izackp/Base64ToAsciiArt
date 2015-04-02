using System;
using System.IO;
using StaticDust;

namespace base64ToAsciiArt
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			args = new [] { "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", "2"};

			if (args.Length != 2 && args.Length != 1) {
				Console.WriteLine ("{\"error\": \"Invalid Parameters: must provide image as base64 and a pixel resolution\"}");
				return;
			}

			string base64Image = args [0];
			string pixelResolution = "1";
			if (args.Length == 2)
				pixelResolution = args [1];
			
			byte[] imageBytes = Convert.FromBase64String(base64Image);
			MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
			ms.Write(imageBytes, 0, imageBytes.Length);
			string result = AsciiArt.ConvertImage (ms, pixelResolution, "False");

			string[] tokens = result.Split(new []{"\n"}, StringSplitOptions.RemoveEmptyEntries);
			string joined = string.Join ("\",\n\"", tokens);
			Console.WriteLine ("{\"result\": [\n\"" + joined + "\"\n]\n}");
		}

	}
}
