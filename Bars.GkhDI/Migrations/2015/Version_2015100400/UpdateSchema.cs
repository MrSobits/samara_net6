namespace Bars.GkhDi.Migrations._2015.Version_2015100400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015100101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        
        public override void Up()
        {
            Database.AddColumn("DI_ADMIN_RESP", new Column("TYPE_PERSON", DbType.Int16, ColumnProperty.NotNull, 10));
            Database.AddColumn("DI_ADMIN_RESP", new Column("FIO", DbType.String, 300, ColumnProperty.Null));
            Database.AddColumn("DI_ADMIN_RESP", new Column("PSITION", DbType.String, 300, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_ADMIN_RESP", "TYPE_PERSON");
            Database.RemoveColumn("DI_ADMIN_RESP", "FIO");
            Database.RemoveColumn("DI_ADMIN_RESP", "PSITION");
        }
    }
}