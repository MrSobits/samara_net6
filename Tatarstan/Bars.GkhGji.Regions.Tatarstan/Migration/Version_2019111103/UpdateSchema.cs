namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019111103
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019111103")]
    [MigrationDependsOn(typeof(Version_2019111102.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_DICT_MANDATORY_REQS",
                new Column("mandratory_req_name", DbType.String, 500, ColumnProperty.NotNull),
                new Column("mandratory_req_content", DbType.String, 300, ColumnProperty.NotNull),
                new Column("start_date_mandator", DbType.DateTime, ColumnProperty.NotNull),
                new Column("end_date_mandatory", DbType.DateTime, ColumnProperty.NotNull),
                new Column("guid", DbType.String, 300, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GJI_DICT_MANDATORY_REQS_CONTOROL_TYPE",
                new RefColumn("MANDATORY_REQS_ID", ColumnProperty.NotNull, "GJI_DICT_MANDATORY_REQS_CONTOROL_TYPE_MR_ID", "GJI_DICT_MANDATORY_REQS", "ID"),
                new RefColumn("CONTORL_TYPE_ID", ColumnProperty.NotNull, "GJI_DICT_MANDATORY_REQS_CONTOROL_TYPE_CT_ID", "GJI_DICT_CONTROL_TYPES", "ID"));

            this.Database.AddEntityTable("GJI_DICT_MANDATORY_REQS_NORMATIVE_DOC",
                new RefColumn("MANDATORY_REQS_ID", ColumnProperty.NotNull, "GJI_DICT_MANDATORY_REQS_TYPE_NPA_MR_ID", "GJI_DICT_MANDATORY_REQS", "ID"),
                new RefColumn("NPA_ID", ColumnProperty.NotNull, "GJI_DICT_MANDATORY_REQS_NORMATIVE_DOC_ND_ID", "GKH_DICT_NORMATIVE_DOC", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_MANDATORY_REQS_CONTOROL_TYPE");
            this.Database.RemoveTable("GJI_DICT_MANDATORY_REQS_NORMATIVE_DOC");
            this.Database.RemoveTable("GJI_DICT_MANDATORY_REQS");
        }
    }
}
