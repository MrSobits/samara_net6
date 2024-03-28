namespace Bars.Gkh.Migrations._2022.Version_2022031700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022031700")]
    
    [MigrationDependsOn(typeof(_2022.Version_2022031000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_MANORG_LICENSE", new Column("ERUL_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GKH_MANORG_LICENSE", new Column("ERUL_NUMBER", DbType.String, 100));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("FLDOC_ISSUED_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("FLDOC_SERIES", DbType.String, 10));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("FLDOC_NUMBER", DbType.String, 10));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("ISSUEDBY", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "ISSUEDBY");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "FLDOC_NUMBER");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "FLDOC_SERIES");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "FLDOC_ISSUED_DATE");
            Database.RemoveColumn("GKH_MANORG_LICENSE", "ERUL_NUMBER");
            Database.RemoveColumn("GKH_MANORG_LICENSE", "ERUL_DATE");
        }
    }
}