namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Типы исполнения по ответу на обращения
    /// </summary>
    public class AppealAnswerExecutionType : BaseGkhEntity
    {
        /// <summary>
        /// Ответ по обращению граждан
        /// </summary>
        public virtual AppealCitsAnswer AppealCitsAnswer { get; set; }

        /// <summary>
        /// Тип исполнения
        /// </summary>
        public virtual AppealExecutionType AppealExecutionType { get; set; }        
    }
}