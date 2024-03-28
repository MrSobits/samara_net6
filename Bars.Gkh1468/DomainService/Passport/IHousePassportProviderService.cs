namespace Bars.Gkh1468.DomainService
{
    using B4;

    public interface IHousePassportProviderService
    {
        IDataResult MakeNewPassport(BaseParams baseParams);

        IDataResult GetActivePassportStruct(BaseParams baseParams);

        IDataResult ListByPassport(BaseParams baseParams);
    }
}