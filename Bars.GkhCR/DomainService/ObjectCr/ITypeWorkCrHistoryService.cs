namespace Bars.GkhCr.DomainService
{
    using Bars.B4;
    using Bars.GkhCr.Entities;

    public interface ITypeWorkCrHistoryService
    {
        /// <summary>
        /// Метод  восстановления записи из истории
        /// </summary>
        IDataResult Restore(BaseParams baseParams);

        /// <summary>
        /// Метод создания истории после создания записи Вида работ в объекте КР
        /// </summary>
        TypeWorkCrHistory HistoryAfterCreation(TypeWorkCr typeWork, int? newYear = null);

        /// <summary>
        /// Метод создания истории после изменения записи Вида работ в объекте КР
        /// </summary>
        TypeWorkCrHistory HistoryAfterChange(TypeWorkCr typeWork, TypeWorkCr oldValue);

        /// <summary>
        /// Метод создания истории после удаления записи Вида работ в объекте КР
        /// </summary>
        TypeWorkCrHistory HistoryAfterRemove(TypeWorkCrRemoval typeWorkRemoval);

    }
}
