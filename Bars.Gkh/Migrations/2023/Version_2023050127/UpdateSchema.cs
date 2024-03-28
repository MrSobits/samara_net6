namespace Bars.Gkh.Migrations._2023.Version_2023050127
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050127")]

    [MigrationDependsOn(typeof(Version_2023050126.UpdateSchema))]

    /// Является Version_2019121401 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            // Добавление к таблице Нормативно-правовые документы столбца с индификатором ТОР
            this.Database.AddColumn("GKH_DICT_NORMATIVE_DOC", new GuidColumn("TOR_ID", ColumnProperty.Null));
            this.Database.AddColumn("GKH_DICT_MUNICIPALITY", new GuidColumn("TOR_ID", ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_DICT_NORMATIVE_DOC", "TOR_ID");
            this.Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "TOR_ID");
        }
    }
}