using Bars.Gkh.Overhaul.Hmao.Entities;
using System.Collections.Generic;
using static Bars.Gkh.Overhaul.Hmao.Services.ActualizeDPKR.ActualizeDPKRService;

namespace Bars.Gkh.Overhaul.Hmao.Services.ActualizeDPKR
{
    public interface IActualizeDPKRService
    {
        /// <summary>
        /// Получить список записей на добавление
        /// </summary>
        /// <param name="version"></param>
        /// <param name="startYear"></param>
        /// <returns></returns>
        List<VersionRecordWithReasonView> GetAddEntriesList(ProgramVersion version, short startYear);

        /// <summary>
        /// Удалить запись из списка на добавление
        /// </summary>
        /// <param name="Id"></param>
        void RemoveHouseForAdd(long Id);

        /// <summary>
        /// Получить список записей на удаление
        /// </summary>
        /// <param name="version"></param>
        /// <param name="startYear"></param>
        /// <returns></returns>
        List<VersionRecordWithReasonView> GetDeleteEntriesList(ProgramVersion version, short startYear);

        /// <summary>
        /// Удалить запись из списка на удаление
        /// </summary>
        /// <param name="Id"></param>
        void RemoveHouseForDelete(long Id);

        /// <summary>
        /// Удалить выбранные записи из списков
        /// </summary>
        /// <param name="version"></param>
        /// <param name="selectedAddId"></param>
        /// <param name="selectedDeleteId"></param>
        void RemoveSelected(ProgramVersion version, long[] selectedAddId, long[] selectedDeleteId);

        /// <summary>
        /// Актуализировать записи
        /// </summary>
        /// <param name="version">Версия программы</param>
        /// <param name="startYear">Год начала актуализации; все добавленные работы должны быть после него</param>
        /// <param name="selectedAddId">Список id записей на добавление. Null означает все</param>
        /// <param name="selectedDeletedId">Список id записей на удаление. Null означает все</param>
        void Actualize(ProgramVersion version, short startYear, long[] selectedAddId = null, long[] selectedDeletedId = null);

        /// <summary>
        /// Очистить списки на добавление и удаление
        /// </summary>
        void ClearCache();      

    }
}
