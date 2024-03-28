namespace Bars.Gkh.Migrations._2023.Version_2023050135
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050135")]

    [MigrationDependsOn(typeof(Version_2023050134.UpdateSchema))]

    /// Является Version_2020091100 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT_CLW", new Column("DATE_FROM", DbType.DateTime, ColumnProperty.NotNull, "'01.01.2020'"));
            Database.AddColumn("GKH_CONTRAGENT_CLW", new Column("DATE_TO", DbType.DateTime, ColumnProperty.NotNull, "'31.12.2020'"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT_CLW", "DATE_TO");
            Database.RemoveColumn("GKH_CONTRAGENT_CLW", "DATE_FROM");
        }
    }
}