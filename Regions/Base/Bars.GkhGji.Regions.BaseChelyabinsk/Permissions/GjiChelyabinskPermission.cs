namespace Bars.GkhGji.Regions.BaseChelyabinsk.Permissions
{
    using Bars.B4;
    using Bars.GkhGji.Permissions;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    /// <summary>
    /// Класс GjiChelyabinskPermission
    /// </summary>
    public class GjiChelyabinskPermission : IGjiPermission
    {
        private class GjiChelyabinskPermissionMap : PermissionMap
        {
            /// <summary>
            /// Конструктор класса GjiChelyabinskPermissionMap
            /// </summary>
            public GjiChelyabinskPermissionMap()
            {
                #region Рассмотрение
                this.Namespace<AppealCitsExecutant>("GkhGji.AppealCitizensState.Field.Consideration", "Рассмотрение");
                this.Permission("GkhGji.AppealCitizensState.Field.Consideration.Create", "Создание записи");
                this.Permission("GkhGji.AppealCitizensState.Field.Consideration.Delete", "Удаление записи");
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
        /// Высший приоритет для получения пермишна
        /// </summary>
        public int Priority => 1;

        /// <summary>
        /// Получить пермишн
        /// </summary>
        public PermissionMap GetPermissionMap()
        {
            return new GjiChelyabinskPermissionMap();
        }
    }
}