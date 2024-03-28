namespace Bars.Gkh.DomainService
{
    using B4;

    public interface IZonalInspectionService
    {
        IDataResult AddInspectors(BaseParams baseParams);

        IDataResult AddMunicipalities(BaseParams baseParams);

        IDataResult GetByOkato(BaseParams baseParams);
    }
}