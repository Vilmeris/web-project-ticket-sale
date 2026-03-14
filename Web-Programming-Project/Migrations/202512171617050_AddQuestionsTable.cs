namespace Web_Programming_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Answer = c.String(),
                        AskerName = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Questions");
        }
    }
}
