namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IGjiScriptService
    {
        BaseDataResult SetAddressAppeal(BaseParams baseParams);

        BaseDataResult ReminderGenerateFake(BaseParams baseParams);

        BaseDataResult SetZonalInspection(BaseParams baseParams);

        IDataResult CorrectDocNumbers(BaseParams baseParams);

        IDataResult ListRealityObjectOnSpecAcc(BaseParams baseParams);

        IDataResult TrySetOpenEDS(BaseParams baseParams);
    }
}