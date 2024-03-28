namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Предостережение ГЖИ
    /// </summary>
    public class WarningDoc : DocumentGji
    {
        /// <summary>
        /// Срок принятия мер о соблюдении требований
        /// </summary>
        public virtual DateTime? TakingDate { get; set; }

        /// <summary>
        /// Результат
        /// </summary>
        public virtual string ResultText { get; set; }

        /// <summary>
        /// Основание предостережения
        /// </summary>
        public virtual string BaseWarning { get; set; }

        /// <summary>
        /// Дата документа (Уведомление о направлении предостережения)
        /// </summary>
        public virtual DateTime? NcOutDate { get; set; }

        /// <summary>
        /// Номер документа (Уведомление о направлении предостережения)
        /// </summary>
        public virtual string NcOutNum { get; set; }
        /// <summary>
        /// Дата исходящего пиьма  (Уведомление о направлении предостережения)
        /// </summary>
        public virtual DateTime? NcOutDateLatter { get; set; }

        /// <summary>
        /// Номер исходящего письма  (Уведомление о направлении предостережения)
        /// </summary>
        public virtual string NcOutNumLatter { get; set; }

        /// <summary>
        /// Уведомление отправлено (Уведомление о направлении предостережения)
        /// </summary>
        public virtual YesNo NcOutSent { get; set; }

        /// <summary>
        /// Дата документа (Уведомление об устранении нарушений)
        /// </summary>
        public virtual DateTime? NcInDate { get; set; }

        /// <summary>
        /// Номер документа (Уведомление об устранении нарушений)
        /// </summary>
        public virtual string NcInNum { get; set; }
        /// <summary>
        /// Дата исходящего пиьма  (Уведомление об устранении нарушений)
        /// </summary>
        public virtual DateTime? NcInDateLatter { get; set; }

        /// <summary>
        /// Номер исходящего письма  (Уведомление об устранении нарушений)
        /// </summary>
        public virtual string NcInNumLatter { get; set; }

        /// <summary>
        /// Уведомление отправлено (Уведомление об устранении нарушений)
        /// </summary>
        public virtual YesNo NcInRecived { get; set; }

        /// <summary>
        /// Должностное лицо (ДЛ) вынесшее распоряжение
        /// </summary>
        public virtual Inspector Autor { get; set; }

        /// <summary>
        /// Ответственный за исполнение
        /// </summary>
        public virtual Inspector Executant { get; set; }

        /// <summary>
        /// Документ основания
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        public virtual FiasAddress CompilationPlace { get; set; }

        /// <summary>
        /// Получено возражение
        /// </summary>
        public virtual YesNo ObjectionReceived { get; set; }

        /// <summary>
        /// Дата начала проведения мероприятия
        /// </summary>
        public virtual DateTime? ActionStartDate { get; set; }

        /// <summary>
        /// Дата окончания проведения мероприятия
        /// </summary>
        public virtual DateTime? ActionEndDate { get; set; }
        
        /// <summary>
        /// Учетный номер предостережения в ЕРКНМ
        /// </summary>
        public virtual string ErknmRegistrationNumber { get; set; }

        /// <summary>
        /// Дата присвоения учетного номера / идентификатора ЕРКНМ
        /// </summary>
        public virtual DateTime? ErknmRegistrationDate { get; set; }
    }
}