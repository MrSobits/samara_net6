namespace Bars.Gkh.Migrations._2023.Version_2023050143
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050143")]

    [MigrationDependsOn(typeof(Version_2023050142.UpdateSchema))]

    /// Является Version_2021122200 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GKH_DICT_NORMATIVE_DOC", new Column("DATE_FROM", DbType.DateTime));
            this.Database.AddColumn("GKH_DICT_NORMATIVE_DOC", new Column("DATE_TO", DbType.DateTime));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_DICT_NORMATIVE_DOC", "DATE_FROM");
            this.Database.RemoveColumn("GKH_DICT_NORMATIVE_DOC", "DATE_TO");
        }
    }
}