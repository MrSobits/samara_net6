namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.B4;

    /// <summary>
    /// Пермишен для неплательщиков ЖКУ
    /// </summary>
    public class UtilityDebtorClaimWorkPermission : IClaimWorkPermission
    {
        private class UtilityDebtorPermission : PermissionMap
        {
            public UtilityDebtorPermission()
            {
                this.Namespace("Clw.ClaimWork.UtilityDebtor", "Основание ПИР - Неплательщики ЖКУ");
                this.Permission("Clw.ClaimWork.UtilityDebtor.View", "Просмотр");
                this.Permission("Clw.ClaimWork.UtilityDebtor.Add", "Добавление неплательщика");
                this.Permission("Clw.ClaimWork.UtilityDebtor.Import", "Импорт");

                this.Namespace("Clw.ClaimWork.UtilityDebtor.Debt", "Задолженность по оплате ЖКУ");
                this.Permission("Clw.ClaimWork.UtilityDebtor.Debt.View", "Просмотр");
                this.Permission("Clw.ClaimWork.UtilityDebtor.Debt.Save", "Редактирование");

                this.Namespace("Clw.ClaimWork.UtilityDebtor.ExecutoryProcess", "Исполнительное производство");
                this.Permission("Clw.ClaimWork.UtilityDebtor.ExecutoryProcess.View", "Просмотр");
                this.Permission("Clw.ClaimWork.UtilityDebtor.ExecutoryProcess.Save", "Редактирование");

                this.Namespace("Clw.ClaimWork.UtilityDebtor.SeizureOfProperty", "Постановление о наложении ареста на имущество");
                this.Permission("Clw.ClaimWork.UtilityDebtor.SeizureOfProperty.View", "Просмотр");
                this.Permission("Clw.ClaimWork.UtilityDebtor.SeizureOfProperty.Save", "Редактирование");

                this.Namespace("Clw.ClaimWork.UtilityDebtor.DepartureRestriction", "Постановление об ограничении выезда из РФ");
                this.Permission("Clw.ClaimWork.UtilityDebtor.DepartureRestriction.View", "Просмотр");
                this.Permission("Clw.ClaimWork.UtilityDebtor.DepartureRestriction.Save", "Редактирование");
            }
        }

        public PermissionMap GetPermissionMap()
        {
            return new UtilityDebtorPermission();
        }
    }
}