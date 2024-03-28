namespace Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams
{
    using Bars.Gkh.Overhaul.Hmao.Entities;

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
        decimal GetValue(IStage3Entity stage3); 
    }
}