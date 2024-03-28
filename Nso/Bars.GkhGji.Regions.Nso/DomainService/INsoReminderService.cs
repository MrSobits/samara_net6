namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using Bars.B4;

    // Интерфейс для дополнительных функций по Напоминаниям в регионе НСО
    public interface INsoReminderService
    {
        // Метод получения только Напоминаний по Обращениям в регионе НСО
        IDataResult ListAppealCitsReminder(BaseParams baseParams);
    }
}
