namespace Bars.Gkh.Reforma.Migrations.Version_2014112800
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Reforma.Migrations.Version_2014112600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "RFRM_REALITY_OBJECT",
                new Column("EXTERNAL_ID", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "RFRM_REAL_OBJ", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("RFRM_REALITY_OBJECT");
        }
    }
}