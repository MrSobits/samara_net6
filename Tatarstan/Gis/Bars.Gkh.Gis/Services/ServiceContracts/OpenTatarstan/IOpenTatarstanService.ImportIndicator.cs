namespace Bars.Gkh.Gis.Services.ServiceContracts.OpenTatarstan
{
    using System.ServiceModel;
    using DataContracts.OpenTatarstan;

    public partial interface IOpenTatarstanService
    {
        [XmlSerializerFormat]
        [System.ServiceModel.OperationContractAttribute(Action = "http://open.tatarstan.ru/IIndicator/import_indicator", ReplyAction = "http://open.tatarstan.ru/IIndicator/import_indicatorResponse")]
        import_indicatorResponseImport_indicatorResult import_indicator(indicator request);
    }
}
