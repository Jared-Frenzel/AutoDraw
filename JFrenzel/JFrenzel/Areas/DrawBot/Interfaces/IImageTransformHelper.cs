using System.Drawing;

namespace AutoDraw.Areas.DrawBot.Interfaces
{
	public interface IImageTransformHelper
	{
		/// <summary>
		/// Creates a bitmap that is composed of the red channel of the given bitmap
		/// </summary>
		/// <param name="originalImage">The bitmap to base the resultant bitmap off of</param>
		/// <returns>A bitmap that is composed of the red channel of the given bitmap</returns>
		Bitmap GetRedChannel(Bitmap originalImage);

		/// <summary>
		/// Creates a bitmap that is composed of the green channel of the given bitmap
		/// </summary>
		/// <param name="originalImage">The bitmap to base the resultant bitmap off of</param>
		/// <returns>A bitmap that is composed of the green channel of the given bitmap</returns>
		Bitmap GetGreenChannel(Bitmap originalImage);

		/// <summary>
		/// Creates a bitmap that is composed of the blue channel of the given bitmap
		/// </summary>
		/// <param name="originalImage">The bitmap to base the resultant bitmap off of</param>
		/// <returns>A bitmap that is composed of the blue channel of the given bitmap</returns>
		Bitmap GetBlueChannel(Bitmap originalImage);

		/// <summary>
		/// Uses an algorithm to create a grayscale representation of the given bitmap
		/// </summary>
		/// <param name="originalImage">The image to be grayscaled</param>
		/// <returns>A bitmap containing a grayscale representation of the given image</returns>
		Bitmap GetGrayscale(Bitmap originalImage);

		/// <summary>
		/// Uses an edge detection algorithm to detect edges in the bitmap
		/// </summary>
		/// <param name="originalImage">The bitmap to use the algorithm on</param>
		/// <returns>A grayscale image with the edges highlighted</returns>
		Bitmap GetEdgeDetection(Bitmap originalImage);

		/// <summary>
		/// Uses a blurring algorithm to blur a bitmap
		/// </summary>
		/// <param name="originalImage">The bitmap to apply the blue to</param>
		/// <param name="radius">The size of the kernel used to apply the blur</param>
		/// <param name="standardDeviation">The deviation in the constants used to create blur kernel</param>
		/// <returns></returns>
		Bitmap GetBlur(Bitmap originalImage, int radius, double standardDeviation);
	}
}
