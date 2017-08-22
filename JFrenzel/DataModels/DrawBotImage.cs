namespace DataModels
{
	public class DrawBotImage : EFModel
	{
		public string Name { get; set; }

		public int Height { get; set; }
		public int Width { get; set; }

		public string RedChannel { get; set; }
		public string GreenChannel { get; set; }
		public string BlueChannel { get; set; }

	}
}