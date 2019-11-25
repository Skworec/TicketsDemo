//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.Entity.Migrations;
//using TicketsDemo.Data.Entities;

//namespace TicketsDemo.EF.Migrations
//{
//    class BookingContext : DbMigration
//    {
//        public override void Up()
//        {
//            CreateTable(
//                "dbo.BookingCompany",
//                c => new
//                {
//                    Id = c.Int(nullable: false, identity: true),
//                    Name = c.String(nullable: false),
//                    Overprice = c.Double(nullable: false),
//                    TrainId = c.Int(nullable: false),
//                })
//                .PrimaryKey(t => t.Id)
//                .ForeignKey("dbo.Train", t => t.TrainId, cascadeDelete: true)
//                .Index(t => t.TrainId);
//        }

//        public override void Down()
//        {
//            DropTable("dbo.BookingCompanies");
//        }
//    }
//}

