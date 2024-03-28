using Bars.B4.DataAccess;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Виды вопросов обращения
    /// </summary>
    public class AppealCitsQuestion : BaseEntity
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Вид вопроса
        /// </summary>
        public virtual QuestionKind QuestionKind { get; set; }
    }
}
