using Bars.Gkh.Entities;

namespace Bars.Gkh.Regions.Nao
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            #region Жилой дом - поля

            Namespace<RealityObject>("Gkh.RealityObject.Field", "Поля");

            Namespace("Gkh.RealityObject.Field.View", "Просмотр");
            Permission("Gkh.RealityObject.Field.View.AreaFederalOwned_View", "Площадь федеральной собственности (кв.м.)");
            Permission("Gkh.RealityObject.Field.View.AreaCommercialOwned_View", "Площадь коммерческой собственности (кв.м.)");

            Namespace("Gkh.RealityObject.Field.Edit", "Редактирование");
            Permission("Gkh.RealityObject.Field.Edit.AreaFederalOwned_Edit", "Площадь федеральной собственности (кв.м.)");
            Permission("Gkh.RealityObject.Field.Edit.AreaCommercialOwned_Edit", "Площадь коммерческой собственности (кв.м.)");

            #endregion Жилой дом - поля

            #region Настройки для выгрузки в Клиент-Сбербанк

            Namespace("Gkh.Dictionaries.ASSberbankClient", "Настройки для выгрузки в Клиент-Сбербанк");
            Permission("Gkh.Dictionaries.ASSberbankClient.View", "Просмотр");
            
            #endregion
        }
    }
}