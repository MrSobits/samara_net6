﻿namespace Bars.Gkh.RegOperator.Migrations._2020.Version_2020052800
{
    using System.Linq;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Dapper;

    [Migration("2020052800")]
   
    [MigrationDependsOn(typeof(_2020.Version_2020051900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            var container = ApplicationContext.Current.Container;
            this.Database.AddRefColumn("REGOP_PERS_ACC_OWNER", 
                new RefColumn("FACT_ADDR_DOC", ColumnProperty.Null, "FK_FILE_DOC_FACT_ADDR_OWNER", "B4_FILE_INFO", "ID"));
            
            var statelessSession = container.Resolve<ISessionProvider>().OpenStatelessSession();
            var connection = statelessSession.Connection;
            var sql = @"update  REGOP_PERS_ACC_OWNER RPAO 
                        set RPAO.FACT_ADDR_DOC = RIAO.FACT_ADDR_DOC 
                        from REGOP_INDIVIDUAL_ACC_OWN RIAO
                        where PIAO.ID = RPAO.ID";
           // var res = connection.Query<long>(sql).FirstOrDefault();
            this.Database.RemoveConstraint("REGOP_INDIVIDUAL_ACC_OWN", "FK_FILE_DOC_FACT_ADDR");
            this.Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "FACT_ADDR_DOC");

        }

        public override void Down()
        {
            var container = ApplicationContext.Current.Container;
            this.Database.AddRefColumn("REGOP_INDIVIDUAL_ACC_OWN", 
                new RefColumn("FACT_ADDR_DOC", ColumnProperty.Null, "FK_FILE_DOC_FACT_ADDR", "B4_FILE_INFO", "ID"));
            
            var statelessSession = container.Resolve<ISessionProvider>().OpenStatelessSession();
            var connection = statelessSession.Connection;
            var sql = @"update  REGOP_INDIVIDUAL_ACC_OWN RIAO 
                        set RIAO.FACT_ADDR_DOC = RPAO.FACT_ADDR_DOC 
                        from REGOP_PERS_ACC_OWNER RPAO
                        where PIAO.ID = RPAO.ID";
           // var res = connection.Query<long>(sql).FirstOrDefault();
            this.Database.RemoveConstraint("REGOP_PERS_ACC_OWNER", "FK_FILE_DOC_FACT_ADDR_OWNER");
            this.Database.RemoveColumn("REGOP_PERS_ACC_OWNER", "FACT_ADDR_DOC"); 
        }
    }
}