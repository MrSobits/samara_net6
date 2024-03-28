using System;
using Bars.B4.Modules.FileStorage;

namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Gkh.Entities;

    public class VersionRecord : BaseEntity, IStage3Entity
    {
        public virtual ProgramVersion ProgramVersion { get; set; }
        
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Плановый Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Строка объектов общего имущества
        /// </summary>
        public virtual string CommonEstateObjects { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public virtual int IndexNumber { get; set; }

        /// <summary>
        /// Балл
        /// </summary>
        public virtual decimal Point { get; set; }

        /// <summary>
        /// Изменялся ли год ремонта
        /// </summary>
        public virtual bool IsChangedYear { get; set; }

        /// <summary>
        /// Документ о переносе года ремонта
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Значения критериев сортировки
        /// </summary>
        public virtual List<StoredPriorityParam> StoredCriteria { get; set; }

        /// <summary>
        /// Значения параметров очередности по баллам
        /// </summary>
        public virtual List<StoredPointParam> StoredPointParams { get; set; }

        /// <summary>
        /// Была добавлена при актуализации "Добавить новые записи" - Навсякий случай перенесен с ХМАО
        /// </summary>
        public virtual bool IsAddedOnActualize { get; set; }

        /// <summary>
        /// Была добавлена при актуализации "Актуализировать год" - Навсякий случай перенесен с ХМАО
        /// </summary>
        public virtual bool IsChangedYearOnActualize { get; set; }

        /// <summary>
        /// Была добавлена при актуализации "Актуализировать сумму" - Навсякий случай перенесен с ХМАО
        /// </summary>
        public virtual bool IsChangeSumOnActualize { get; set; }

        /// <summary>
        /// Изменения записи
        /// </summary>
        public virtual string Changes { get; set; }
        public VersionRecord()
        {
            StoredCriteria = new List<StoredPriorityParam>();
            StoredPointParams = new List<StoredPointParam>();
        }
    }
}