﻿namespace Bars.GkhGji.Regions.Tula.Entities.AppealCits
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Entities;

    public class CheckTimeChange : BaseEntity
    {
        /// <summary>
        /// Обращение
        /// </summary>
        public virtual GkhGji.Entities.AppealCits AppealCits { get; set; } 

        /// <summary>
        /// Старый срок
        /// </summary>
        public virtual DateTime? OldValue { get; set; }

        /// <summary>
        /// Новый срок
        /// </summary>
        public virtual DateTime? NewValue { get; set; }

        /// <summary>
        /// Пользователь, поменявший срок
        /// </summary>
        public virtual User User { get; set; }
    }
}
