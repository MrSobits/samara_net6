namespace Bars.Gkh.Migrations._2023.Version_2023050141
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050141")]

    [MigrationDependsOn(typeof(Version_2023050140.UpdateSchema))]

    /// Является Version_2021081800 из ядра
    public class UpdateSchema : Migration
    {
        private readonly string tableName = "gkh_reality_object";

        public override void Up()
        {
            this.Database.AddColumn(this.tableName, new Column("is_included_reg_cho", DbType.Boolean));
            this.Database.AddColumn(this.tableName, new Column("is_included_list_ident_cho", DbType.Boolean));
            this.Database.AddColumn(this.tableName, new Column("is_determined_sub_protect_cho", DbType.Boolean));
        }

        public override void Down()
        {
            this.Database.RemoveColumn(this.tableName, "is_included_reg_cho");
            this.Database.RemoveColumn(this.tableName, "is_included_list_ident_cho");
            this.Database.RemoveColumn(this.tableName, "is_determined_sub_protect_cho");
        }
    }
}