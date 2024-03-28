namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.States;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Заявка на доступ к экзамену 
    /// </summary>
    public class PersonRequestToExam : BaseImportableEntity, IStatefulEntity
    {
        /// <summary>
        /// Физ лицо
        /// </summary>
        public virtual Person Person { get; set; }

        /// <summary>
        /// Способ подачи заявления
        /// </summary>
        public virtual RequestSupplyMethod RequestSupplyMethod { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string RequestNum { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime RequestDate { get; set; }

        /// <summary>
        /// Время
        /// </summary>
        public virtual string RequestTime { get; set; }

        /// <summary>
        /// Файл заявления
        /// </summary>
        public virtual FileInfo RequestFile { get; set; }

		/// <summary>
		/// Файл согласия на обработку перс.данных
		/// </summary>
		public virtual FileInfo PersonalDataConsentFile { get; set; }

        /// <summary>
        /// Номер уведомления
        /// </summary>
        public virtual string NotificationNum { get; set; }

        /// <summary>
        /// Дата уведомления
        /// </summary>
        public virtual DateTime NotificationDate { get; set; }

        /// <summary>
        /// Отказ
        /// </summary>
        public virtual bool IsDenied { get; set; }

        /// <summary>
        /// Дата экзамена
        /// </summary>
        public virtual DateTime? ExamDate { get; set; }

        /// <summary>
        /// Время экзамена
        /// </summary>
        public virtual string ExamTime { get; set; }

        /// <summary>
        /// Количество набранных баллов
        /// </summary>
        public virtual decimal? CorrectAnswersPercent { get; set; }

        /// <summary>
        /// Номер протокола
        /// </summary>
        public virtual string ProtocolNum { get; set; }

        /// <summary>
        /// Дата протокола
        /// </summary>
        public virtual DateTime? ProtocolDate { get; set; }

        /// <summary>
        /// Файл протокола
        /// </summary>
        public virtual FileInfo ProtocolFile { get; set; }

        /// <summary>
        /// Номер уведомления (из блока "Результаты экзамена")
        /// </summary>
        public virtual string ResultNotificationNum { get; set; }

        /// <summary>
        /// Дата уведомления (из блока "Результаты экзамена")
        /// </summary>
        public virtual DateTime ResultNotificationDate { get; set; }

        /// <summary>
        /// Дата отправки почтой
        /// </summary>
        public virtual DateTime MailingDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }


        /// <summary>
        /// Не хранимое поле, нужно для того чтобы в реестре и в селект филдах отображать в виде "№ 1 от 12.12.2014"
        /// </summary>
        public virtual string Name { get; set; }
    }
}