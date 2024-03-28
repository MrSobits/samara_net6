namespace Bars.GkhGji.Report.StatisticsAppealsCits
{
    internal sealed class Record
    {
        /// <summary>
        /// количество обращений
        /// </summary>
        public int CountAppealCits { get; set; }

        /// <summary>
        /// Принято на личном приеме
        /// </summary>
        public int AcceptedPersonalAppointment { get; set; }

        /// <summary>
        /// Руководством
        /// </summary>
        public int Leadership { get; set; }

        /// <summary>
        /// По видео конференции
        /// </summary>
        public int VideoConference { get; set; }

        /// <summary>
        /// Письменных обращений
        /// </summary>
        public int WrittenAppeal { get; set; }

        /// <summary>
        /// Доложено руководству
        /// </summary>
        public int ReportedLeadership { get; set; }

        /// <summary>
        /// ВзятоНаКонтроль
        /// </summary>
        public int TakenOnControl { get; set; }

        /// <summary>
        /// Проверено с выездом на место
        /// </summary>
        public int VerifiedOnSpot { get; set; }

        /// <summary>
        /// Нарушения прав заявителей
        /// </summary>
        public int ViolApplicantRight { get; set; }

        /// <summary>
        /// Наказано должн лиц
        /// </summary>
        public int OfficialsPunished { get; set; }

        /// <summary>
        /// Через интернет приемную
        /// </summary>
        public int ThroughInternet { get; set; }
    }
}
