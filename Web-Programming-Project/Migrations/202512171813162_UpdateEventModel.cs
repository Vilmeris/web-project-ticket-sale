namespace Web_Programming_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEventModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreditCards", "CardLimit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.CreditCards", "CardBalance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CreditCards", "CardBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.CreditCards", "CardLimit");
        }
    }
}
