using AutoDraw.Areas.DrawBot.Interfaces;
using System;
using System.Drawing;
using System.Linq;

namespace AutoDraw.Areas.DrawBot.Implementations
{
	public class ImageTransformHelper : IImageTransformHelper
	{
		private void DecomposeBitmap(Bitmap originalImage, out double[,] red, out double[,] green, out double[,] blue)
		{
			red = new double[originalImage.Width, originalImage.Height];
			green = new double[originalImage.Width, originalImage.Height];
			blue = new double[originalImage.Width, originalImage.Height];

			for (int i = 0; i < originalImage.Width; i++)
			{
				for (int j = 0; j < originalImage.Height; j++)
				{
					Color c = originalImage.GetPixel(i, j);
					red[i, j] = c.R;
					green[i, j] = c.G;
					blue[i, j] = c.B;
				}
			}
		}

		private Bitmap ComposeBitmap(double[,] red, double[,] green, double[,] blue)
		{
			Bitmap ret = new Bitmap(red.GetLength(0), red.GetLength(1));

			for (int x = 0; x < ret.Width; x++)
			{
				for (int y = 0; y < ret.Height; y++)
				{
					Color c = Color.FromArgb((int)red[x, y], (int)green[x, y], (int)blue[x, y]);
					ret.SetPixel(x, y, c);
				}
			}

			return ret;
		}

		private double[,] KernelConvolution(double[,] img, double[,] kernel)
		{
			double[,] retMat = new double[img.GetLength(0), img.GetLength(1)];
			int kernelRadius = (kernel.GetLength(0) - 1) / 2;


			//Iterate over the image
			for (int x = 0; x < img.GetLength(0); x++)
			{
				for (int y = 0; y < img.GetLength(1); y++)
				{
					//Iterate over the kernel
					for (int i = Math.Max(-x, -kernelRadius); i <= Math.Min(img.GetLength(0) - 1 - x, kernelRadius); i++)
					{
						for (int j = Math.Max(-y, -kernelRadius); j <= Math.Min(img.GetLength(1) - 1 - y, kernelRadius); j++)
						{
							retMat[x, y] += kernel[i + kernelRadius, j + kernelRadius] * img[x + i, y + j];
						}
					}
				}
			}

			return retMat;
		}

		/// <summary>
		/// Creates a bitmap that is composed of the red channel of the given bitmap
		/// </summary>
		/// <param name="originalImage">The bitmap to base the resultant bitmap off of</param>
		/// <returns>A bitmap that is composed of the red channel of the given bitmap</returns>
		public Bitmap GetRedChannel(Bitmap originalImage)
		{
			Bitmap result = new Bitmap(originalImage);

			for (int x = 0; x < result.Width; x++)
			{
				for (int y = 0; y < result.Height; y++)
				{
					int redVal = result.GetPixel(x, y).R;
					result.SetPixel(x, y, Color.FromArgb(redVal, 0, 0));
				}
			}

			return result;
		}

		/// <summary>
		/// Creates a bitmap that is composed of the green channel of the given bitmap
		/// </summary>
		/// <param name="originalImage">The bitmap to base the resultant bitmap off of</param>
		/// <returns>A bitmap that is composed of the green channel of the given bitmap</returns>
		public Bitmap GetGreenChannel(Bitmap originalImage)
		{
			Bitmap result = new Bitmap(originalImage);

			for (int x = 0; x < result.Width; x++)
			{
				for (int y = 0; y < result.Height; y++)
				{
					int greenVal = result.GetPixel(x, y).G;
					result.SetPixel(x, y, Color.FromArgb(0, greenVal, 0));
				}
			}

			return result;
		}

		/// <summary>
		/// Creates a bitmap that is composed of the blue channel of the given bitmap
		/// </summary>
		/// <param name="originalImage">The bitmap to base the resultant bitmap off of</param>
		/// <returns>A bitmap that is composed of the blue channel of the given bitmap</returns>
		public Bitmap GetBlueChannel(Bitmap originalImage)
		{
			Bitmap result = new Bitmap(originalImage);

			for (int x = 0; x < result.Width; x++)
			{
				for (int y = 0; y < result.Height; y++)
				{
					int blueVal = result.GetPixel(x, y).B;
					result.SetPixel(x, y, Color.FromArgb(0, 0, blueVal));
				}
			}

			return result;
		}

