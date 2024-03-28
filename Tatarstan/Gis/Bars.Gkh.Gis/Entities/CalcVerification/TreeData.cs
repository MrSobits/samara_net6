namespace Bars.KP60.Protocol.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Протокол расчета: узлы дерева расчета
    /// </summary>
    public class TreeData
    {
        /// <summary>
        /// Список слагаемых (слева направо)
        /// </summary>
        public List<TreeData> children { get; set; }

        /// <summary>
        /// Арифметическая операция
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Подсказка
        /// </summary>
        public string Hint { get; set; }
    }
}
