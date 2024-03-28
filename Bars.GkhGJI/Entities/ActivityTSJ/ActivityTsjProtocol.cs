namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Протокол деятельности ТСЖ
    /// </summary>
    public class ActivityTsjProtocol : BaseGkhEntity
    {
        /// <summary>
        /// Деятельность ТСЖ
        /// </summary>
        public virtual ActivityTsj ActivityTsj { get; set; }

        /// <summary>
        /// Дата протокола
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер протокола
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Доля участников
        /// </summary>
        public virtual decimal? PercentageParticipant { get; set; }

        /// <summary>
        /// Дата голосования
        /// </summary>
        public virtual DateTime? VotesDate { get; set; }

        /// <summary>
        /// Количество голосов
        /// </summary>
        public virtual int? CountVotes { get; set; }

        /// <summary>
        /// Общее количество голосов
        /// </summary>
        public virtual int? GeneralCountVotes { get; set; }

        /// <summary>
        /// Вид протокола ТСЖ
        /// </summary>
        public virtual KindProtocolTsj KindProtocolTsj { get; set; }

        /// <summary>
        /// Файл бюллетень голосования
        /// </summary>
        public virtual FileInfo FileBulletin { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}