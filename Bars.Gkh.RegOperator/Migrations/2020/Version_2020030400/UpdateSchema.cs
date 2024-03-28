namespace Bars.Gkh.RegOperator.Migrations._2020.Version_2020030400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020030400")]
   
    [MigrationDependsOn(typeof(_2019.Version_2019121700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC", new Column("IS_NOT_DEBTOR", DbType.Boolean,false));
            Database.AddColumn("REGOP_PERS_ACC", new Column("INSTALLMENTPLAN", DbType.Boolean,false));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "IS_NOT_DEBTOR");
            Database.RemoveColumn("REGOP_PERS_ACC", "INSTALLMENTPLAN");
        }
    }
}