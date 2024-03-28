namespace Bars.GkhCr.Regions.Tatarstan.DomainService
{
    using Bars.B4;

    public interface IRealityObjectOutdoorProgramService
    {
        /// <summary>
        /// Копирование программы двора.
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult CopyProgram(BaseParams baseParams);

        /// <summary>
        /// Получение списка наименований программ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetProgramNames(BaseParams baseParams);
    }
}
