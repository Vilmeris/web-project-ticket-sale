namespace Web_Programming_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdToTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "UserId");
            AddForeignKey("dbo.Tickets", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "UserId", "dbo.Users");
            DropIndex("dbo.Tickets", new[] { "UserId" });
            DropColumn("dbo.Tickets", "UserId");
        }
    }
}
