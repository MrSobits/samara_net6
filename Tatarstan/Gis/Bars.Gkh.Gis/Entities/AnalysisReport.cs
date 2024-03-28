using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.Gkh.Gis.Entities
{
    class AnalysisReport
    {
        /// <summary>
        /// Идентификатор квартиры
        /// </summary>
        public virtual long ApartmentId { get; set; }

        /// <summary>
        /// Расчетный месяц
        /// </summary>
        public virtual string CalcMonth { get; set; }

        /// <summary>
        /// Наименование услуги
        /// </summary>
        public virtual string Service { get; set; }

        /// <summary>
        /// Наименование поставщика
        /// </summary>
        public virtual string Supplier { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Дерево расчетов
        /// </summary>
        public virtual AnalysisReportNode ReportTree { get; set; }

        ///// <summary>
        ///// Сумма начисления
        ///// </summary>
        //public virtual decimal Charge { get; set; }
        ///// <summary>
        ///// Тариф
        ///// </summary>
        //public virtual decimal Tariff { get; set; }
        ///// <summary>
        ///// Расход
        ///// </summary>
        //public virtual decimal Consumption { get; set; }


    }

    class AnalysisReportNode
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Сумма начисления
        /// </summary>
        public virtual decimal Charge { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }


        /// <summary>
        /// Операция между потомками
        /// </summary>
        public virtual string ChildsOperation { get; set; }

        /// <summary>
        /// Левый потомок
        /// </summary>
        public virtual AnalysisReportNode Left { get; set; }

        /// <summary>
        /// Правый потомок
        /// </summary>
        public virtual AnalysisReportNode Right { get; set; }
        ///// <summary>
        ///// Единицы измерения
        ///// </summary>
        //public virtual string Measure { get; set; }
    }
}
