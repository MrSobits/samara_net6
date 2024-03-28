namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;

    public interface IReminderService
    {
        IDataResult ListTaskState(BaseParams baseParams);

        IDataResult ListTaskControl(BaseParams baseParams);

        IDataResult ListReminderOfInspector(BaseParams baseParams);

        IDataResult ListReminderOfHead(BaseParams baseParams);

        IDataResult GetInfo(BaseParams baseParams);

        IDataResult ListWidgetInspector(BaseParams baseParams);

        IQueryable<Reminder> GetQueryable(BaseParams baseParams, IDomainService<Reminder> service);

        IDataResult ListTypeReminder(BaseParams baseParams);

        TypeReminder[] ReminderTypes();
    }
}
