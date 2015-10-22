//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using ImageMagick;

namespace screenium
{
	class CustomImageComparer
	{
		private readonly ArgsProcessor _argProc;

		internal CustomImageComparer(ArgsProcessor argProc)
		{
			_argProc = argProc;
		}

		internal CompareResult CompareImages(string actualImageFilePath, string expectedImageFilePath, string testName, ArgsProcessor argProc)
		{
			bool areImagesSimilar;

			//image library (.NET wrapper) - Magick.NET
			using (MagickImage image1 = new MagickImage(actualImageFilePath))
			using (MagickImage image2 = new MagickImage(expectedImageFilePath))
			using (MagickImage diffImage = new MagickImage())
			{
				DirectoryManager dirManager = new DirectoryManager(argProc);

				double distortion = image1.Compare(image2, ErrorMetric.Absolute, diffImage);
				areImagesSimilar = distortion < 1000; //TODO move to args

				diffImage.Write(dirManager.GetDiffImageFilePath(testName));
			}
			
			return areImagesSimilar ? CompareResult.Similar : CompareResult.Different;
		}
	}
}
