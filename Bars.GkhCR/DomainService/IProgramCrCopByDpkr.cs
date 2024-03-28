namespace Bars.GkhCr.DomainService
{
    using Bars.B4;

    public interface IProgramCrCopByDpkr
    {
        // Метод копирования программы с учетом того что она создана из ДПКР
        IDataResult CopyProgramCr(BaseParams baseParams);
    }
}
