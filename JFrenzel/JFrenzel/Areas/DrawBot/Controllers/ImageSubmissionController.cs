using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Web.Mvc;

using JFrenzel.Areas.DrawBot.Models;

namespace JFrenzel.Areas.DrawBot.Controllers
{
    public class ImageSubmissionController : Controller
    {
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
					//Save the img on the local file system
					string path = Path.Combine(Server.MapPath("~/Images"),
																			Path.GetFileName(file.FileName));
					file.SaveAs(path);

					//Create a asp image from the file, and convert it to the DrawBot image format. Store in DB as well
					Bitmap img = new Bitmap(Image.FromFile(path));

					DrawBotImage dbImage = new DrawBotImage();
					dbImage.Height = img.Height;
					dbImage.Width = img.Width;

					dbImage.RedChannel = new int[dbImage.Width, dbImage.Height];
					dbImage.GreenChannel = new int[dbImage.Width, dbImage.Height];
					dbImage.BlueChannel = new int[dbImage.Width, dbImage.Height];

					for (int x = 0; x < dbImage.Width; x++)
					{
						for (int y = 0; y < dbImage.Height; y++)
						{
							Color px = img.GetPixel(x, y);

							dbImage.RedChannel[x, y] = px.R;
							dbImage.BlueChannel[x, y] = px.B;
							dbImage.GreenChannel[x, y] = px.G;
						}
					}

					ViewBag.Message = "File uploaded successfully";
				}
				catch(OutOfMemoryException ex)
				{
					ViewBag.Message = "Error: You uploaded an unsupported file type. Please uplad a BMP, GIF, JPEG, PNG, or TIFF file.";
				}
				catch (Exception ex)
				{
					ViewBag.Message = "ERROR:" + ex.Message.ToString();
				}
			else
			{
				ViewBag.Message = "You have not specified a file.";
			}

			return View();
		}
    }
}