namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2014052500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014051600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_NSO_ACT_PROVDOC",
                new Column("DATE_PROVIDED", DbType.DateTime),
                new RefColumn("PROVDOC_ID", ColumnProperty.NotNull, "GJI_NSO_ACT_PROVDOC_P", "GJI_DICT_PROVIDEDDOCUMENT", "ID"),
                new RefColumn("ACT_ID", ColumnProperty.NotNull, "GJI_NSO_ACT_PROVDOC_A", "GJI_ACTCHECK", "ID"));

            Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_NUM", DbType.String, 100));
            Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_DATE", DbType.DateTime));

            Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_NUM_LETTER", DbType.String, 100));
            Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_DATE_LETTER", DbType.DateTime));

            Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_OBTAINED", DbType.Int16, ColumnProperty.NotNull, 20));
            Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_SENT", DbType.Int16, ColumnProperty.NotNull, 20));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_NSO_ACT_PROVDOC");

            Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_NUM");
            Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_DATE");
            Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_NUM_LETTER");
            Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_DATE_LETTER");
            Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_OBTAINED");
            Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_SENT");
        }
    }
}