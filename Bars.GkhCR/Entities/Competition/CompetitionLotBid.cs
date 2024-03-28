namespace Bars.GkhCr.Entities
{
    using System;

    using Bars.Gkh.Entities;

    using Gkh.Entities;

    /// <summary>
    /// Заявка лота
    /// </summary>
    public class CompetitionLotBid : BaseImportableEntity
    {
        /// <summary>
        /// Лот
        /// </summary>
        public virtual CompetitionLot Lot { get; set; }

        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Builder Builder { get; set; }

        /// <summary>
        /// Дата поступления
        /// </summary>
        public virtual DateTime IncomeDate { get; set; }

        /// <summary>
        /// Количество баллов
        /// </summary>
        public virtual Decimal? Points { get; set; }

        /// <summary>
        /// Цена заявки (без НДС)
        /// </summary>
        public virtual Decimal? Price { get; set; }

        /// <summary>
        /// Цена заявки (с НДС)
        /// </summary>
        public virtual Decimal? PriceNds { get; set; }

        /// <summary>
        /// Является победителем
        /// </summary>
        public virtual bool IsWinner { get; set; }
    }
}