namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Dto;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    /// <summary>
    /// Интерфейс сервиса для <see cref="RapidResponseSystemAppeal"/>
    /// </summary>
    public interface IRapidResponseSystemAppealService
    {
        /// <summary>
        /// Изменить статус
        /// </summary>
        IDataResult ChangeState(BaseParams baseParams);

        /// <summary>
        /// Получить перечень контрагентов доступных для привязки
        /// </summary>
        IDataResult GetAvailableContragents(BaseParams baseParams);
        
        /// <summary>
        /// Получить статистику по реестру обращений ГЖИ, связанных с СОПР
        /// </summary>
        IDataResult GetAppealsStatistic();

        /// <summary>
        /// Получить статистику по реестру обращений СОПР
        /// </summary>
        IDataResult GetAppealDetailsStatistic();

        /// <summary>
        /// Получить обращения СОПР
        /// </summary>
        IQueryable<RapidResponseSystemAppealLink> GetSoprAppeals();

        /// <summary>
        /// Обновить у записей поле 'Контрольный срок'
        /// </summary>
        IDataResult UpdateSoprControlPeriod(BaseParams baseParams);

        /// <summary>
        /// Отправить электронное письмо о формировании нового обращения в СОПР
        /// </summary>
        IDataResult SendNotificationMail(BaseParams baseParams);

        /// <summary>
        /// Расчёт контрольного срока по заданной дате и добавляемым дням
        /// </summary>
        /// <param name="additionalDaysCount">Количество дней которые нужно добавить</param>
        /// <param name="fromDate">Дата отсчёта</param>
        /// <returns>Рассчитанная дата</returns>
        DateTime GetControlPeriodMaxDay(int additionalDaysCount, DateTime fromDate);
    }
}