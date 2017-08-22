namespace JFrenzel.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class DrawBotImage : DbMigration
	{
		public override void Up()
		{
			CreateTable(
					"dbo.DrawBotImages",
					c => new
					{
						Id = c.Int(nullable: false, identity: true),
						Name = c.String(),
						Height = c.Int(nullable: false),
						Width = c.Int(nullable: false),
						RedChannel = c.String(),
						GreenChannel = c.String(),
						BlueChannel = c.String(),
					})
					.PrimaryKey(t => t.Id);

		}

		public override void Down()
		{
			DropTable("dbo.DrawBotImages");
		}
	}
}
