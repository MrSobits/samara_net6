namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;

    /// <summary>
    /// Запись в версии программы
    /// </summary>
    public class VersionRecord : BaseImportableEntity, IStage3Entity
    {
        /// <summary>
        /// Версия программы
        /// </summary>
        public virtual ProgramVersion ProgramVersion { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Плановый Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Рассчетный год
        /// </summary>
        public virtual int YearCalculated { get; set; }

        /// <summary>
        /// Год зафиксирован
        /// </summary>
        public virtual bool FixedYear { get; set; }

        /// <summary>
        /// Строка объектов общего имущества
        /// </summary>
        public virtual string CommonEstateObjects { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Изменялся ли год ремонта
        /// </summary>
        public virtual bool IsChangedYear { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public virtual int IndexNumber { get; set; }

        /// <summary>
        /// Балл
        /// </summary>
        public virtual decimal Point { get; set; }

        /// <summary>
        /// Значения критериев сортировки
        /// </summary>
        public virtual List<StoredPriorityParam> StoredCriteria { get; set; }

        /// <summary>
        /// Значения параметров очередности по баллам
        /// </summary>
        public virtual List<StoredPointParam> StoredPointParams { get; set; }

        /// <summary>
        /// Была ли ручная корректировка записи
        /// </summary>
        public virtual bool IsManuallyCorrect { get; set; }

        /// <summary>
        /// Тип основания изменения записи ДПКР
        /// </summary>
        public virtual ChangeBasisType ChangeBasisType { get; set; }

        /// <summary>
        /// Была добавлена при актуализации "Добавить новые записи"
        /// </summary>
        public virtual bool IsAddedOnActualize { get; set; }

        /// <summary>
        /// Была добавлена при актуализации "Актуализировать год"
        /// </summary>
        public virtual bool IsChangedYearOnActualize { get; set; }

        /// <summary>
        /// Была добавлена при актуализации "Актуализировать сумму"
        /// </summary>
        public virtual bool IsChangeSumOnActualize { get; set; }

        /// <summary>
        /// Была добавлена в результате разделения
        /// </summary>
        public virtual bool IsDividedRec { get; set; }

        /// <summary>
        /// Опубликованный год (только для отщепенцев!!!!)
        /// </summary>
        public virtual int PublishYearForDividedRec { get; set; }

        /// <summary>
        /// Изменения записи
        /// </summary>
        public virtual string Changes { get; set; }

        /// <summary>
        /// Изменения записи
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// Показывать в ДПКР
        /// </summary>
        public virtual bool Show { get; set; }

        /// <summary>
        /// Показывать в ДПКР
        /// </summary>
        public virtual bool SubProgram { get; set; }
        
        /// Опубликованный год изменен
        /// </summary>
        public virtual bool IsChangedPublishYear { get; set; }

        /// <summary>
        /// Код работы
        /// </summary>
        public virtual string WorkCode { get; set; }

        public VersionRecord()
        {
            StoredCriteria = new List<StoredPriorityParam>();
            StoredPointParams = new List<StoredPointParam>();
        }
    }
}