namespace Bars.Gkh.Gis.DomainService.CalcVerification.Intf
{
    using System.Data;
    using B4;

    /// <summary>
    /// Перенос полученных начислений(УК+проверочные) на ЦХД
    /// </summary>
    public interface ITransfer
    {
        IDataResult TransferCharge();
    }
}