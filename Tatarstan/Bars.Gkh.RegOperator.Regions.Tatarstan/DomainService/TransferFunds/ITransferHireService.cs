namespace Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService
{
    using B4;

    public interface ITransferHireService
    {
        // Метод рапсчета перечислений по найму 
        IDataResult Calc(BaseParams baseParams);
    }
}