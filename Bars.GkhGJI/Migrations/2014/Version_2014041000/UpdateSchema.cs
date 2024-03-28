namespace Bars.GkhGji.Migrations.Version_2014041000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014040802.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_DICT_SOC_ST",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300));
            Database.AddIndex("IND_GJI_DICT_SOC_ST_NAME", false, "GJI_DICT_SOC_ST", "NAME");
            Database.AddIndex("IND_GJI_DICT_SOC_ST_CODE", false, "GJI_DICT_SOC_ST", "CODE");

            Database.AddRefColumn("GJI_APPEAL_CITIZENS", new RefColumn("SOCIAL_ST_ID", "GJI_APPEAL_CITIZENS_SOC_ST", "GJI_DICT_SOC_ST", "ID"));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "SOCIAL_ST_ID");

            Database.RemoveTable("GJI_DICT_SOC_ST");
        }
    }
}