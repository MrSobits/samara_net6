namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Форма голосования
    /// </summary>
    public enum VoteForm
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Очная")]
        FaceToFace = 20,

        [Display("Заочная")]
        Absenctee = 30,

        [Display("Очно-заочная")]
        Both = 40
    }
}

