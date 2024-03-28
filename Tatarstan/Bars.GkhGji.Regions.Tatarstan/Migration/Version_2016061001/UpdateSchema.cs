namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2016061001
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;


    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016061001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tatarstan.Migration.Version_2014112700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.ClearExistRolePermissions();

            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentNumber_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateSupply_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_Edit', id  from b4_role");

            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.DocumentDate_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.DocumentDate_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.resolutionBaseName_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.resolutionBaseName_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.GisUin_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.GisUin_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.OffenderWas_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.OffenderWas_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.DeliveryDate_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.DeliveryDate_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.TypeInitiativeOrg_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.TypeInitiativeOrg_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.FineMunicipality_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.FineMunicipality_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.Official_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.Official_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.Municipality_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.Municipality_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.Sanction_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.Sanction_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.TypeTerminationBasement_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.TypeTerminationBasement_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.DocumentNumber_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.Paided_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.Paided_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.DateTransferSsp_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.Executant_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.Executant_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.RulinFio_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.RulinFio_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.DocumentNumSsp_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.PenaltyAmount_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.PenaltyAmount_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.RulingNumber_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.RulingNumber_Edit', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.RulingDate_View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'GkhGji.DocumentsGji.Protocol.Field.RulingDate_Edit', id  from b4_role");

            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentNumber_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateSupply_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) "
                +
                "Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_View',  0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_Edit',  0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_View', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
            this.Database.ExecuteNonQuery("INSERT INTO b4_state_role_permission( permission_id, object_version,object_create_date, object_edit_date, role_id, state_id) Select 'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_Edit', 0,now(),now(), b4_role.id, b4_state.id  from b4_role, b4_state where b4_state.type_id = 'gji_document_protocolmvd'");
        }

        public override void Down()
        {
            this.ClearExistRolePermissions();

            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentNumber_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateSupply_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_View'");
            this.Database.ExecuteNonQuery("delete from b4_state_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_Edit'");
        }

        private void ClearExistRolePermissions()
        {
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentNumber_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateSupply_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.ProtocolMvd.Field.Company_Edit'");

            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.DocumentDate_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.DocumentDate_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.resolutionBaseName_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.resolutionBaseName_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.GisUin_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.GisUin_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.OffenderWas_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.OffenderWas_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.DeliveryDate_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.DeliveryDate_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.TypeInitiativeOrg_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.TypeInitiativeOrg_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.FineMunicipality_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.FineMunicipality_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.Official_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.Official_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.Municipality_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.Municipality_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.Sanction_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.Sanction_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.TypeTerminationBasement_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.TypeTerminationBasement_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.DocumentNumber_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.Paided_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.Paided_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.DateTransferSsp_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.Executant_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.Executant_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.RulinFio_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.RulinFio_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.DocumentNumSsp_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.PenaltyAmount_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.PenaltyAmount_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.RulingNumber_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.RulingNumber_Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.RulingDate_View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'GkhGji.DocumentsGji.Protocol.Field.RulingDate_Edit'");
        }
    }
}