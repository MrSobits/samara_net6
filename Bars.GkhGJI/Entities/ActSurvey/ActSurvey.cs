namespace Bars.GkhGji.Entities
{
    using System.Collections.Generic;

    using Bars.GkhGji.Enums;

    /// <summary>
    /// Акт обследования
    /// </summary>
    public class ActSurvey : DocumentGji
    {
        /// <summary>
        /// Обследованная площадь
        /// </summary>
        public virtual decimal? Area { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public virtual string Flat { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Выводы по результату
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Факт обследования
        /// </summary>
        public virtual SurveyResult FactSurveyed { get; set; }

        // ToDo ГЖИ следующие поля необходимо выпилить после перехода на правила

        /// <summary>
        /// Список жилых домов акта (Используется при создании объекта)
        /// </summary>
        public virtual List<long> RealityObjectsList { get; set; }

        /// <summary>
        /// Список родительских документов (Используется при создании объекта)
        /// </summary>
        public virtual List<long> ParentDocumentsList { get; set; }
    }
}