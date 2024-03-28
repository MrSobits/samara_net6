namespace Bars.GkhGji.Migrations._2016.Version_2016061700
{
    using System.Collections.Generic;

    using Bars.Gkh;

    /// <summary>
    /// Миграция Gkh 2016061700
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016061700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2016061000.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        private readonly List<string> protocolMvdPermissions = new List<string>
        {
            "GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentNumber_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.DateSupply_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_Edit",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.Company_View",
            "GkhGji.DocumentsGji.ProtocolMvd.Field.Company_Edit",
        };
        private readonly List<string> protocolPermissions = new List<string>
        {
            "GkhGji.DocumentsGji.Resolution.Field.DocumentDate_View",
            "GkhGji.DocumentsGji.Resolution.Field.DocumentDate_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.resolutionBaseName_View",
            "GkhGji.DocumentsGji.Resolution.Field.resolutionBaseName_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.GisUin_View",
            "GkhGji.DocumentsGji.Resolution.Field.GisUin_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.OffenderWas_View",
            "GkhGji.DocumentsGji.Resolution.Field.OffenderWas_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.DeliveryDate_View",
            "GkhGji.DocumentsGji.Resolution.Field.DeliveryDate_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.TypeInitiativeOrg_View",
            "GkhGji.DocumentsGji.Resolution.Field.TypeInitiativeOrg_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.FineMunicipality_View",
            "GkhGji.DocumentsGji.Resolution.Field.FineMunicipality_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.Official_View",
            "GkhGji.DocumentsGji.Resolution.Field.Official_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.Municipality_View",
            "GkhGji.DocumentsGji.Resolution.Field.Municipality_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.Sanction_View",
            "GkhGji.DocumentsGji.Resolution.Field.Sanction_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.TypeTerminationBasement_View",
            "GkhGji.DocumentsGji.Resolution.Field.TypeTerminationBasement_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.DocumentNumber_View",
            "GkhGji.DocumentsGji.Resolution.Field.Paided_View",
            "GkhGji.DocumentsGji.Resolution.Field.Paided_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.DateTransferSsp_View",
            "GkhGji.DocumentsGji.Resolution.Field.Executant_View",
            "GkhGji.DocumentsGji.Resolution.Field.RulinFio_View",
            "GkhGji.DocumentsGji.Resolution.Field.RulinFio_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.DocumentNumSsp_View",
            "GkhGji.DocumentsGji.Resolution.Field.PenaltyAmount_View",
            "GkhGji.DocumentsGji.Resolution.Field.PenaltyAmount_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.RulingNumber_View",
            "GkhGji.DocumentsGji.Resolution.Field.RulingNumber_Edit",
            "GkhGji.DocumentsGji.Resolution.Field.RulingDate_View",
            "GkhGji.DocumentsGji.Resolution.Field.RulingDate_Edit",
        };

        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.RemoveColumn("GJI_PROTOCOL_MVD", "MUNICIPALITY_ID_RT");

            foreach (var permission in this.protocolMvdPermissions)
            {
                this.InsertPermission(permission);
            }

            foreach (var permission in this.protocolPermissions)
            {
                this.InsertPermission(permission);
            }

            foreach (var permission in this.protocolMvdPermissions)
            {
                this.InsertStatePermission(permission, "gji_document_protocolmvd");
            }

            foreach (var permission in this.protocolPermissions)
            {
                this.InsertStatePermission(permission, "gji_document_resol");
            }
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            foreach (var permission in this.protocolMvdPermissions)
            {
                this.DeletePermission("b4_role_permission",permission);
                this.DeletePermission("b4_state_role_permission", permission);
            }

            foreach (var permission in this.protocolPermissions)
            {
                this.DeletePermission("b4_role_permission", permission);
                this.DeletePermission("b4_state_role_permission", permission);
            }
        }

        private void InsertPermission(string permission )
        {
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select '"+ permission +"', id  from b4_role r "
            + "where not exists(select permission_id from b4_role_permission p where p.permission_id = '"+ permission + "' and p.role_id = r.id)");
        }

        private void InsertStatePermission(string permission, string type)
        {
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                + $"Select '{permission}', 0,now(),now(), r.id, s.id  from b4_role r, b4_state s "
                + "where not exists("
                + $"select permission_id from b4_state_role_permission p where p.permission_id = '{permission}' "
                + $"and p.role_id = r.id and p.state_id = s.id and s.type_id = '{type}') "
                + $"and s.type_id = '{type}'");
        }

        private void DeletePermission(string table, string permission)
        {
            this.Database.ExecuteNonQuery($"delete from {table} where permission_id = '{permission}'");
        }


    }
}
