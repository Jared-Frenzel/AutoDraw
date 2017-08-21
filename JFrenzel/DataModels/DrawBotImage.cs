namespace DataModels
{
	public class DrawBotImage : EFModel
	{
		public string Name { get; set; }

		public int Height { get; set; }
		public int Width { get; set; }

		public int[,] RedChannel { get; set; }
		public int[,] GreenChannel { get; set; }
		public int[,] BlueChannel { get; set; }

	}
}