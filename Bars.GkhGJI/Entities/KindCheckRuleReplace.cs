namespace Bars.GkhGji.Entities
{
    using B4.DataAccess;
    using Enums;

    public class KindCheckRuleReplace : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код вида проверки
        /// </summary>
        public virtual TypeCheck Code { get; set; }

        /// <summary>
        /// Код правила
        /// </summary>
        public virtual string RuleCode { get; set; }
    }
}