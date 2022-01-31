namespace SAPTCO.BILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckoutLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CHECKOUTID = c.Int(nullable: false),
                        STATUS = c.String(),
                        ACTION = c.String(),
                        REQUESTJSON = c.String(),
                        RESPONSEJSON = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.HyperTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(),
                        Bill = c.Binary(),
                        CreatedAt = c.DateTime(),
                        is_used = c.Int(),
                        DownloadUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MerchandTransactions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        STATUS = c.String(),
                        QUANTITY = c.Int(),
                        PRICE = c.Double(),
                        AMOUNT = c.Double(),
                        VATPERCENTAGE = c.Int(nullable: false),
                        VATAMOUNT = c.Double(nullable: false),
                        TOTALAMOUNT = c.Double(nullable: false),
                        TYPE = c.String(),
                        CREATEDON = c.DateTime(nullable: false),
                        CUSTOMERNAME = c.String(),
                        CUSTOMERPHONE = c.String(),
                        FROMPOINT = c.String(),
                        TOPOINT = c.String(),
                        ARRIVALTIME = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Paymints",
                c => new
                    {
                        IdPay = c.Int(nullable: false, identity: true),
                        NamePay = c.String(),
                    })
                .PrimaryKey(t => t.IdPay);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Cost = c.Int(),
                        ServiceId = c.Int(),
                        NameEn = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.ServiceId)
                .Index(t => t.ServiceId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        SubServiceId = c.Int(),
                        CustomerId = c.Int(),
                        CreatedAt = c.DateTime(),
                        IdTrip = c.Int(),
                        IdPay = c.Int(),
                        countTiket = c.Int(),
                        idTerminal = c.Int(),
                        idTime = c.Int(),
                        idTimeOut = c.Int(),
                        idtripOut = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.SubServices", t => t.SubServiceId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SubServiceId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(),
                        Bill1 = c.Binary(),
                        CreatedAt = c.DateTime(),
                        Is_used = c.Int(),
                        Transaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.Transaction_Id)
                .Index(t => t.Transaction_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Phone = c.String(),
                        NationalId = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Terminals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NameEn = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TicketPaymentPDFs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(),
                        Bill = c.Binary(),
                        CreatedAt = c.DateTime(),
                        is_used = c.Int(),
                        DownloadUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketID = c.Int(nullable: false, identity: true),
                        BillID = c.Int(),
                        CDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TicketID);
            
            CreateTable(
                "dbo.TimeTerminals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TerminalID = c.Int(),
                        TripID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        IdTrip = c.Int(nullable: false, identity: true),
                        NameTrip = c.String(),
                    })
                .PrimaryKey(t => t.IdTrip);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "UserId", "dbo.Users");
            DropForeignKey("dbo.Transactions", "SubServiceId", "dbo.SubServices");
            DropForeignKey("dbo.Transactions", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Bills", "Transaction_Id", "dbo.Transactions");
            DropForeignKey("dbo.SubServices", "ServiceId", "dbo.Services");
            DropIndex("dbo.Bills", new[] { "Transaction_Id" });
            DropIndex("dbo.Transactions", new[] { "CustomerId" });
            DropIndex("dbo.Transactions", new[] { "SubServiceId" });
            DropIndex("dbo.Transactions", new[] { "UserId" });
            DropIndex("dbo.SubServices", new[] { "ServiceId" });
            DropTable("dbo.Trips");
            DropTable("dbo.TimeTerminals");
            DropTable("dbo.Tickets");
            DropTable("dbo.TicketPaymentPDFs");
            DropTable("dbo.Terminals");
            DropTable("dbo.Users");
            DropTable("dbo.Customers");
            DropTable("dbo.Bills");
            DropTable("dbo.Transactions");
            DropTable("dbo.SubServices");
            DropTable("dbo.Services");
            DropTable("dbo.Paymints");
            DropTable("dbo.MerchandTransactions");
            DropTable("dbo.HyperTickets");
            DropTable("dbo.CheckoutLogs");
        }
    }
}
