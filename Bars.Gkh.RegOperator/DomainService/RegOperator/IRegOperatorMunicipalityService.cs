namespace Bars.Gkh.RegOperator.DomainService
{
    using Bars.B4;

    public interface IRegOperatorMunicipalityService
    {
        IDataResult AddMunicipalities(BaseParams baseParams);

        IDataResult ListMuByRegOp(BaseParams baseParams);
    }
}