using Bars.B4.Utils;

namespace Bars.GkhGji.Regions.Chelyabinsk.Enums.Egrn
{
    /// <summary>
    /// Вид запроса данных по ОН
    /// </summary>
    public enum RequestDataType
    {
        [Display("Выписка из Единого государственного реестра объектов недвижимости об объекте недвижимости")]
        [Description("extractRealty")]
        extractRealty = 10,

        [Display("Выписка из Единого государственного реестра объектов недвижимости о переходе прав на объект недвижимости")]
        [Description("extractRealtyList")]
        extractRealtyList = 20,

        [Display("Выписка из Единого государственного реестра объектов недвижимости о зарегистрированных договорах участия в долевом строительстве")]
        [Description("extractEquityConstructionContract")]
        extractEquityConstructionContract = 30,

        [Display("Выписка из Единого государственного реестра объектов недвижимости об основных характеристиках и зарегистрированных правах на объект недвижимости")]
        [Description("extrextractObjectMainFeaturesRightsactRealty")]
        extractObjectMainFeaturesRights = 40,

        [Display("Выписка о дате получения органом регистрации прав заявления о государственном кадастровом учете и (или) государственной регистрации прав и прилагаемых к нему документов")]
        [Description("extractStatementReceiptDate")]
        extractStatementReceiptDate = 50
    }
}
