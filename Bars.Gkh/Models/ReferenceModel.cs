namespace Bars.Gkh.Models
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// DTO для передачи ReferenceModel
    /// </summary>
    public class ReferenceModel
    {
        /// <summary>
        /// Идентификатор подписываемого блока
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Алгоритмы преобразования XML
        /// </summary>
        public TransformAlgorithm[] TransformAlgorithms { get; set; }

        /// <summary>
        /// Метод вычисления хэша
        /// </summary>
        public string HashMethod { get; set; }
    }
}