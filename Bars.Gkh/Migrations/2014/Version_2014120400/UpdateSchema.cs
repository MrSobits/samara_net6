namespace Bars.Gkh.Migrations.Version_2014120400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014112701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_DICT_LIC_PROVDOC",
                    new Column("CODE", DbType.String, 300),
                    new Column("NAME", DbType.String, 2000, ColumnProperty.NotNull));

            Database.AddEntityTable("GKH_MANORG_LIC_REQUEST",
                    new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "GKH_MANORG_LIC_REQUEST_C", "GKH_CONTRAGENT", "ID"),
                    new RefColumn("FILE_ID", ColumnProperty.Null, "GKH_MANORG_LIC_REQUEST_F", "B4_FILE_INFO", "ID"),
                    new RefColumn("STATE_ID", ColumnProperty.Null, "GKH_MANORG_LIC_REQUEST_S", "B4_STATE", "ID"),
                    new Column("DATE_REQUEST", DbType.DateTime),
                    new Column("REG_NUMBER", DbType.String, 100),
                    new Column("REG_NUM", DbType.Int64, 22),
                    new Column("CONF_DUTY", DbType.String, 1000),
                    new Column("REASON_OFFERS", DbType.String, 1000),
                    new Column("REASON_REFUSAL", DbType.String, 1000));

            Database.AddEntityTable("GKH_MANORG_REQ_PERSON",
                    new RefColumn("LIC_REQUEST_ID", ColumnProperty.NotNull, "GKH_MANORG_REQ_PERSON_LR", "GKH_MANORG_LIC_REQUEST", "ID"),
                    new RefColumn("PERSON_ID", ColumnProperty.NotNull, "GKH_MANORG_REQ_PERSON_P", "GKH_PERSON", "ID"));

            Database.AddEntityTable("GKH_MANORG_REQ_PROVDOC",
                    new RefColumn("LIC_REQUEST_ID", ColumnProperty.NotNull, "GKH_MANORG_REQ_PROVDOC_LR", "GKH_MANORG_LIC_REQUEST", "ID"),
                    new RefColumn("LIC_PROVDOC_ID", ColumnProperty.NotNull, "GKH_MANORG_REQ_PROVDOC_LP", "GKH_DICT_LIC_PROVDOC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_MANORG_REQ_PERSON");
            Database.RemoveTable("GKH_MANORG_REQ_PROVDOC");
            Database.RemoveTable("GKH_MANORG_LIC_REQUEST");
            Database.RemoveTable("GKH_DICT_LIC_PROVDOC");
        }
    }
}