namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Permissions
{
    using B4;

    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;

    public class GkhRegOpRegionsTatarstanPermissionMap : PermissionMap
    {
        public GkhRegOpRegionsTatarstanPermissionMap()
        {
            Namespace("GkhRegOp.RegionalFundUse.ConfirmContributionDocs", 
                "Документы, подтверждающие поступление взносов на КР от собственников помещений");
            Permission("GkhRegOp.RegionalFundUse.ConfirmContributionDocs.View", "Просмотр");

            Permission("Import.ThirdPartyPersonalAccountImport", "Импорт лс из сторонних систем");
            Permission("Import.ThirdPartyPersonalAccountImportClosed", "Импорт лс из сторонних систем (импорт в закрытые периоды)");

            //Namespace<ConfirmContributionDoc>("Gkh.RegOperator.ConfirmContribution.EditData", "Редактирование данных");
            //Permission("GkhRf.ConfirmContribution.EditData.ConfirmContributionDoc", "Добавление документа");

            Permission("GkhRegOp.PersonalAccount.Registry.Action.ExportPersonalAccounts", "Выгрузка информации по ЛС");
        }
    }
}