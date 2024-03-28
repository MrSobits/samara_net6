namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.RealityObject
{
    /// <summary>
    /// Конструктивные элементы дома
    /// </summary>
    public class StructuralElements
    {
        /// <summary>
        /// Конструктивный элемент
        /// </summary>
        public string StructuralElement { get; set; }
        
        /// <summary>
        /// Объект общего имущества
        /// </summary>
        public string Ooi { get; set; }
        
        /// <summary>
        /// Год установки
        /// </summary>
        public int YearInstallation { get; set; }

        /// <summary>
        /// Тип системы
        /// </summary>
        public string SystemType { get; set; }
        
        /// <summary>
        /// Износ
        /// </summary>
        public decimal Wear { get; set; }
        
        /// <summary>
        /// Объем
        /// </summary>
        public decimal Volume { get; set; }
        
        /// <summary>
        /// Объем
        /// </summary>
        public string Measure { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public string Condition { get; set; }
    }
}