namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;

    public interface IExtReminderService
    {
        // Метод получения только Напоминаний по Обращениям в регионе НСО
        IDataResult ListAppealCitsReminder(BaseParams baseParams);
    }
}
