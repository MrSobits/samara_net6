namespace Bars.Gkh.Migrations._2023.Version_2023050129
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050129")]

    [MigrationDependsOn(typeof(Version_2023050128.UpdateSchema))]

    /// Является Version_2020021100 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("B4_FIAS_ADDRESS", new Column("HOUSING", DbType.String, 20));
        }

        public override void Down()
        {
            this.Database.ChangeColumn("B4_FIAS_ADDRESS", new Column("HOUSING", DbType.String, 10));
        }
    }
}