namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018102600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2018102600")]
   
    [MigrationDependsOn(typeof(Version_2018100300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //Функция для поддержки удаления записей должников, которых не должно быть в реестре по каким-либо причинам
            //Создается пустой, поскольку для каждого региона она должна быть разной
            this.Database.ExecuteNonQuery(
                @"create or replace function debtor_cleanup()
                    returns void as
                    $$
                        begin
                        end;
                    $$
                    language plpgsql;");
        }
        public override void Down()
        {
            this.Database.ExecuteNonQuery("drop function debtor_cleanup;");
        }
    }
}