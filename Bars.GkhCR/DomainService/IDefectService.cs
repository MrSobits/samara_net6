namespace Bars.GkhCr.DomainService
{
    using B4;
    using Entities;

    public interface IDefectService
    {
        IDataResult CalcInfo(BaseParams baseParams);

        IDataResult CalcInfo(DefectList defectList);

        IDataResult WorksForDefectList(BaseParams baseParams);

        IDataResult GetDefectListViewValue(BaseParams baseParams);
    }
}