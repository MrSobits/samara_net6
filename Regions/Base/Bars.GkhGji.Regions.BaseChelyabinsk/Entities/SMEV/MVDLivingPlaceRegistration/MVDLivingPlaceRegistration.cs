namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using System.Collections.Generic;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Enums;
    using Bars.B4.Modules.FileStorage;

    public class MVDLivingPlaceRegistration : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// дата рождения
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string PatronymicName { get; set; }

        /// <summary>
        /// Адрес основной
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Файл с данными ответа 
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        public virtual string PassportSeries { get; set; }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public virtual string PassportNumber { get; set; }

        /// <summary>
        /// дата выдачи
        /// </summary>
        public virtual DateTime? IssueDate { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string AnswerInfo { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

    }
}
