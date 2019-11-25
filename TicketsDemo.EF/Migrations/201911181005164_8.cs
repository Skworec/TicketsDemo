namespace TicketsDemo.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OverPrice = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            Sql("INSERT into dbo.BookingCompany(Name, OverPrice)" +
                "VALUES('First', 0.2)");
            AddColumn("dbo.Train", "BookingId", c => c.Int(nullable: true));
            Sql("UPDATE dbo.Train " +
                "SET BookingId = 1"
                );
            AlterColumn("dbo.Train", "BookingId", c => c.Int(nullable: false));
            CreateIndex("dbo.Train", "BookingId");
            AddForeignKey("dbo.Train", "BookingId", "dbo.BookingCompany", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Train", "BookingId", "dbo.BookingCompany");
            DropIndex("dbo.Train", new[] { "BookingId" });
            DropColumn("dbo.Train", "BookingId");
            DropTable("dbo.BookingCompany");
        }
    }
}
