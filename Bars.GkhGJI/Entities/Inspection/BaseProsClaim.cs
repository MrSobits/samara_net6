namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Основание требование прокуратуры ГЖИ
    /// </summary>
#warning если ошибок не возникнет удалить поле ProsClaimDate (PROSCLAIM_DATE) из таблицы бд
    public class BaseProsClaim : InspectionGji
    {
        /// <summary>
        /// ДЛ, вынесшее требование
        /// </summary>
        public virtual string IssuedClaim { get; set; }

        /// <summary>
        /// Дата проверки
        /// </summary>
        public virtual DateTime? ProsClaimDateCheck { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Описание документа
        /// </summary>
        public virtual string DocumentDescription { get; set; }

        /// <summary>
        /// Тип основания проверки по требованию прокуратуры
        /// </summary>
        public virtual TypeBaseProsClaim TypeBaseProsClaim { get; set; }

        /// <summary>
        /// Форма проверки ЮЛ
        /// </summary>
        public virtual TypeFormInspection TypeForm { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Результаты отправлены в прокуратуру
        /// </summary>
        public virtual bool IsResultSent { get; set; }

        /// <summary>
        /// ИНН физ./долж. лица
        /// </summary>
        public virtual string Inn { get; set; }
    }
}