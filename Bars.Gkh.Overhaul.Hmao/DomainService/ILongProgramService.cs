using System.Collections.Generic;
using System.Linq;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Overhaul.Enum;
using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using Bars.B4;
    using Enums;

    public interface ILongProgramService
    {
        IDataResult MakeLongProgram(BaseParams baseParams);

        IDataResult MakeLongProgramAll(BaseParams baseParams);

        /// <summary>
        /// Создание ДПКР для опебликования
        /// </summary>
        IDataResult CreateDpkrForPublish(BaseParams baseParams);

        IDataResult GetParams(BaseParams baseParams);

        /// <summary>
        /// Удаление программы ДПКР
        /// </summary>
        /// <param name="baseParams"></param>
        IDataResult DeleteDpkr(BaseParams baseParams);

        /// <summary>
        /// Проверка программы ДПКР
        /// </summary>
        /// <param name="baseParams"></param>
        IDataResult ValidationDeleteDpkr(BaseParams baseParams);

        /// <summary>
        /// Расчет записей  первого этапа
        /// </summary>
        /// <param name="baseParams"></param>
        IEnumerable<RealityObjectStructuralElementInProgramm> GetStage1(int startYear, long municipalityId,
            IQueryable<RealityObjectStructuralElement> roStructElQuery, int? endYear = null, bool fromActualize = false);

        /// <summary>
        /// Расчет записей  второго этапа
        /// </summary>
        /// <param name="baseParams"></param>
        IEnumerable<RealityObjectStructuralElementInProgrammStage2> GetStage2(
            IEnumerable<RealityObjectStructuralElementInProgramm> stage1Records);

        /// <summary>
        /// Расчет записей второго этапа
        /// </summary>
        /// <param name="baseParams"></param>
        IEnumerable<RealityObjectStructuralElementInProgrammStage3> GetStage3(
            IEnumerable<RealityObjectStructuralElementInProgrammStage2> stage2Records);

        decimal? GetDpkrCost(long muId, long? settlementId, int workPriceYear, long strElemId, long roId, long capitalGroupId,
            PriceCalculateBy calculateBy,
            decimal objVolume, decimal? areaLiving, decimal? areaMkd, decimal? areaLivingNotLivingMkd);

        /// <summary>
        /// Получение всех параметров очередности ДПКР
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат</returns>
        IDataResult GetAllParams(BaseParams baseParams);
    }
}