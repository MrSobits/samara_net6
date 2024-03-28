namespace Bars.GkhGji.Regions.Smolensk.Entities
{
    using System;
    using Bars.Gkh.Enums;

    using GkhGji.Entities;

    public class ActCheckSmol : ActCheck
    {
        /// <summary>
        /// Выявлены или нет нарушения - Это признак по всему акту а не по конкретному дому, Не путать с ActCheckRealityObject.Description 
        /// </summary>
        public virtual YesNoNotSet HaveViolation { get; set; }

        /// <summary>
        /// Дата уведомления о проверке
        /// </summary>
        public virtual DateTime? DateNotification { get; set; }

        /// <summary>
        /// Номер уведомления о проверке
        /// </summary>
        public virtual string NumberNotification { get; set; }
    }
}
