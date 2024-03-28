namespace Bars.GkhCr.Entities
{
    using System;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Лот конкурса
    /// </summary>
    public class CompetitionLot : BaseImportableEntity
    {
        /// <summary>
        /// Конкурс
        /// </summary>
        public virtual Competition Competition { get; set; }

        /// <summary>
        /// Номер лота
        /// </summary>
        public virtual int LotNumber { get; set; }

        /// <summary>
        /// Начальная цена
        /// </summary>
        public virtual decimal StartingPrice { get; set; }

        /// <summary>
        /// Предмет договора
        /// </summary>
        public virtual string Subject { get; set; }

        //хранимые поля вкладки "договор"

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string ContractNumber { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime? ContractDate { get; set; }

        /// <summary>
        /// Фактическая цена договора
        /// </summary>
        public virtual decimal? ContractFactPrice { get; set; }

        /// <summary>
        /// Файд договора
        /// </summary>
        public virtual FileInfo ContractFile { get; set; }
    }
}