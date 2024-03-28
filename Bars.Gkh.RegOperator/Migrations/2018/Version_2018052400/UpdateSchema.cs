namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018052400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;
    using System;

    [Migration("2018052400")]
   
    [MigrationDependsOn(typeof(Version_2018052300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.RemoveConstraint("REGOP_LAWSUIT_OWNER_INFO", "UNIQUE_PERSONAL_ACCOUNT_REGOP_LAWSUIT_OWNER_INFO");

            this.Database.AddUniqueConstraint("UNIQUE_PERSONAL_ACCOUNT_REGOP_LAWSUIT_OWNER_INFO",
                                                "REGOP_LAWSUIT_OWNER_INFO",
                                                "NAME",
                                                "PERSONAL_ACCOUNT_ID",
                                                "LAWSUIT_ID");
        }
        public override void Down()
        {
            //Не предназначено для отката, после расширения ограничения новые данные будут нарушать целостность старого ограничения
            throw new NotImplementedException();
        }
    }
}