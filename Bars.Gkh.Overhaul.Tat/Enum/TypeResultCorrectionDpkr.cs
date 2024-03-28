using Bars.B4.Utils;

namespace Bars.Gkh.Overhaul.Tat.Enum
{
    public enum TypeResultCorrectionDpkr
    {
        [Display("Запись новая")]
        New = 10,

        [Display("Запись существует в краткосрочке")]
        InShortTerm = 20,

        [Display("Запись существует в долгосрочке")]
        InLongTerm = 30,

        [Display("Запись удаляется из краткосрочки")]
        RemoveFromShortTerm = 40,

        [Display("Запись добавляется в краткосрочке")]
        AddInShortTerm = 50
    }
}
