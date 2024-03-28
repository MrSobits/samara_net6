namespace Bars.Gkh1468.DomainService
{
    using B4;

    public interface IOkiPassportProviderService
    {
        IDataResult MakeNewPassport(BaseParams baseParams);

        IDataResult GetActivePassportStruct(BaseParams baseParams);

        IDataResult ListByPassport(BaseParams baseParams);

        IDataResult MunicipalityForOki(BaseParams baseParams);
    }
}