namespace Bars.GkhGji.Regions.Tatarstan.Migration._2020.Version_2020101300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2020101300")]
    [MigrationDependsOn(typeof(Version_2020092900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string ControlListTable = "GJI_CONTROL_LIST";
        private const string ControlListQuestionTable = "GJI_CONTROL_LIST_QUESTION";
        private const string ErpGuidColumn = "ERP_GUID";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(UpdateSchema.ControlListTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));
            this.Database.AddColumn(UpdateSchema.ControlListQuestionTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));

            this.Database.ExecuteNonQuery(this.GetQuery(false));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.ControlListTable, UpdateSchema.ErpGuidColumn);
            this.Database.RemoveColumn(UpdateSchema.ControlListQuestionTable, UpdateSchema.ErpGuidColumn);
            this.Database.ExecuteNonQuery(this.GetQuery(true));
        }

        private string GetQuery(bool isSet)
        {
            var command = isSet ? "set" : "drop";
            return $@"alter table public.gji_dict_prosecutor_office 
                alter column type {command} not null,
                alter column federal_district_code {command} not null,
                alter column federal_district_name {command} not null,
                alter column federal_center_name {command} not null,
                alter column district_code {command} not null;";
        }
    }
}
