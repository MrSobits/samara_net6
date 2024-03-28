namespace Bars.Gkh.Migrations._2017.Version_2017052200
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [Migration("2017052200")]
    [MigrationDependsOn(typeof(Version_2017050800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(
                @"
                    --редактирование
                    insert into b4_role_permission (permission_id, role_id)
                      select
                        replace(p1.permission_id, 'Gkh.RealityObject.Field.', 'Gkh.RealityObject.Field.Edit.'),
                        role_id
                      from b4_role_permission p1
                      where p1.permission_id like 'Gkh.RealityObject.Field.%_Edit'
                            and not exists(select 1
                                           from b4_role_permission p2
                                           where p1.role_id = p2.role_id and
                                                 replace(p1.permission_id, 'Gkh.RealityObject.Field.', 'Gkh.RealityObject.Field.Edit.') =
                                                 p2.permission_id
                      );

                    --редактирование по статусам
                    insert into b4_state_role_permission (object_version, object_create_date, object_edit_date, permission_id, role_id, state_id)
                      select
                        0,
                        now(),
                        now(),
                        replace(p1.permission_id, 'Gkh.RealityObject.Field.', 'Gkh.RealityObject.Field.Edit.'),
                        role_id,
                        state_id
                      from b4_state_role_permission p1
                      where p1.permission_id like 'Gkh.RealityObject.Field.%_Edit'
                            and not exists(select 1
                                           from b4_state_role_permission p2
                                           where p1.role_id = p2.role_id and
                                                 replace(p1.permission_id, 'Gkh.RealityObject.Field.', 'Gkh.RealityObject.Field.Edit.') =
                                                 p2.permission_id and p1.state_id = p2.state_id
                      );

                    --просмотр
                    insert into b4_role_permission (permission_id, role_id)
                      select
                        replace(p1.permission_id, 'Gkh.RealityObject.Field.', 'Gkh.RealityObject.Field.View.'),
                        role_id
                      from b4_role_permission p1
                      where p1.permission_id like 'Gkh.RealityObject.Field.%_View'
                            and not exists(select 1
                                           from b4_role_permission p2
                                           where p1.role_id = p2.role_id and
                                                 replace(p1.permission_id, 'Gkh.RealityObject.Field.', 'Gkh.RealityObject.Field.View.') =
                                                 p2.permission_id
                      );

                    --просмотр по статусам
                    insert into b4_state_role_permission (object_version, object_create_date, object_edit_date, permission_id, role_id, state_id)
                      select
                        0,
                        now(),
                        now(),
                        replace(p1.permission_id, 'Gkh.RealityObject.Field.', 'Gkh.RealityObject.Field.View.'),
                        role_id,
                        state_id
                      from b4_state_role_permission p1
                      where p1.permission_id like 'Gkh.RealityObject.Field.%_View'
                            and not exists(select 1
                                           from b4_state_role_permission p2
                                           where p1.role_id = p2.role_id and
                                                 replace(p1.permission_id, 'Gkh.RealityObject.Field.', 'Gkh.RealityObject.Field.View.') =
                                                 p2.permission_id and p1.state_id = p2.state_id
                      );"
                );

            this.ApplyNotExistsPermissions();
        }

        public override void Down()
        {
            this.Database.ExecuteNonQuery(
                @"
                    delete
                    from b4_role_permission
                    where permission_id like 'Gkh.RealityObject.Field.Edit.%';

                    delete
                    from b4_role_permission
                    where permission_id like 'Gkh.RealityObject.Field.View.%';

                    delete
                    from b4_state_role_permission
                    where permission_id like 'Gkh.RealityObject.Field.Edit.%';

                    delete
                    from b4_state_role_permission
                    where permission_id like 'Gkh.RealityObject.Field.View.%';"
                );
        }

        private void ApplyNotExistsPermissions()
        {
            var notExistsPermissionKeys = new List<string>
            {
                "Gkh.RealityObject.Field.View.AddressCode_View",
                "Gkh.RealityObject.Field.View.AreaCommonUsage_View",
                "Gkh.RealityObject.Field.View.AreaGovernmentOwned_View",
                "Gkh.RealityObject.Field.View.AreaLivingNotLivingMkd_View",
                "Gkh.RealityObject.Field.View.AreaLivingOwned_View",
                "Gkh.RealityObject.Field.View.AreaLiving_View",
                "Gkh.RealityObject.Field.View.AreaMkd_View",
                "Gkh.RealityObject.Field.View.AreaMunicipalOwned_View",
                "Gkh.RealityObject.Field.View.AreaNotLivingFunctional_View",
                "Gkh.RealityObject.Field.View.AreaOwned_View",
                "Gkh.RealityObject.Field.View.BuildYear_View",
                "Gkh.RealityObject.Field.View.CadastralHouseNumber_View",
                "Gkh.RealityObject.Field.View.CadastreNumber_View",
                "Gkh.RealityObject.Field.View.CodeErc_View",
                "Gkh.RealityObject.Field.View.ConditionHouse_View",
                "Gkh.RealityObject.Field.View.CulturalHeritageAssignmentDate_View",
                "Gkh.RealityObject.Field.View.DateCommissioningLastSection_View",
                "Gkh.RealityObject.Field.View.DateCommissioning_View",
                "Gkh.RealityObject.Field.View.DateDemolition_View",
                "Gkh.RealityObject.Field.View.DateTechInspection_View",
                "Gkh.RealityObject.Field.View.FederalNum_View",
                "Gkh.RealityObject.Field.View.FiasAddress_View",
                "Gkh.RealityObject.Field.View.FloorHeight_View",
                "Gkh.RealityObject.Field.View.Floors_View",
                "Gkh.RealityObject.Field.View.GkhCode_View",
                "Gkh.RealityObject.Field.View.HasJudgmentCommonProp_View",
                "Gkh.RealityObject.Field.View.HasPrivatizedFlats_View",
                "Gkh.RealityObject.Field.View.IsBuildSocialMortgage_View",
                "Gkh.RealityObject.Field.View.IsInsuredObject_View",
                "Gkh.RealityObject.Field.View.MaximumFloors_View",
                "Gkh.RealityObject.Field.View.NecessaryConductCr_View",
                "Gkh.RealityObject.Field.View.NumberApartments_View",
                "Gkh.RealityObject.Field.View.NumberEntrances_View",
                "Gkh.RealityObject.Field.View.NumberLifts_View",
                "Gkh.RealityObject.Field.View.NumberLiving_View",
                "Gkh.RealityObject.Field.View.PercentDebt_View",
                "Gkh.RealityObject.Field.View.PhysicalWear_View",
                "Gkh.RealityObject.Field.View.PrivatizationDateFirstApartment_View",
                "Gkh.RealityObject.Field.View.RealEstateType_View",
                "Gkh.RealityObject.Field.View.ResidentsEvicted_View",
                "Gkh.RealityObject.Field.View.TotalBuildingVolume_View",
                "Gkh.RealityObject.Field.View.TypeHouse_View",
                "Gkh.RealityObject.Field.View.TypeProject_View",
                "Gkh.RealityObject.Field.View.WebCameraUrl_View"
            };

            PermissionMigrator.GrantForAllRole(this.Database, notExistsPermissionKeys.ToArray());
            PermissionMigrator.GrantForAllState(this.Database, "gkh_real_obj", notExistsPermissionKeys.ToArray());
        }
    }
}