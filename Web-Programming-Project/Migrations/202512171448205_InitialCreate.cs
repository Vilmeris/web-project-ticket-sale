namespace Web_Programming_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        ZoneName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Capacity = c.Int(nullable: false),
                        EventPrice_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.EventPrices", t => t.EventPrice_Id)
                .Index(t => t.EventId)
                .Index(t => t.EventPrice_Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Artist = c.String(nullable: false),
                        Description = c.String(),
                        Category = c.String(nullable: false),
                        ImageUrl = c.String(),
                        Date = c.DateTime(nullable: false),
                        City = c.String(nullable: false),
                        Venue = c.String(nullable: false),
                        Capacity = c.Int(nullable: false),
                        SoldTicketCount = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SubCategory = c.String(),
                        DiscountRate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        SeatNumber = c.String(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                        PricePaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TicketId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Phone = c.String(),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventPrices", "EventPrice_Id", "dbo.EventPrices");
            DropForeignKey("dbo.EventPrices", "EventId", "dbo.Events");
            DropIndex("dbo.Tickets", new[] { "EventId" });
            DropIndex("dbo.EventPrices", new[] { "EventPrice_Id" });
            DropIndex("dbo.EventPrices", new[] { "EventId" });
            DropTable("dbo.Users");
            DropTable("dbo.Tickets");
            DropTable("dbo.Events");
            DropTable("dbo.EventPrices");
        }
    }
}
