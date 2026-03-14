namespace Web_Programming_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWalletAndCards : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        CardId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CardNumber = c.String(nullable: false, maxLength: 16),
                        CardHolderName = c.String(nullable: false),
                        ExpiryDate = c.String(nullable: false),
                        CVV = c.String(nullable: false),
                        CardBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CardId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Users", "WalletBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditCards", "UserId", "dbo.Users");
            DropIndex("dbo.CreditCards", new[] { "UserId" });
            DropColumn("dbo.Users", "WalletBalance");
            DropTable("dbo.CreditCards");
        }
    }
}
