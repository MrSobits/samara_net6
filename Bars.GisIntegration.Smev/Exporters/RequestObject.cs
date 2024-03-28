namespace Bars.GisIntegration.Smev.Exporters
{
    using Bars.GisIntegration.Smev.Enums;
    using Bars.GisIntegration.Smev.SmevExchangeService.Erp;

    public class RequestDataToErp
    {
        public ExporterType ExporterType { get; set; }

        public LetterToErpType RequestObjectData { get; set; }
    }
}
