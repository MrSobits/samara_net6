namespace Bars.GkhCr.Migrations._2015.Version_2015121000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations._2015.Version_2015111400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJECT", new Column("MAX_KPKR_AMOUNT", DbType.Decimal));
            Database.AddColumn("CR_OBJECT", new Column("FACT_AMOUNT_SPENT", DbType.Decimal));
            Database.AddColumn("CR_OBJECT", new Column("FACT_START_DATE", DbType.Date));
            Database.AddColumn("CR_OBJECT", new Column("FACT_END_DATE", DbType.Date));
            Database.AddColumn("CR_OBJECT", new Column("WARRANTY_END_DATE", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJECT", "MAX_KPKR_AMOUNT");
            Database.RemoveColumn("CR_OBJECT", "FACT_AMOUNT_SPENT");
            Database.RemoveColumn("CR_OBJECT", "FACT_START_DATE");
            Database.RemoveColumn("CR_OBJECT", "FACT_END_DATE");
            Database.RemoveColumn("CR_OBJECT", "WARRANTY_END_DATE");
        }
    }
}
