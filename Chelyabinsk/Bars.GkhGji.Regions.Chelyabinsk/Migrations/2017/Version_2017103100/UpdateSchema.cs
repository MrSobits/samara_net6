namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017103100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017103100")]
    [MigrationDependsOn(typeof(Version_2017103000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddEntityTable("GJI_CH_LICENSE_REISSUANCE",
                    new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "GKH_MANORG_LIC_REISSUANCE_C", "GKH_CONTRAGENT", "ID"),
                    new RefColumn("LICENSE_ID", ColumnProperty.NotNull, "REISS_GKH_MANORG_LIC", "GKH_MANORG_LICENSE", "ID"),
                    new RefColumn("STATE_ID", ColumnProperty.Null, "GJI_CH_LICENSE_REISSUANCE", "B4_STATE", "ID"),
                    new Column("REISSUANCE_DATE", DbType.DateTime),
                    new Column("REG_NUMBER", DbType.String, 100),
                    new Column("REG_NUM", DbType.Int64, 22),
                    new Column("CONF_DUTY", DbType.String, 1000));

            Database.AddEntityTable("GJI_CH_LICENSE_REISSUANCE_PERSON",
                    new RefColumn("LIC_REISSUANCE_ID", ColumnProperty.NotNull, "PERSON_REISSUANCE_GJI_LR", "GJI_CH_LICENSE_REISSUANCE", "ID"),
                    new RefColumn("PERSON_ID", ColumnProperty.NotNull, "PERSON_REISSUANCE_PERSON_P", "GKH_PERSON", "ID"));

            Database.AddEntityTable("GJI_CH_LICENSE_REISSUANCE_PROVDOC",
                    new RefColumn("LIC_REISSUANCE_ID", ColumnProperty.NotNull, "REISSUANCE_PROVDOC_GJI_LR", "GJI_CH_LICENSE_REISSUANCE", "ID"),
                    new RefColumn("LIC_PROVDOC_ID", ColumnProperty.NotNull, "REISSUANCE_PROVDOC_PROVDOC_LP", "GKH_DICT_LIC_PROVDOC", "ID"),
                    new RefColumn("LIC_PROVDOC_FILE_ID", ColumnProperty.None, "REISSUANCE_PROVDOC_FILE_INFO", "B4_FILE_INFO", "ID"),
                    new Column("LIC_PROVDOC_NUMBER", DbType.String, 100),
                    new Column("LIC_PROVDOC_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_LICENSE_REISSUANCE_PROVDOC");
            Database.RemoveTable("GJI_CH_LICENSE_REISSUANCE_PERSON");
            Database.RemoveTable("GJI_CH_LICENSE_REISSUANCE");
        }
    }
}