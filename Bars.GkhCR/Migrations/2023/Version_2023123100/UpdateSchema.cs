using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh.Enums;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023123100
{
    [Migration("2023123100")]
    [MigrationDependsOn(typeof(Version_2023082100.UpdateSchema))]
    // Является Version_2018060400 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("CR_DICT_PROGRAM",
                new RefColumn("GOV_CUSTOMER_ID", "CR_DICT_PROGRAM_GOV_CUSTOMER", "GKH_CONTRAGENT", "ID"));

            this.Database.AddColumn("CR_OBJ_BUILD_CONTRACT", "IS_LAW_PROVIDED", DbType.Int32, ColumnProperty.None, (int)YesNo.No);
            this.Database.AddColumn("CR_OBJ_BUILD_CONTRACT", "WEBSITE", DbType.String);
            this.Database.AddColumn("CR_OBJ_BUILD_CONTRACT", "BUILD_CONTRACT_STATE", DbType.Int32, ColumnProperty.None);

            this.Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", "REPRESENTATIVE_SIGNED", DbType.Int32, ColumnProperty.None, (int)YesNo.No);
            this.Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", "REPRESENTATIVE_NAME", DbType.String);
            this.Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", "EXPLOITATION_ACCEPTED", DbType.Int32, ColumnProperty.None, (int)YesNo.No);
            this.Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", "WARRANTY_START_DATE", DbType.DateTime);
            this.Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", "WARRANTY_END_DATE", DbType.DateTime);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "WARRANTY_END_DATE");
            this.Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "WARRANTY_START_DATE");
            this.Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "EXPLOITATION_ACCEPTED");
            this.Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "REPRESENTATIVE_NAME");
            this.Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "REPRESENTATIVE_SIGNED");
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "BUILD_CONTRACT_STATE");
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "WEBSITE");
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "IS_LAW_PROVIDED");
            this.Database.RemoveColumn("CR_DICT_PROGRAM", "GOV_CUSTOMER_ID");
        }
    }
}