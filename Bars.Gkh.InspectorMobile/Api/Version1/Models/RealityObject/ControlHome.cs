namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.RealityObject
{
    using System;

    /// <summary>
    /// Управление домом
    /// </summary>
    public class ControlHome
    {
        /// <summary>
        /// Вид управления
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        /// Дата начала управления
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// Дата начала управления
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Тип договора
        /// </summary>
        public string ContractType { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public Organization Organization { get; set; }
    }
}