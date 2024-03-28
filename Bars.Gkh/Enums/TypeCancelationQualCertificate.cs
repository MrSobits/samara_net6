namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип отмены квалификационного аттестата
    /// </summary>
    public enum TypeCancelationQualCertificate
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Дисквалификация должностного лица")]
        Disqualification = 10,

        [Display("Использование подложных документов")]
        FalsifyingDocuments = 20,

        [Display("Вынесение приговора суда за преступления")]
        CourtVerdict = 30,

        [Display("Окончание действия аттестата")]
        EndOfTime = 40
    }
}
