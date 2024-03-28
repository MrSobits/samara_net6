namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018072600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2018072600")]
   
    [MigrationDependsOn(typeof(Version_2018071600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            DropConstraint();
        }
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public void DropConstraint()
        {
            Database.ExecuteNonQuery(@"alter table regop_lawsuit_owner_info drop constraint if exists unique_personal_account_regop_lawsuit_owner_info");
        }
    }
}