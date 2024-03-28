using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.States;
using Bars.Gkh.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    public class VDGOViolators : BaseEntity
    {
        /// <summary>
        /// ВДГО
        /// </summary>
        public virtual Contragent Contragent { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public virtual RealityObject Address { get; set; }
        /// <summary>
        /// УК
        /// </summary>
        public virtual Contragent MinOrgContragent { get; set; }
        /// <summary>
        /// Номер уведомления
        /// </summary>
        public virtual int NotificationNumber { get; set; }
        /// <summary>
        /// Дата уведомления
        /// </summary>
        public virtual DateTime NotificationDate { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// Электронная почта
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string FIO { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public virtual string PhoneNumber { get; set; }
        /// <summary>
        /// Дата исполнения
        /// </summary>
        public virtual DateTime DateExecution { get; set; }
        /// <summary>
        /// Отметка об исполнении
        /// </summary>
        public virtual bool MarkOfExecution { get; set; }
        /// <summary>
        /// Файл исполнения
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Файл уведомления
        /// </summary>
        public virtual FileInfo NotificationFile { get; set; }

        /// <summary>
        /// Отметка отправки сообщения
        /// </summary>
        public virtual bool MarkOfMessage { get; set; }
    }
}
