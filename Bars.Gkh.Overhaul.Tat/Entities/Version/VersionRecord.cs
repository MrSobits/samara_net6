namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Gkh.Entities;

    public class VersionRecord : BaseEntity
    {
        public virtual ProgramVersion ProgramVersion { get; set; }
        
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Плановый Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// год зафиксирован
        /// </summary>
        public virtual bool FixedYear { get; set; }

        /// <summary>
        /// Скорректированный год
        /// </summary>
        public virtual int CorrectYear { get; set; }

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
        /// тип записи ДПКР
        /// </summary>
        public virtual TypeDpkrRecord TypeDpkrRecord { get; set; }
        
        /// <summary>
        /// Не хранимое поле нужное для вычисления баллов при актуализации очередности
        /// </summary>
        public virtual int NeedOverhaul { get; set; }


        /// <summary>
        /// Значения критериев сортировки
        /// </summary>
        public virtual List<StoredPriorityParam> StoredCriteria { get; set; }

        public VersionRecord()
        {
            StoredCriteria = new List<StoredPriorityParam>();
        }
    }
}