using Bars.B4.DataAccess;
using Bars.B4.Modules.Security;
using Bars.Gkh.Regions.Tatarstan.Enums;
using System;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Regions.Tatarstan.Enums.Administration;

namespace Bars.Gkh.Regions.Tatarstan.Entities.Administration
{
    /// <summary>
    /// Лог интеграции данных по тарифам
    /// </summary>
    public class TariffDataIntegrationLog : BaseEntity
    {
        /// <summary>
        /// Метод интеграции данных по тарифам
        /// </summary>
        public virtual TariffDataIntegrationMethod TariffDataIntegrationMethod { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Время запуска метода
        /// </summary>
        public virtual DateTime StartMethodTime { get; set; }

        /// <summary>
        /// Параметры
        /// </summary>
        public virtual string Parameters { get; set; }

        /// <summary>
        /// Статус выполнения
        /// </summary>
        public virtual ExecutionStatus ExecutionStatus { get; set; }

        /// <summary>
        /// Лог операции
        /// </summary>
        public virtual FileInfo LogFile { get; set; }
    }
}