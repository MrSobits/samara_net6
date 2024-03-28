namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Типовых ответы на вопросы проверочного листа.
    /// </summary>
    public class ControlListTypicalAnswer : BaseEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Уникальный идентификатор ТОР.
        /// </summary>
        public virtual Guid? TorId { get; set; }

        /// <summary>
        /// Ответ на вопрос проверочного листа.
        /// </summary>
        public virtual string Answer { get; set; }
       
    }
}
