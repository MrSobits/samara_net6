namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Состав трудовых ресурсов
    /// </summary>
    public class BuilderWorkforce : BaseGkhEntity
    {
        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Builder Builder { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }   
        
        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Наименование документа подтверждающего квалификацию
        /// </summary>
        public virtual string DocumentQualification { get; set; }

        /// <summary>
        /// Дата приема на работу
        /// </summary>
        public virtual DateTime? EmploymentDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }


        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string Fio { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string Position { get; set; }

        /// <summary>
        /// Специальность
        /// </summary>
        public virtual Specialty Specialty { get; set; }

        /// <summary>
        /// Учебное заведение
        /// </summary>
        public virtual Institutions Institutions { get; set; }
    }
}
