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
				areImagesSimilar = distortion < 1000; //xxx move to args

                //TODO rename to be GetActualImageFilePath
				diffImage.Write(dirManager.GetTempImageFilePath(testName + "_diff"));
			}
			
			/* .NET comparing:
			ImageComparer.Compare Method(Image, Image, Image)
			https://msdn.microsoft.com/en-us/library/hh135398.aspx
			*/
			//var actualImage = Image.FromFile(actualImageFilePath);
			//var expectedImage = Image.FromFile(expectedImageFilePath);
			//ColorDifference argbTolerance = new ColorDifference(0xDD, 0xDD, 0xDD, 0xDD); //xxx make configurable
			//Image diffImage;
			//bool areImagesSimilar = ImageComparer.Compare(actualImage, expectedImage, argbTolerance, out diffImage);
			//DirectoryManager dirManager = new DirectoryManager(_argProc);
			//diffImage.Save(dirManager.GetDiffImageFilePath(testName));
		/*
		blurring:
		https://msdn.microsoft.com/en-us/library/system.windows.media.effects.blureffect.aspx
		*/
			
		/* colorizing
		Namespace:   Microsoft.VisualStudio.TestTools.UITesting
		Assembly:  Microsoft.VisualStudio.TestTools.UITesting (in Microsoft.VisualStudio.TestTools.UITesting.dll)

		grayscale + colorizing:
		http://www.codeproject.com/Articles/3772/ColorMatrix-Basics-Simple-Image-Color-Adjustment
		*/
			return areImagesSimilar ? CompareResult.Similar : CompareResult.Different;
		}
	}
}
