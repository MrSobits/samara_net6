namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014052500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014051600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_NSO_ACT_PROVDOC",
                new Column("DATE_PROVIDED", DbType.DateTime),
                new RefColumn("PROVDOC_ID", ColumnProperty.NotNull, "GJI_NSO_ACT_PROVDOC_P", "GJI_DICT_PROVIDEDDOCUMENT", "ID"),
                new RefColumn("ACT_ID", ColumnProperty.NotNull, "GJI_NSO_ACT_PROVDOC_A", "GJI_ACTCHECK", "ID"));

            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_NUM", DbType.String, 100));
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_DATE", DbType.DateTime));

            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_NUM_LETTER", DbType.String, 100));
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_DATE_LETTER", DbType.DateTime));

            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_OBTAINED", DbType.Int16, ColumnProperty.NotNull, 20));
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("NC_SENT", DbType.Int16, ColumnProperty.NotNull, 20));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_NSO_ACT_PROVDOC");

            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_NUM");
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_DATE");
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_NUM_LETTER");
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_DATE_LETTER");
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_OBTAINED");
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "NC_SENT");
        }
    }
}