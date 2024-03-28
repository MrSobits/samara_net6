namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Ответ на вопрос проверочного листа
    /// </summary>
    public class ActCheckControlListAnswer : BaseEntity
    {
        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }

        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual ControlListQuestion ControlListQuestion { get; set; }
        
        /// <summary>
        /// Вопрос
        /// </summary>
        public virtual string Question { get; set; }

        /// <summary>
        /// НПА
        /// </summary>
        public virtual string NpdName { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual YesNoNotApplicable YesNoNotApplicable { get; set; }

        /// <summary>
        /// Примечание 
        /// </summary>
        public virtual string Description { get; set; }
    }
}