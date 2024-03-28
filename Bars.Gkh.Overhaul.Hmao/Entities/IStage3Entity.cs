namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System.Collections.Generic;
    using Gkh.Entities;

    /// <summary>
    ///  интерфейс для записей 3-его этапа (нужно для того чтобы не писать разные реализации для 3 его этапа ДПКР и версии)
    /// </summary>
    public interface IStage3Entity 
    {
        long Id { get; set; }

        RealityObject RealityObject { get; set; }

        /// <summary>
        /// Плановый Год
        /// </summary>
        int Year { get; set; }

        /// <summary>
        /// Строка объектов общего имущества
        /// </summary>
       string CommonEstateObjects { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        int IndexNumber { get; set; }

        /// <summary>
        /// Балл
        /// </summary>
        decimal Point { get; set; }


        /// <summary>
        /// Значения критериев сортировки
        /// </summary>
         List<StoredPriorityParam> StoredCriteria { get; set; }

        /// <summary>
        /// Значения параметров очередности по баллам
        /// </summary>
        List<StoredPointParam> StoredPointParams { get; set; }
    }

    /// <summary>
    /// Значение параметра приоритета
    /// </summary>
    public sealed class StoredPriorityParam
    {
        /// <summary>
        /// Критерий
        /// </summary>
        public string Criterion { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get;set; }
    }

    /// <summary>
    /// Значение параметра приоритета по баллам
    /// </summary>
    public sealed class StoredPointParam
    {
        /// <summary>
        /// Код параметра очередности по баллам
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public decimal Value { get; set; }
    }
}