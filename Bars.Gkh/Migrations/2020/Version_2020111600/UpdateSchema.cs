namespace Bars.Gkh.Migrations._2020.Version_2020111600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2020111600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2020111100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_CS_CATEGORY_TYPE",
                new Column("NAME", DbType.String, ColumnProperty.NotNull, 500),
                new Column("CODE", DbType.String, 20, ColumnProperty.NotNull));

            Database.AddEntityTable(
             "GKH_CS_CATEGORY",
             new Column("NAME", DbType.String, ColumnProperty.NotNull, 500),
             new Column("CODE", DbType.String, ColumnProperty.NotNull, 20),
             new RefColumn("TYPE_ID", ColumnProperty.None, "GKH_CS_CATEGORY_TYPE_ID", "GKH_CS_CATEGORY_TYPE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_CS_CATEGORY");
            Database.RemoveTable("GKH_CS_CATEGORY_TYPE");
        }
    }
}