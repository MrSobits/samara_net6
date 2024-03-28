namespace Bars.GkhGji.Migrations._2020.Version_2020030300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020030300")]
    [MigrationDependsOn(typeof(Version_2020022500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
          
       
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("INCOMING_SOURCES_NAMES", DbType.String, 250));
            UpdateSourceNames();
        }

        public override void Down()
        {           
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "INCOMING_SOURCES_NAMES");           
        }

        private void UpdateSourceNames()
        {
            var sql = @"drop table if exists appcitincsources;
                create temp table appcitincsources as
                select gac.id, string_agg(gdr.name, ';') ros, count(gas.id) cntid from GJI_APPEAL_SOURCES gas
                inner join gji_dict_revenuesource gdr on gdr.id = gas.revenue_source_id
                inner join gji_appeal_citizens gac on gac.id = appcit_id where (gac.INCOMING_SOURCES_NAMES is null or gac.INCOMING_SOURCES_NAMES = '')
                group by 1;
                update gji_appeal_citizens set INCOMING_SOURCES_NAMES = appcitincsources.ros
                from appcitincsources where gji_appeal_citizens.id = appcitincsources.id and length(appcitincsources.ros) <= 250;";

            this.Database.ExecuteNonQuery(sql);

        }
    }
}