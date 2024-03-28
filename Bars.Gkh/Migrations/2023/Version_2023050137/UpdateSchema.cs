namespace Bars.Gkh.Migrations._2023.Version_2023050137
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050137")]

    [MigrationDependsOn(typeof(Version_2023050136.UpdateSchema))]

    /// Является Version_2020100100 из ядра
    public class UpdateSchema : Migration
    {
        private const string TableName = "gkh_reality_object";

        public override void Up()
        {
            Database.AddColumn(TableName, new Column("start_control_date", DbType.String, 2000));

            var sql = $@"
				update {TableName} gro
				set start_control_date = startControlDates.column2
				from (select gmcr.reality_obj_id as column1, string_agg(to_char(gmc.START_DATE, 'DD.MM.YYYY'), ', ') as column2
					  from public.gkh_morg_contract_realobj gmcr
					  inner join public.gkh_morg_contract gmc on gmc.id = gmcr.man_org_contract_id
					  inner join public.gkh_managing_organization gmo on gmo.id = gmc.manag_org_id
					  inner join public.gkh_contragent gc on gc.id = gmo.contragent_id
					  where gmc.start_date <= current_date and (gmc.end_date >= current_date or gmc.end_date is null)
					  group by gmcr.reality_obj_id) as startControlDates
				where gro.id = startControlDates.column1";

            Database.ExecuteNonQuery(sql);
        }

        public override void Down()
        {
            Database.RemoveColumn(TableName, "start_control_date");
        }
    }
}