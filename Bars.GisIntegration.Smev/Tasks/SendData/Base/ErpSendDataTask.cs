namespace Bars.GisIntegration.Smev.Tasks.SendData.Base
{
    using Bars.GisIntegration.Smev.Entity;
    using Bars.GisIntegration.Smev.SmevExchangeService.Erp;

    public abstract class ErpSendDataTask<TServiceResponseType> : SmevBaseSendDataTask<TServiceResponseType, LetterFromErpType, ErpGuid>
    {
        /// <inheritdoc />
        protected override string SmevCommunicationUrl => "urn://ru.gov.proc.erp.communication/5.0.2";
    }
}