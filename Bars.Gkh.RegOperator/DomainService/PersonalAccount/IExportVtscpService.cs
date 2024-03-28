namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using Bars.B4;

    public interface IExportVtscpService
    {
        IDataResult Export(BaseParams baseParams);      
    }
}