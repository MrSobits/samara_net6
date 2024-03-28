namespace Bars.GkhGji.Migrations._2017.Version_2017061300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2017061300")]
    [MigrationDependsOn(typeof(Version_2017051800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Field.Area_View", "GkhGji.DocumentsGji.ActCheck.Field.Area_Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Field.Area_View", "GkhGji.DocumentsGji.ActCheck.Field.Area_Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Field.Inspectors", "GkhGji.DocumentsGji.ActCheck.View");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Field.Inspectors", "GkhGji.DocumentsGji.ActCheck.View");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Field.AcquaintedWithDisposalCopy", "GkhGji.DocumentsGji.ActCheck.View");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Field.AcquaintedWithDisposalCopy", "GkhGji.DocumentsGji.ActCheck.View");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Field.DocumentPlace_View", "GkhGji.DocumentsGji.ActCheck.View");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Field.DocumentPlace_View", "GkhGji.DocumentsGji.ActCheck.View");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Field.DocumentTime", "GkhGji.DocumentsGji.ActCheck.View");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Field.DocumentTime", "GkhGji.DocumentsGji.ActCheck.View");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.Requisites", "GkhGji.DocumentsGji.ActCheck.View");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.Requisites", "GkhGji.DocumentsGji.ActCheck.View");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.Period.View", "GkhGji.DocumentsGji.ActCheck.Register.Period.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.Period.View", "GkhGji.DocumentsGji.ActCheck.Register.Period.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.Violation.View", "GkhGji.DocumentsGji.ActCheck.Register.Violation.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.Violation.View", "GkhGji.DocumentsGji.ActCheck.Register.Violation.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.Annex.View", "GkhGji.DocumentsGji.ActCheck.Register.Annex.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.Annex.View", "GkhGji.DocumentsGji.ActCheck.Register.Annex.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.View", "GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.View", "GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc", "GkhGji.DocumentsGji.ActCheck.View");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc", "GkhGji.DocumentsGji.ActCheck.View");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.Violation.View", "GkhGji.DocumentsGji.ActCheck.Register.Violation.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.ActCheck.Register.Violation.View", "GkhGji.DocumentsGji.ActCheck.Register.Violation.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Prescription.Register.Requisites", "GkhGji.DocumentsGji.Prescription.View");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Prescription.Register.Requisites", "GkhGji.DocumentsGji.Prescription.View");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Prescription.Register.Violation.View", "GkhGji.DocumentsGji.Prescription.Register.Violation.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Prescription.Register.Violation.View", "GkhGji.DocumentsGji.Prescription.Register.Violation.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Prescription.Register.Cancel.View", "GkhGji.DocumentsGji.Prescription.Register.Cancel.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Prescription.Register.Cancel.View", "GkhGji.DocumentsGji.Prescription.Register.Cancel.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Prescription.Register.Annex.View", "GkhGji.DocumentsGji.Prescription.Register.Annex.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Prescription.Register.Annex.View", "GkhGji.DocumentsGji.Prescription.Register.Annex.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Prescription.Register.Directions", "GkhGji.DocumentsGji.Prescription.View");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Prescription.Register.Directions", "GkhGji.DocumentsGji.Prescription.View");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.Requisites", "GkhGji.DocumentsGji.Protocol.View");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.Requisites", "GkhGji.DocumentsGji.Protocol.View");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.Directions", "GkhGji.DocumentsGji.Protocol.View");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.Directions", "GkhGji.DocumentsGji.Protocol.View");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.Violation.View", "GkhGji.DocumentsGji.Protocol.Register.Violation.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.Violation.View", "GkhGji.DocumentsGji.Protocol.Register.Violation.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.View", "GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.View", "GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.Definition.View", "GkhGji.DocumentsGji.Protocol.Register.Definition.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.Definition.View", "GkhGji.DocumentsGji.Protocol.Register.Definition.Edit");

            PermissionMigrator.ApplyRolePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.Annex.View", "GkhGji.DocumentsGji.Protocol.Register.Annex.Edit");
            PermissionMigrator.ApplyStatePermission(this.Database, "GkhGji.DocumentsGji.Protocol.Register.Annex.View", "GkhGji.DocumentsGji.Protocol.Register.Annex.Edit");
        }

        public override void Down()
        {
        }
    }
}