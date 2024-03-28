using Bars.B4.DataAccess;

namespace Bars.GkhGji.Contracts.Reminder
{
    
    /// <summary>
    /// Интерфейс для выполнения каких либо действий для создания Напоминий для определенного типа
    /// </summary>
    public interface IReminderRule
    {

        /// <summary>
        /// Id вида напоминания
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Создать напоминимние на основание какогото объеката
        /// В реализации просто IEntity проверяется если entity is InspectionGji то делается определенные действя при создании напоминания  
        /// </summary>
        void Create(IEntity entity);


    }
}
