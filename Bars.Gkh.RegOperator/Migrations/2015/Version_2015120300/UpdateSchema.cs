namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015120300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015.12.03.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015120300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015112600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_DEBTOR", new Column("COURT_TYPE", DbType.Int32, ColumnProperty.NotNull, 0));
            this.Database.AddRefColumn("REGOP_DEBTOR", new RefColumn("JUR_INST_ID", "REGOP_DEBTOR_JUR_INS", "CLW_JUR_INSTITUTION", "ID"));
        }

        /// <summary>
        /// Вверх
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_DEBTOR", "COURT_TYPE");
            this.Database.RemoveColumn("REGOP_DEBTOR", "JUR_INST_ID");
        }
    }
}
