using System;
using System.IO;

namespace DataModels
{
	public class DrawBotImage : EFModel
	{
		public enum TRANSFORM_TYPE
		{
			NONE,
			RED_CHANNEL,
			GREEN_CHANNEL,
			BLUE_CHANNEL,
			GRAYSCALE,
			EDGE_DETECTION
		}

		public DrawBotImage() { }

		public DrawBotImage(string OwnerUsername, DateTime Timestamp)
		{
			this.OwnerUsername = OwnerUsername == "" ? "anon" : OwnerUsername;
			this.Timestamp = Timestamp;

			//Generate the filepath based off the username and timestamp
			this.ImagePath = Path.Combine(
				this.Timestamp.Year.ToString(),
				this.Timestamp.Month.ToString(),
				this.Timestamp.Day.ToString(),
				this.OwnerUsername + "_" + (this.Timestamp.Ticks / TimeSpan.TicksPerMillisecond).ToString() + ".bmp"
				);
		}

		public string ImagePath { get; set; }
		public string Name { get; set; }
		public string OwnerUsername { get; set; }
		public DateTime Timestamp { get; set; }
	}
}