		/// <summary>
		/// Uses a mean grayscale algorithm to create a grayscale representation of the given bitmap
		/// </summary>
		/// <param name="originalImage">The image to be grayscaled</param>
		/// <returns>A bitmap containing a grayscale representation of the given image</returns>
		public Bitmap GetGrayscale(Bitmap originalImage)
		{
			Bitmap result = new Bitmap(originalImage);

			for (int x = 0; x < result.Width; x++)
			{
				for (int y = 0; y < result.Height; y++)
				{
					int grayVal = (result.GetPixel(x, y).R + result.GetPixel(x, y).G + result.GetPixel(x, y).B) / 3;
					result.SetPixel(x, y, Color.FromArgb(grayVal, grayVal, grayVal));
				}
			}

			return result;
		}

		/// <summary>
		/// Uses an edge detection algorithm to detect edges in the bitmap
		/// </summary>
		/// <param name="originalImage">The bitmap to use the algorithm on</param>
		/// <returns>A grayscale image with the edges highlighted</returns>
		public Bitmap GetEdgeDetection(Bitmap originalImage)
		{
			//Convert to grayscale
			originalImage = GetGrayscale(originalImage);

			//Apply a gaussian filter to remove noise
			originalImage = GetBlur(originalImage, 3, .84089242);

			//Apply sobel operator in x
			double[,] xSobelOperator = { { 1, 0, -1 }, { 2, 0, -2 }, { 1, 0, -1 } };
			double[,] gray;
			DecomposeBitmap(originalImage, out gray, out gray, out gray);
			double[,] xSobel = KernelConvolution(gray, xSobelOperator);


			//Apply sobel operator in y
			double[,] ySobelOperator = { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
			double[,] ySobel = KernelConvolution(gray, ySobelOperator);

			//Combine for result
			for (int x = 0; x < originalImage.Width; x++)
			{
				for (int y = 0; y < originalImage.Height; y++)
				{
					//TODO: Place this cutoff on client side to alide for slider.
					int grayVal = (Math.Sqrt(Math.Pow(ySobel[x, y], 2) + Math.Pow(xSobel[x, y], 2)) / (4 * Math.Sqrt(2))) > 10 ? 255 : 0;
					originalImage.SetPixel(x, y, Color.FromArgb(grayVal, grayVal, grayVal));
				}
			}

			return originalImage;
		}

		/// <summary>
		/// Uses a gaussian blurring algorithm to blur a bitmap
		/// </summary>
		/// <param name="originalImage">The bitmap to apply the blue to</param>
		/// <param name="radius">The size of the kernel used to apply the blur</param>
		/// <param name="standardDeviation">The deviation in the constants used to create blur kernel</param>
		/// <returns></returns>
		public Bitmap GetBlur(Bitmap originalImage, int radius, double standardDeviation)
		{
			//TODO: Allow client to select std deviation of blur.
			//Create the gaussian matrix to be used as the kernel
			double[,] kernel = new double[2 * radius + 1, 2 * radius + 1];

			for (int x = 0; x < 2 * radius + 1; x++)
			{
				for (int y = 0; y < 2 * radius + 1; y++)
				{
					double displacement = Math.Sqrt(Math.Pow(x - radius, 2) + Math.Pow(y - radius, 2));
					kernel[x, y] = 1 / (standardDeviation * Math.Sqrt(Math.PI * 2)) * Math.Pow(Math.E, -1 * Math.Pow(displacement, 2) / (2 * Math.Pow(standardDeviation, 2)));
				}
			}

			double kernelWeight = kernel.Cast<double>().Sum();

			double[,] red, green, blue;
			DecomposeBitmap(originalImage, out red, out green, out blue);

			red = KernelConvolution(red, kernel);
			green = KernelConvolution(green, kernel);
			blue = KernelConvolution(blue, kernel);

			//Must scale them by weighted value
			for (int x = 0; x < originalImage.Width; x++)
			{
				for (int y = 0; y < originalImage.Height; y++)
				{
					red[x, y] /= kernelWeight;
					green[x, y] /= kernelWeight;
					blue[x, y] /= kernelWeight;
				}
			}

			return ComposeBitmap(red, green, blue);
		}

	}
}

