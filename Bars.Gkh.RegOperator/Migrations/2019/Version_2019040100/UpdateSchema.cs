﻿namespace Bars.Gkh.RegOperator.Migrations._2019.Version_2019040100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019040100")]
   
    [MigrationDependsOn(typeof(Version_2019032500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
           Database.AddColumn("REGOP_WALLET", new Column("REPAYMENT", DbType.Boolean, ColumnProperty.None));

        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_WALLET", "REPAYMENT");
        
        }
    }
}