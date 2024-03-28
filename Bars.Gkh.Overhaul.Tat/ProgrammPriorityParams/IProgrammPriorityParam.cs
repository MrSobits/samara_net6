namespace Bars.Gkh.Overhaul.Tat.ProgrammPriorityParams
{
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>
    /// Интерфейс для получения значения свойства приоритезации
    /// </summary>
    public interface IProgrammPriorityParam
    {
        /// <summary>
        /// Порядок сортировки
        /// </summary>
        bool Asc { get; }

        /// <summary>
        /// Понятное имя свойства
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Код свойства
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Функция получения значения своства
        /// </summary>
        decimal GetValue(RealityObjectStructuralElementInProgrammStage3 stage3); 
    }
}