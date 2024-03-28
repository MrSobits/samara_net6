using System.Data;
using Bars.B4.Modules.Ecm7.Framework;

namespace Bars.GkhGji.Migrations._2017.Version_2017090400
{
    [Migration("2017090400")]
    [MigrationDependsOn(typeof(Version_2017083000.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            var gjiAppealCitizens = new SchemaQualifiedObjectName {Schema = "public", Name = "gji_appeal_citizens"};
            var gjiAppcitExecutant = new SchemaQualifiedObjectName {Schema = "public", Name = "gji_appcit_executant"};

            this.Database.AddColumn(gjiAppealCitizens, "amount_pages", DbType.Int32);
            this.Database.AddColumn(gjiAppealCitizens, "gji_dict_citizenship_id", DbType.Int32);
            this.Database.AddColumn(gjiAppealCitizens, "declarant_mailing_address", DbType.String, 200);
            this.Database.AddColumn(gjiAppealCitizens, "declarant_work_place", DbType.String, 300);
            this.Database.AddColumn(gjiAppealCitizens, "declarant_sex", DbType.Int32);
            this.Database.AddColumn(gjiAppealCitizens, "appeal_status", DbType.Int32);
            this.Database.AddColumn(gjiAppealCitizens, "planned_exec_date", DbType.DateTime);
            this.Database.AddColumn(gjiAppealCitizens, "registrator_id", DbType.Int64);
            this.Database.AddColumn(gjiAppealCitizens, "appeal_uid", DbType.String, 36);

            if (this.Database.TableExists(gjiAppcitExecutant))
            {
                this.Database.AddColumn(gjiAppcitExecutant, "zonainsp_id", DbType.Int64);
            }
        }

        public override void Down()
        {
            var gjiAppealCitizens = new SchemaQualifiedObjectName { Schema = "public", Name = "gji_appeal_citizens" };
            var gjiAppcitExecutant = new SchemaQualifiedObjectName { Schema = "public", Name = "gji_appcit_executant" };

            this.RemoveColumnIfExists(gjiAppealCitizens, "amount_pages");
            this.RemoveColumnIfExists(gjiAppealCitizens, "gji_dict_citizenship_id");
            this.RemoveColumnIfExists(gjiAppealCitizens, "declarant_mailing_address");
            this.RemoveColumnIfExists(gjiAppealCitizens, "declarant_work_place");
            this.RemoveColumnIfExists(gjiAppealCitizens, "declarant_sex");
            this.RemoveColumnIfExists(gjiAppealCitizens, "appeal_status");
            this.RemoveColumnIfExists(gjiAppealCitizens, "planned_exec_date");
            this.RemoveColumnIfExists(gjiAppealCitizens, "registrator_id");
            this.RemoveColumnIfExists(gjiAppealCitizens, "appeal_uid");
            this.RemoveColumnIfExists(gjiAppcitExecutant, "zonainsp_id");
        }

        private void RemoveColumnIfExists(SchemaQualifiedObjectName table, string columnName)
        {
            if (this.Database.TableExists(table) && this.Database.ColumnExists(table, columnName))
            {
                this.Database.RemoveColumn(table, columnName);
            }
        }
    }
}
