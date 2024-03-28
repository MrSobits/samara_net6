namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Reminder
{
    using AppealCits;
    using GkhGji.Entities;

    public class ChelyabinskReminder : Reminder
    {
        /// <summary>
        /// Проверка ГЖИ
        /// </summary>
        public virtual AppealCitsExecutant AppealCitsExecutant { get; set; }
    }
}