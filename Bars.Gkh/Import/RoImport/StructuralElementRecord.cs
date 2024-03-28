namespace Bars.Gkh.Import.RoImport
{
    public class StructuralElementRecord
    {
        /// <summary>
        /// Код Конструктивного элемента
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public string Volume { get; set; }

        /// <summary>
        /// Год последнего ремонта
        /// </summary>
        public string LastOverhaulYear { get; set; }

        /// <summary>
        /// Износ
        /// </summary>
        public string Wearout { get; set; } 
    }
}