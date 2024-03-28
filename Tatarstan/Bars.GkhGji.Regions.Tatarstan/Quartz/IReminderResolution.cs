namespace Bars.GkhGji.Regions.Tatarstan.Quartz
{
    using Bars.B4.Utils;

    public interface IReminderResolution
    {
        bool CreateReminders(DynamicDictionary dict, out string message);
    }
}
