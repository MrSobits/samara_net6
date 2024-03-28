namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using B4;

    public interface IProgramVersionService
    {
        IDataResult ChangeRecordsIndex(BaseParams baseParams);

        IDataResult CopyProgram(BaseParams baseParams);
    }
}
