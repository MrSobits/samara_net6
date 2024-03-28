namespace Bars.GkhCr.DomainService 
{
    using B4;

    public interface IProgramCrService
    {
        IDataResult CopyProgram(BaseParams baseParams);

        IDataResult ListForQualification(BaseParams baseParams);

        IDataResult ListWithoutPaging(BaseParams baseParams);

        IDataResult GetAonProgramsList(BaseParams baseParams);

        IDataResult RealityObjectList(BaseParams baseParams);

        /// <summary>
        /// Заполнить поле "Номер ГЖИ" для объектов капремонта
        /// </summary>
        IDataResult GjiNumberFill(BaseParams baseParams);
    }
}
