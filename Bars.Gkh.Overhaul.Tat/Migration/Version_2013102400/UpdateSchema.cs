namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013102400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013102301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Database.AddEntityTable(
            //    "OVRHL_REALESTTYPE_PRIORITY",
            //    new Column("CODE", DbType.String, 300, ColumnProperty.NotNull),
            //    new Column("WEIGHT", DbType.Int16, ColumnProperty.NotNull),
            //    new RefColumn("REAL_ESTATE_TYPE_ID", "OVRHL_REALESTPROIR_REALEST", "OVRHL_REAL_ESTATE_TYPE", "ID"));
        }

        public override void Down()
        {
            //Database.RemoveEntityTable("OVRHL_REALESTTYPE_PRIORITY");
        }
    }
}