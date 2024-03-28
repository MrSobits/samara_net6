namespace Bars.GkhCr.DomainService
{
    using Bars.B4;
    using Bars.GkhCr.Entities;

    public interface ISpecialTypeWorkCrHistoryService
    {
        /// <summary>
        /// Метод  восстановления записи из истории
        /// </summary>
        IDataResult Restore(BaseParams baseParams);

        /// <summary>
        /// Метод создания истории после создания записи Вида работ в объекте КР
        /// </summary>
        SpecialTypeWorkCrHistory HistoryAfterCreation(SpecialTypeWorkCr typeWork, int? newYear = null);

        /// <summary>
        /// Метод создания истории после изменения записи Вида работ в объекте КР
        /// </summary>
        SpecialTypeWorkCrHistory HistoryAfterChange(SpecialTypeWorkCr typeWork, SpecialTypeWorkCr oldValue);

        /// <summary>
        /// Метод создания истории после удаления записи Вида работ в объекте КР
        /// </summary>
        SpecialTypeWorkCrHistory HistoryAfterRemove(SpecialTypeWorkCrRemoval typeWorkRemoval);

    }
}
