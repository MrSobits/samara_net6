namespace Bars.Gkh.Migrations._2023.Version_2023050133
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Utils;

    [Migration("2023050133")]

    [MigrationDependsOn(typeof(Version_2023050132.UpdateSchema))]

    /// Является Version_2020072400 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT", new Column("DEBT_START_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("CLW_LAWSUIT", new Column("DEBT_END_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("CLW_LAWSUIT", new Column("DEBT_CALC_METHOD", DbType.Int32, ColumnProperty.None, (int)DebtCalcMethod.None));
            Database.AddColumn("CLW_LAWSUIT", new Column("DESCRIPTION", DbType.String, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT", "DESCRIPTION");
            Database.RemoveColumn("CLW_LAWSUIT", "DEBT_CALC_METHOD");
            Database.RemoveColumn("CLW_LAWSUIT", "DEBT_END_DATE");
            Database.RemoveColumn("CLW_LAWSUIT", "DEBT_START_DATE");
        }
    }
}