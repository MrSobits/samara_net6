namespace Bars.Gkh.Migrations._2023.Version_2023050114
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050114")]

    [MigrationDependsOn(typeof(Version_2023050113.UpdateSchema))]

    /// Является Version_2018092100 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_DICT_WORK", new Column("GIS_CODE", DbType.String));
            this.Database.AddColumn("OVRHL_COMMON_ESTATE_OBJECT", new Column("GIS_CODE", DbType.String));

            this.Database.AddSequence("GKH_WORKKPRTYPE_EXPORT_ID_SEQ");
            this.Database.AddExportId("GKH_DICT_WORK", "GKH_WORKKPRTYPE_EXPORT_ID_SEQ");
            this.Database.AddExportId("OVRHL_COMMON_ESTATE_OBJECT", "GKH_WORKKPRTYPE_EXPORT_ID_SEQ");
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_DICT_WORK", "GIS_CODE");
            this.Database.RemoveColumn("OVRHL_COMMON_ESTATE_OBJECT", "GIS_CODE");
            this.Database.RemoveExportId("GKH_DICT_WORK");
            this.Database.RemoveExportId("OVRHL_COMMON_ESTATE_OBJECT");
        }
    }
}