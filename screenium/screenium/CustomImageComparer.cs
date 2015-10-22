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

		internal CompareResultDescription CompareImages(string actualImageFilePath, string expectedImageFilePath, string testName)
		{
			bool areImagesSimilar;
		    double distortion;
		    var tolerance = _argProc.GetArgAsDouble(ArgsProcessor.Args.DIFFERENCE_TOLERANCE);

			//image library (.NET wrapper) - Magick.NET
			using (MagickImage image1 = new MagickImage(actualImageFilePath))
			using (MagickImage image2 = new MagickImage(expectedImageFilePath))
			using (MagickImage diffImage = new MagickImage())
			{
				DirectoryManager dirManager = new DirectoryManager(_argProc);

                //ErrorMetric - ref: http://www.imagemagick.org/Usage/compare/#statistics
                distortion = image1.Compare(image2, ErrorMetric.MeanAbsolute, diffImage);
				areImagesSimilar = distortion < tolerance;

				diffImage.Write(dirManager.GetDiffImageFilePath(testName));
			}
			
            var result = areImagesSimilar ? CompareResult.Similar : CompareResult.Different;
		    return new CompareResultDescription()
		    {
                Result = result,
                Distortion = distortion,
                Tolerance = tolerance
		    };
		}
	}
}
