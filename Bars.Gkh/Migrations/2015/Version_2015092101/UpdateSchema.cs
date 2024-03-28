namespace Bars.Gkh.Migrations._2015.Version_2015092101
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015091800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        
        public override void Up()
        {
                Database.AddColumn("GKH_CONTRAGENT", new Column("EGRUL_EXC_NUMBER", DbType.String, 100, ColumnProperty.Null, "''"));
                Database.AddColumn("GKH_CONTRAGENT", new Column("EGRUL_EXC_DATE", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT", "EGRUL_EXC_NUMBER");
            Database.RemoveColumn("GKH_CONTRAGENT", "EGRUL_EXC_DATE");
            
        }
    }
}