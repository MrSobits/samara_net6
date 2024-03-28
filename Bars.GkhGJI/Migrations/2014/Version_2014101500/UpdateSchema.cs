namespace Bars.GkhGji.Migrations._2014.Version_2014101500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.MigrationsVersion_2014100300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_DICT_DECISMAKEAUTH",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));
            Database.AddIndex("IND_DECISMAKEAUTH_CODE", false, "GJI_DICT_DECISMAKEAUTH", "CODE");
            Database.AddIndex("IND_DECISMAKEAUTH_NAME", false, "GJI_DICT_DECISMAKEAUTH", "NAME");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_DECISMAKEAUTH");
        }
    }
}
