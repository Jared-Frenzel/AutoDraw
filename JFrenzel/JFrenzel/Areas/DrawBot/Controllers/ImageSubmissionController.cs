using DataModels;
using JFrenzel.Interfaces;
using System;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JFrenzel.Areas.DrawBot.Controllers
{
	public class ImageSubmissionController : Controller
	{
		readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private IEFStore<DrawBotImage> dbImageStore;

		public ImageSubmissionController(IEFStore<DrawBotImage> dbImageStore)
		{
			this.dbImageStore = dbImageStore;
		}

		// GET: DrawBot/ImageSubmission
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		//https://www.aurigma.com/upload-suite/developers/aspnet-mvc/how-to-upload-files-in-aspnet-mvc
		// POST: DrawBot/ImageSubmission
		[HttpPost]
		public ActionResult Index(HttpPostedFileBase file)
		{
			if (file != null && file.ContentLength > 0)
				try
				{
					//Create an image from the file, and convert it to the DrawBot image format. Store in DB as well
					Bitmap img = new Bitmap(Image.FromStream(file.InputStream));

					DrawBotImage dbImage = new DrawBotImage();
					dbImage.Height = img.Height;
					dbImage.Width = img.Width;

					//Each channel is stored as a string of chars (0-255)
					StringBuilder RedBuilder = new StringBuilder();
					StringBuilder GreenBuilder = new StringBuilder();
					StringBuilder BlueBuilder = new StringBuilder();

					for (int x = 0; x < dbImage.Width; x++)
					{
						for (int y = 0; y < dbImage.Height; y++)
						{
							Color px = img.GetPixel(x, y);

							RedBuilder.Append((char)px.R);
							BlueBuilder.Append((char)px.B);
							GreenBuilder.Append((char)px.G);
						}
					}

					dbImage.RedChannel = RedBuilder.ToString();
					dbImage.GreenChannel = GreenBuilder.ToString();
					dbImage.BlueChannel = BlueBuilder.ToString();

					dbImage = this.dbImageStore.Create(dbImage);

					//TODO: Put some verification in here.
					ViewBag.Message = "File uploaded successfully";
				}
				catch (OutOfMemoryException ex)
				{
					ViewBag.Message = "Error: You uploaded an unsupported file type. Please uplad a BMP, GIF, JPEG, PNG, or TIFF file.";
				}
				catch (Exception ex)
				{
					logger.Error("ImageSubmissionController: Error occured while converting submitted file to DrawBotImage; Error Message: " + ex.Message);
					throw new Exception("ImageSubmissionController: Error occured while converting submitted file to DrawBotImage", ex);
				}
			else
			{
				ViewBag.Message = "You have not specified a file.";
			}

			return View();
		}
	}
}