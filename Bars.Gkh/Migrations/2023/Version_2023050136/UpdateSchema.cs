namespace Bars.Gkh.Migrations._2023.Version_2023050136
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050136")]

    [MigrationDependsOn(typeof(Version_2023050135.UpdateSchema))]

    /// Является Version_2020091101 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("gkh_reality_object", new Column("inn_man_orgs", DbType.String, 2000));

            var sql = @"
				update public.gkh_reality_object gro
				set inn_man_orgs = inns.column2
				from (select gmcr.reality_obj_id as column1, string_agg(inn, ', ') as column2
					  from public.gkh_morg_contract_realobj gmcr
					  inner join public.gkh_morg_contract gmc on gmc.id = gmcr.man_org_contract_id
					  inner join public.gkh_managing_organization gmo on gmo.id = gmc.manag_org_id
					  inner join public.gkh_contragent gc on gc.id = gmo.contragent_id
					  where gmc.start_date <= current_date and (gmc.end_date >= current_date or gmc.end_date is null)
					  group by gmcr.reality_obj_id) as inns
				where gro.id = inns.column1";
            Database.ExecuteNonQuery(sql);
        }

        public override void Down()
        {
            Database.RemoveColumn("gkh_reality_object", "inn_man_orgs");
        }
    }
}