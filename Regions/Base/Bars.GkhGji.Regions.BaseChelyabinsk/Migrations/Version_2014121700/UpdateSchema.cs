namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014121700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014121604.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_NSO_DISP_FACT_VIOL",
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "GJI_NSO_DISP_FV_D", "GJI_DISPOSAL", "ID"),
                new RefColumn("FACT_VIOL_ID", ColumnProperty.NotNull, "GJI_NSO_DISP_FV_FV", "GJI_DICT_TYPE_FACT_VIOL", "ID"));

            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("PROSECUTOR_DEC_NUM", DbType.String, 300));
            this.Database.AddColumn("GJI_NSO_DISPOSAL", new Column("PROSECUTOR_DEC_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "PROSECUTOR_DEC_NUM");
            this.Database.RemoveColumn("GJI_NSO_DISPOSAL", "PROSECUTOR_DEC_DATE");
            this.Database.RemoveTable("GJI_NSO_DISP_FACT_VIOL");
        }
    }
}