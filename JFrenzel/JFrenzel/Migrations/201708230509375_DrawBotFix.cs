namespace JFrenzel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DrawBotFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrawBotImages", "ImagePath", c => c.String());
            AddColumn("dbo.DrawBotImages", "OwnerUsername", c => c.String());
            AddColumn("dbo.DrawBotImages", "Timestamp", c => c.DateTime(nullable: false));
            DropColumn("dbo.DrawBotImages", "Height");
            DropColumn("dbo.DrawBotImages", "Width");
            DropColumn("dbo.DrawBotImages", "RedChannel");
            DropColumn("dbo.DrawBotImages", "GreenChannel");
            DropColumn("dbo.DrawBotImages", "BlueChannel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DrawBotImages", "BlueChannel", c => c.String());
            AddColumn("dbo.DrawBotImages", "GreenChannel", c => c.String());
            AddColumn("dbo.DrawBotImages", "RedChannel", c => c.String());
            AddColumn("dbo.DrawBotImages", "Width", c => c.Int(nullable: false));
            AddColumn("dbo.DrawBotImages", "Height", c => c.Int(nullable: false));
            DropColumn("dbo.DrawBotImages", "Timestamp");
            DropColumn("dbo.DrawBotImages", "OwnerUsername");
            DropColumn("dbo.DrawBotImages", "ImagePath");
        }
    }
}
