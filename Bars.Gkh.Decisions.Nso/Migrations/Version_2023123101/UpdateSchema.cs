namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2023123101
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023123101")]
    [MigrationDependsOn(typeof(Version_2023123100.UpdateSchema))]
    //Является Version_2018062100 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("DEC_GOV_DECISION", "NPA_NAME", DbType.String);
            this.Database.AddColumn("DEC_GOV_DECISION", "NPA_DATE", DbType.DateTime);
            this.Database.AddColumn("DEC_GOV_DECISION", "NPA_NUMBER", DbType.String);
            this.Database.AddColumn("DEC_GOV_DECISION", "NPA_STATUS", DbType.Int32);
            this.Database.AddColumn("DEC_GOV_DECISION", "NPA_CANCELLATION_REASON", DbType.String);

            this.Database.AddRefColumn("DEC_GOV_DECISION", new RefColumn("TYPE_INFORMATION_NPA_ID", "DEC_GOV_DECISION_TYPE_INFORMATION_NPA", "GKH_DICT_TYPE_INFORMATION_NPA", "ID"));
            this.Database.AddRefColumn("DEC_GOV_DECISION", new RefColumn("TYPE_NPA_ID", "DEC_GOV_DECISION_TYPE_NPA", "GKH_DICT_TYPE_NPA", "ID"));
            this.Database.AddRefColumn("DEC_GOV_DECISION", new RefColumn("TYPE_NORMATIVE_ACT_ID", "DEC_GOV_DECISION_TYPE_NORMATIVE_ACT", "GKH_DICT_TYPE_NORMATIVE_ACT", "ID"));
            this.Database.AddRefColumn("DEC_GOV_DECISION", new RefColumn("NPA_CONTRAGENT_ID", "DEC_GOV_DECISION_NPA_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
            this.Database.AddRefColumn("DEC_GOV_DECISION", new RefColumn("NPA_FILE_ID", "DEC_GOV_DECISION_NPA_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("DEC_GOV_DECISION", "NPA_NAME");
            this.Database.RemoveColumn("DEC_GOV_DECISION", "NPA_DATE");
            this.Database.RemoveColumn("DEC_GOV_DECISION", "NPA_NUMBER");
            this.Database.RemoveColumn("DEC_GOV_DECISION", "NPA_STATUS");
            this.Database.RemoveColumn("DEC_GOV_DECISION", "NPA_CANCELLATION_REASON");

            this.Database.RemoveColumn("DEC_GOV_DECISION", "TYPE_INFORMATION_NPA_ID");
            this.Database.RemoveColumn("DEC_GOV_DECISION", "TYPE_NPA_ID");
            this.Database.RemoveColumn("DEC_GOV_DECISION", "TYPE_NORMATIVE_ACT_ID");
            this.Database.RemoveColumn("DEC_GOV_DECISION", "NPA_CONTRAGENT_ID");
            this.Database.RemoveColumn("DEC_GOV_DECISION", "NPA_FILE_ID");
        }
    }
}