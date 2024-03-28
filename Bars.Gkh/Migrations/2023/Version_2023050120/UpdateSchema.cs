namespace Bars.Gkh.Migrations._2023.Version_2023050120
{
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050120")]

    [MigrationDependsOn(typeof(Version_2023050119.UpdateSchema))]

    /// Является Version_2019041900 из ядра
    public class UpdateSchema : Migration
    {
        private readonly List<string> permissionList = new List<string> { "Reports.CR.PlanedProgramIndicators", "Reports.RF.RoomAreaControl" };

        public override void Up()
        {
            var sb = new StringBuilder();
            foreach (var permission in this.permissionList)
            {
                sb.Append($@"INSERT INTO B4_ROLE_PERMISSION (permission_id, role_id) 
                                            SELECT '{permission}', id
                                            FROM B4_ROLE r
                                            WHERE NOT EXISTS(SELECT null FROM B4_ROLE_PERMISSION p
                                                            WHERE p.permission_id = '{permission}' AND role_id = r.id);");
            }

            this.Database.ExecuteNonQuery(sb.ToString());
        }

        public override void Down()
        {
            var sb = new StringBuilder();
            foreach (var permission in this.permissionList)
            {
                sb.Append($@"DELETE FROM B4_ROLE_PERMISSION WHERE permission_id = '{permission}';");
            }

            this.Database.ExecuteNonQuery(sb.ToString());
        }
    }
}