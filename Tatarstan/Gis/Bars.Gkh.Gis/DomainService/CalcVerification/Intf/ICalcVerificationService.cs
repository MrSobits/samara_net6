namespace Bars.Gkh.Gis.DomainService.CalcVerification.Intf
{
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    using Entities.CalcVerification;
    using KP60.Protocol.Entities;

    /// <summary>
    /// Проверочный расчет
    /// </summary>
    public interface ICalcVerificationService
    {
        IDataResult VerificationCalc(BaseParams baseParams);
        Gkh.DataResult.ListDataResult<ChargeProxyOut> Delta(ChargeProxyIn chargeIn, DeltaParams Params);
        string GetProtocol(BaseParams baseParams);

        /// <summary>
        /// Дерево протокола расчетов
        /// </summary>
        TreeData GetTree(BaseParams baseParams);
    }
}