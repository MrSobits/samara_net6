namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018011100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2018011100")]
    [MigrationDependsOn(typeof(Migrations._2017.Version_2017122600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddRefColumn("REGOP_CALC_DEBT_DETAIL", new RefColumn("ACCOUNT_ID", "CALC_DEBT_DETAIL_ACCOUNT_ID", "REGOP_PERS_ACC", "ID"));
            this.Database.AddColumn("REGOP_CALC_DEBT_DETAIL", "IS_IMPORTED", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_CALC_DEBT_DETAIL", "ACCOUNT_ID");
            this.Database.RemoveColumn("REGOP_CALC_DEBT_DETAIL", "IS_IMPORTED");
        }
    }
}