using System.Data;
using Bars.B4.Modules.Ecm7.Framework;

namespace Bars.GkhGji.Migrations._2017.Version_2017111000
{
    [Migration("2017111000")]
    [MigrationDependsOn(typeof(Version_2017101900.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        private readonly string gjiAppealCitizens = "GJI_APPEAL_CITIZENS";
        private readonly string gjiAppcitExecutant = "GJI_APPCIT_EXECUTANT";

        public override void Up()
        {
            this.Database.AddColumn(this.gjiAppealCitizens, "amount_pages", DbType.Int32);
            this.Database.AddColumn(this.gjiAppealCitizens, "gji_dict_citizenship_id", DbType.Int32);
            this.Database.AddColumn(this.gjiAppealCitizens, "declarant_mailing_address", DbType.String, 200);
            this.Database.AddColumn(this.gjiAppealCitizens, "declarant_work_place", DbType.String, 300);
            this.Database.AddColumn(this.gjiAppealCitizens, "declarant_sex", DbType.Int32);
            this.Database.AddColumn(this.gjiAppealCitizens, "appeal_status", DbType.Int32);
            this.Database.AddColumn(this.gjiAppealCitizens, "planned_exec_date", DbType.DateTime);
            this.Database.AddColumn(this.gjiAppealCitizens, "registrator_id", DbType.Int64);
            this.Database.AddColumn(this.gjiAppealCitizens, "appeal_uid", DbType.String, 36);

            if (this.Database.TableExists(this.gjiAppcitExecutant))
            {
                this.Database.AddColumn(this.gjiAppcitExecutant, "zonainsp_id", DbType.Int64);
            }
        }

        public override void Down()
        {
            this.Database.RemoveColumn(this.gjiAppealCitizens, "amount_pages");
            this.Database.RemoveColumn(this.gjiAppealCitizens, "gji_dict_citizenship_id");
            this.Database.RemoveColumn(this.gjiAppealCitizens, "declarant_mailing_address");
            this.Database.RemoveColumn(this.gjiAppealCitizens, "declarant_work_place");
            this.Database.RemoveColumn(this.gjiAppealCitizens, "declarant_sex");
            this.Database.RemoveColumn(this.gjiAppealCitizens, "appeal_status");
            this.Database.RemoveColumn(this.gjiAppealCitizens, "planned_exec_date");
            this.Database.RemoveColumn(this.gjiAppealCitizens, "registrator_id");
            this.Database.RemoveColumn(this.gjiAppealCitizens, "appeal_uid");

            if (this.Database.TableExists(this.gjiAppcitExecutant) && this.Database.ColumnExists(this.gjiAppcitExecutant, "zonainsp_id"))
            {
                this.Database.RemoveColumn(this.gjiAppcitExecutant, "zonainsp_id");
            }
            
        }
    }
}
