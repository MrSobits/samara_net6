namespace Bars.GkhGji.Permissions
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    
    /// <summary>
    /// Пермишн класс для GkhGji
    /// </summary>
    public class GjiPermission : IGjiPermission
    {
        private class GjiChelyabinskPermissionMap : PermissionMap
        {
            public GjiChelyabinskPermissionMap()
            {
                #region Рассмотрение
                this.Namespace<AppealCits>("GkhGji.AppealCitizensState.Field.Consideration", "Рассмотрение");
                this.Permission("GkhGji.AppealCitizensState.Field.Consideration.ZonalInspection_Edit", "ГЖИ, рассмотревшая обращение");
                this.Permission("GkhGji.AppealCitizensState.Field.Consideration.Surety_Edit", "ФИО поручителя");
                this.Permission("GkhGji.AppealCitizensState.Field.Consideration.SuretyResolve_Edit", "Резолюция");
                this.Permission("GkhGji.AppealCitizensState.Field.Consideration.SuretyDate_Edit", "Дата поручения (Поручитель)");

                this.Permission("GkhGji.AppealCitizensState.Field.Consideration.Executant_Edit", "ФИО исполнителя");
                this.Permission("GkhGji.AppealCitizensState.Field.Consideration.Tester_Edit", "Проверяющий");
                this.Permission("GkhGji.AppealCitizensState.Field.Consideration.ExecuteDate_Edit", "Срок исполнения (Исполнитель)");

                #endregion Рассмотрение
            }
        }

        /// <summary>
        /// Приоритет при получении пермишна
        /// </summary>
        public int Priority => 0;

        /// <summary>
        /// Получение пермишна
        /// </summary>
        /// <returns></returns>
        public PermissionMap GetPermissionMap()
        {
            return new GjiChelyabinskPermissionMap();
        }
    }
}