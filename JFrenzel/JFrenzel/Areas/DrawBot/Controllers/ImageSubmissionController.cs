using AutoDraw.Areas.DrawBot.Interfaces;
using DataModels;
using JFrenzel.Areas.DrawBot.ViewModels;
using JFrenzel.Interfaces;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using static DataModels.DrawBotImage;

namespace JFrenzel.Areas.DrawBot.Controllers
{
	public class ImageSubmissionController : Controller
	{
		readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private IEFStore<DrawBotImage> dbImageStore;
		private IImageTransformHelper imageTransformHelper;

		public ImageSubmissionController(IEFStore<DrawBotImage> dbImageStore, IImageTransformHelper imageTransformHelper)
		{
			this.dbImageStore = dbImageStore;
			this.imageTransformHelper = imageTransformHelper;
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
			//Will store ids to be passed onto preview after image has been uploaded
			ImageSubmissionPreviewViewModel vm = new ImageSubmissionPreviewViewModel();

			if (file != null && file.ContentLength > 0)
				try
				{
					//Create an image from the file, and convert it to the DrawBot image format. Store in DB as well
					Bitmap img = new Bitmap(Image.FromStream(file.InputStream));

					DrawBotImage dbImage = new DrawBotImage(HttpContext.User.Identity.Name, DateTime.Now);
					string savePath = Path.Combine(Server.MapPath("~/Images"), dbImage.ImagePath);
					Directory.CreateDirectory(Path.GetDirectoryName(savePath));
					img.Save(savePath);

					dbImage = this.dbImageStore.Create(dbImage);
					vm.OriginalId = dbImage.Id;

					//TODO: Put some verification in here.
					ViewBag.Message = "File uploaded successfully";
				}
				catch (OutOfMemoryException)
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

			return View("Preview", vm);
		}

		public ActionResult GetPreviewImage(int Id, TRANSFORM_TYPE type)
		{
			//Retrieve the desired image from the db and reconstruct a bmp object
			DrawBotImage dbImage = this.dbImageStore.FindById(Id);

			Bitmap img = new Bitmap(Path.Combine(Server.MapPath("~/Images"), dbImage.ImagePath));

			//Transform the image if necessary
			Bitmap ret = null;
			switch (type)
			{
				case TRANSFORM_TYPE.RED_CHANNEL:
					ret = this.imageTransformHelper.GetRedChannel(img);
					break;
				case TRANSFORM_TYPE.GREEN_CHANNEL:
					ret = this.imageTransformHelper.GetGreenChannel(img);
					break;
				case TRANSFORM_TYPE.BLUE_CHANNEL:
					ret = this.imageTransformHelper.GetBlueChannel(img);
					break;
				case TRANSFORM_TYPE.GRAYSCALE:
					ret = this.imageTransformHelper.GetGrayscale(img);
					break;
				case TRANSFORM_TYPE.EDGE_DETECTION:
					ret = this.imageTransformHelper.GetEdgeDetection(img);
					break;
				case TRANSFORM_TYPE.NONE:
					ret = img;
					break;
				default:
					break;
			}

			//Convert the result into a type that can be transferred back to the client
			ImageConverter imgConv = new ImageConverter();
			byte[] fileData = (byte[])imgConv.ConvertTo(null, CultureInfo.CurrentCulture, ret, typeof(byte[]));
			return new FileContentResult(fileData, "img/bmp");
		}
	}
}