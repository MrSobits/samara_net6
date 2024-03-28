namespace Bars.GkhGji.Migrations.Version_2013062801
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013062801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013062000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION_DISPUTE", "PROSECUTION_PROTEST", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION_DISPUTE", "PROSECUTION_PROTEST");
        }
    }
}