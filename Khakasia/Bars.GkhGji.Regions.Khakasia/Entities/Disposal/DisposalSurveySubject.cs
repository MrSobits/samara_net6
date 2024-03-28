﻿namespace Bars.GkhGji.Regions.Khakasia.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Khakasia.Enums;
    using Gkh.Entities;

    /// <summary>
    /// Предмет проверки для приказа 
    /// </summary>
    public class DisposalSurveySubject : BaseEntity
    {
        /// <summary>
        /// Приказ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Предмет проверки
        /// </summary>
        public virtual SurveySubject SurveySubject { get; set; }
    }
}