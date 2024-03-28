namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Программы капитального ремонта (pkr.csv)
    /// </summary>
    public class PkrProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Вид программы
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 3. Уровень
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 4. Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 5. Код ОКТМО
        /// </summary>
        public string Oktmo { get; set; }

        /// <summary>
        /// 6. Месяц и год начала программы
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 7. Месяц и год окончания программы
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 8. Программа сформирована на основании региональной программы
        /// </summary>
        public long? IsFormedOnBaseRegPrograms { get; set; }

        /// <summary>
        /// 9. Цель программы
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// 10. Задачи программы
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// 11. Государственный заказчик (Координатор программы)
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? StateCustomer { get; set; }

        /// <summary>
        /// 12. Основной разработчик программы
        /// <para>"АО БАРС ГРУП"</para>
        /// </summary>
        public string MainDeveloper { get; } = "АО БАРС ГРУП";

        /// <summary>
        /// 13. Исполнитель программы
        /// </summary>
        public string Executor { get; set; }

        /// <summary>
        /// 14. Статус программы
        /// </summary>
        public int? State { get; set; }

        #region PKRDOC
        /// <summary>
        /// PKRDOC 1. Уникальный код документа КПР
        /// </summary>
        public long? DocId { get; set; }

        /// <summary>
        /// PKRDOC 3. Вид документа
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// PKRDOC 4. Наименование документа
        /// </summary>
        public string DocName { get; set; }

        /// <summary>
        /// PKRDOC 5. Номер документа
        /// </summary>
        public string DocNum { get; set; }

        /// <summary>
        /// PKRDOC 6. Дата документа
        /// </summary>
        public DateTime? DocDate { get; set; }

        /// <summary>
        /// PKRDOC 7. Орган, принявший документ
        /// </summary>
        public string AcceptedGoverment { get; set; }

        /// <summary>
        /// PKRDOC 8. Статус документа
        /// </summary>
        public int DocState { get; set; }
        #endregion

        #region PKRDOCFILES
        /// <summary>
        /// PKRDOCFILES 2. Идентификатор файла
        /// </summary>
        public FileInfo File { get; set; }
        #endregion
    }
}