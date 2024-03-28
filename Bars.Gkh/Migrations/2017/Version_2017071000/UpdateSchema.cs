namespace Bars.Gkh.Migrations._2017.Version_2017071000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2017071000")]
    [MigrationDependsOn(typeof(Version_2017070600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (!this.Database.TableExists("DI_DICT_TEMPL_SERVICE"))
            {
                this.Database.AddEntityTable(
                    "DI_DICT_TEMPL_SERVICE",
                    new RefColumn("UNIT_MEASURE_ID", "DI_TEM_SER_UM", "GKH_DICT_UNITMEASURE", "ID"),
                    new Column("NAME", DbType.String, 300),
                    new Column("CODE", DbType.String, 300),
                    new Column("CHARACTERISTIC", DbType.String, 300),
                    new Column("CHANGEABLE", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("IS_MANDATORY", DbType.Boolean, ColumnProperty.NotNull, false),
                    new Column("TYPE_GROUP_SERVICE", DbType.Int32, ColumnProperty.NotNull, 10),
                    new Column("KIND_SERVICE", DbType.Int32, ColumnProperty.NotNull, 10),
                    new Column("EXTERNAL_ID", DbType.String, 36));

                this.Database.AddIndex("IND_DI_TEM_SER_N", false, "DI_DICT_TEMPL_SERVICE", "NAME");
            }

            this.Database.AddExportId("DI_DICT_TEMPL_SERVICE", FormatDataExportSequences.DictUslugaExportId);

        }

        public override void Down()
        {
            this.Database.RemoveExportId("DI_DICT_TEMPL_SERVICE");
        }
    }
}