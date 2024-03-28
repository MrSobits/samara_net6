namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using Bars.B4;

    public interface IExportPenaltyService
    {
        IDataResult Export(BaseParams baseParams);
        IDataResult ExportPenaltyExcel(BaseParams baseParams);
    }
}