namespace Bars.Gkh.Domain.TableLocker
{
    using Bars.B4;

    /// <summary>
    ///     Сервис работы с блокировками
    /// </summary>
    public interface ITableLockService
    {
        /// <summary>
        ///     Список заблокированных таблиц
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult List(BaseParams baseParams);

        /// <summary>
        ///     Снять блокировку с таблицы
        /// </summary>
        /// <param name="baseParams"></param>
        void Unlock(BaseParams baseParams);

        /// <summary>
        ///     Снять все блокировки
        /// </summary>
        void UnlockAll();
    }
}