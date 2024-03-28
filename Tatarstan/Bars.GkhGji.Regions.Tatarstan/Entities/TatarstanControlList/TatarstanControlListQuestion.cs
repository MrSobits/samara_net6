namespace Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList;

    /// <summary>
    /// Вопрос проверочного листа.
    /// </summary>
    public class TatarstanControlListQuestion : BaseEntity
    {
        /// <summary>
        /// Вопрос проверочного листа. todo notNull??
        /// </summary>
        public virtual string QuestionContent { get; set; }

        /// <summary>
        /// Вопрос проверочного листа.
        /// </summary>
        public virtual ControlListTypicalQuestion TypicalQuestion { get; set; }

        /// <summary>
        /// Ответ на вопрос проверочного листа.
        /// </summary>
        public virtual ControlListTypicalAnswer TypicalAnswer { get; set; }

        /// <summary>
        /// Шаблон проверочного листа.
        /// </summary>
        public virtual TatarstanControlList ControlList { get; set; }

        /// <summary>
        /// Гуид ЕРП
        /// </summary>
        public virtual string ErpGuid { get; set; }
    }
}
