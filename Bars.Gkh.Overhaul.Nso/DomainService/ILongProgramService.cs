using System.Collections.Generic;
using System.Linq;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Overhaul.Nso.Entities;

namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using Bars.B4;

    public interface ILongProgramService
    {
        IDataResult MakeLongProgram(BaseParams baseParams);

        /// <summary>
        /// Создание ДПКР для опебликования
        /// </summary>
        IDataResult CreateDpkrForPublish(BaseParams baseParams);

        IDataResult GetParams(BaseParams baseParams);
        
        IDataResult SetPriority(BaseParams baseParams);

        /// <summary>
        /// Получение дерева ООИ с детализацией по КЭ
        /// </summary>
        /// <param name="baseParams">Содержащий st3Id - id 3 этапа ДПКР</param>
        /// <returns>Дерево</returns>
        IDataResult ListDetails(BaseParams baseParams);

        /// <summary>
        /// Получение краткой информации по 3 этапу ДПКР
        /// </summary>
        /// <param name="baseParams">Содержащий st3Id - id 3 этапа ДПКР</param>
        /// <returns>Объект</returns>
        IDataResult GetInfo(BaseParams baseParams);

        /// <summary>
        /// Получение видов работ по объекту недвижимости
        /// </summary>
        /// <param name="baseParams">Содержащий идентификатор 3 этапа ДПКР</param>
        IDataResult ListWorkTypes(BaseParams baseParams);

        /// <summary>
        /// Сохранение новой версии программы
        /// </summary>
        /// <param name="baseParams"></param>
        IDataResult MakeNewVersion(BaseParams baseParams);

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
        IEnumerable<RealityObjectStructuralElementInProgramm> GetStage1(int startYear,
            IQueryable<RealityObjectStructuralElement> roStructElQuery = null);

        /// <summary>
        /// Расчет 2 и 3 этапа
        /// </summary>
        /// <param name="baseParams"></param>
        void GetStage2And3(IEnumerable<RealityObjectStructuralElementInProgramm> stage1Records, out List<RealityObjectStructuralElementInProgrammStage2> listSt2, out List<RealityObjectStructuralElementInProgrammStage3> listSt3);
    }
}