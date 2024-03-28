namespace Bars.Gkh.Decisions.Nso.Permissions
{
    using B4;

    using Bars.Gkh.Decisions.Nso.Entities.Decisions;

    using Entities;

    public class DecisionsNsoPermissionMap : PermissionMap
    {
        public DecisionsNsoPermissionMap()
        {
            this.Namespace("Reports.DecisionsNso", "Модуль решений собственников");
            this.Permission("Reports.DecisionsNso.FundFormation", "Отчет по разделу уведомлений о способе формирования фонда");
            this.Permission("Reports.DecisionsNso.RepairAbovePayment", "Отчет о размере взноса на кап. ремонт");
            
            /*Namespace("Gkh.RealityObject.Register.ExistingSolutions", "Действующие решения");
            Permission("Gkh.RealityObject.Register.ExistingSolutions.View", "Просмотр");*/

            this.Namespace("Gkh.RealityObject.Register.DecisionHistory", "Действующие решения");
            this.Permission("Gkh.RealityObject.Register.DecisionHistory.View", "Просмотр");

            // Сделано для отображения реестра в карточке ЖД
            this.Namespace("Gkh.RealityObject.Register.DecisionProtocolsViewCreate", "Протоколы решений(карточка дома): Просмотр, создание");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.View", "Просмотр");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.Create", "Создание записей");

            this.Namespace("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes", "Типы протоколов");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes.Owners", "Протокол решения собственников жилых помещений");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes.Government", "Протокол решения органа государственной власти");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes.CrFund", "Протокол о формировании фонда капитального ремонта");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes.MkdManagementType", "Протокол о выборе формы управления многоквартирным домом");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes.ManagementOrganization", "Протокол о выборе управляющей компании для дома");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes.TariffApproval", "Протокол об утверждение тарифа на содержание и ремонт жилья");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes.OoiManagement", "Протокол по вопросам, связанных с эксплуатацией и управлением общим имуществом здания");

            this.Namespace<RealityObjectDecisionProtocol>("Gkh.RealityObject.Register.DecisionProtocolsEditDelete", "Протоколы решений(карточка дома): Изменение, удаление");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsEditDelete.Edit", "Изменение записей");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsEditDelete.Delete", "Удаление записей");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsEditDelete.CreateNotification", "Сформировать уведомление");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsEditDelete.PhoneAuthorizedPerson", "Телефон уполномоченного лица");
            this.Permission("Gkh.RealityObject.Register.DecisionProtocolsEditDelete.DownloadContract", "Скачать договор");

            this.Namespace("Gkh.RealityObject.Register.GovProtocolDecisionViewCreate", "Протоколы решений органа гос. власти(карточка дома): Просмотр, создание");
            this.Permission("Gkh.RealityObject.Register.GovProtocolDecisionViewCreate.View", "Просмотр");
            this.Permission("Gkh.RealityObject.Register.GovProtocolDecisionViewCreate.Create", "Создание записей");

            this.Namespace("Gkh.RealityObject.Register.GovProtocolDecisionViewCreate.Npa", "НПА");
            this.Permission("Gkh.RealityObject.Register.GovProtocolDecisionViewCreate.Npa.View", "Просмотр НПА");

            this.Namespace<GovDecision>("Gkh.RealityObject.Register.GovProtocolDecisionEditDelete", "Протоколы решений органа гос. власти(карточка дома): Изменение, удаление");
            this.Permission("Gkh.RealityObject.Register.GovProtocolDecisionEditDelete.Edit", "Изменение записей");
            this.Permission("Gkh.RealityObject.Register.GovProtocolDecisionEditDelete.Delete", "Удаление записей");

            this.Namespace<DecisionNotification>("Ovrhl.RegistryNotifications", "Реестр уведомлений");
            this.CRUDandViewPermissions("Ovrhl.RegistryNotifications");

            this.Namespace<CrFundDecisionProtocol>("Gkh.RealityObject.Register.CrFundDecisionProtocol", "Протокол о формировании фонда капитального ремонта: Изменение, удаление");
            this.Permission("Gkh.RealityObject.Register.CrFundDecisionProtocol.Edit", "Изменение записей");
            this.Permission("Gkh.RealityObject.Register.CrFundDecisionProtocol.Delete", "Удаление записей");

            this.Namespace<ManagementOrganizationDecisionProtocol>("Gkh.RealityObject.Register.ManagementOrganizationDecisionProtocol", "Протокол о выборе управляющей компании для дома: Изменение, удаление");
            this.Permission("Gkh.RealityObject.Register.ManagementOrganizationDecisionProtocol.Edit", "Изменение записей");
            this.Permission("Gkh.RealityObject.Register.ManagementOrganizationDecisionProtocol.Delete", "Удаление записей");

            this.Namespace<MkdManagementTypeDecisionProtocol>("Gkh.RealityObject.Register.MkdManagementTypeDecisionProtocol", "Протокол о выборе формы управления многоквартирным домом: Изменение, удаление");
            this.Permission("Gkh.RealityObject.Register.MkdManagementTypeDecisionProtocol.Edit", "Изменение записей");
            this.Permission("Gkh.RealityObject.Register.MkdManagementTypeDecisionProtocol.Delete", "Удаление записей");

            this.Namespace<OoiManagementDecisionProtocol>("Gkh.RealityObject.Register.OoiManagementDecisionProtocol", "Протокол по вопросам, связанных с эксплуатацией и управлением общим имуществом здания: Изменение, удаление");
            this.Permission("Gkh.RealityObject.Register.OoiManagementDecisionProtocol.Edit", "Изменение записей");
            this.Permission("Gkh.RealityObject.Register.OoiManagementDecisionProtocol.Delete", "Удаление записей");

            this.Namespace<TariffApprovalDecisionProtocol>("Gkh.RealityObject.Register.TariffApprovalDecisionProtocol", "Протокол об утверждение тарифа на содержание и ремонт жилья: Изменение, удаление");
            this.Permission("Gkh.RealityObject.Register.TariffApprovalDecisionProtocol.Edit", "Изменение записей");
            this.Permission("Gkh.RealityObject.Register.TariffApprovalDecisionProtocol.Delete", "Удаление записей");

            //Namespace("GkhGji.RegistryNotifications.DecisionNotification", "Сводный реестр уведомлений о решениях общего собрания");
            //Permission("GkhGji.RegistryNotifications.DecisionNotification.View", "Просмотр");
        }
    }
}