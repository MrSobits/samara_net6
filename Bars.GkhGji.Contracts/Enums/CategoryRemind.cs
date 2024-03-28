namespace Bars.GkhGji.Contracts.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Категория напоминания. Тут все в перемешку часть категорий относится к проверкам и пункт для Обращения.
    /// </summary>
    public enum CategoryReminder
    {
        [Display("Обработка обращения")]
        ExecutionStatemen = 0,

        [Display("Инспекционная проверка")]
        Inspection = 10,

        [Display("Проверка по обращению граждан")]
        CitizenStatement = 20,

        [Display("Плановая проверка юр. лиц")]
        PlanJuridicalPerson = 30,

        [Display("Поручение руководства")]
        DisposalHead = 40,

        [Display("Требование прокуратуры")]
        ProsecutorsClaim = 50,

        [Display("Постановление прокуратуры")]
        ProsecutorsResolution = 60,

        [Display("Проверка деятельности ТСЖ")]
        ActivityTsj = 70,

        [Display("Подготовка к отопительному сезону")]
        HeatingSeason = 80,

        [Display("Лицензирование")]
        Licensing = 85,

        [Display("Без основания")]
        Default = 150
    }
}