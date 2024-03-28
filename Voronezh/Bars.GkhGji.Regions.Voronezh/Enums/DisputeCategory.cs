namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид спора
    /// </summary>
    public enum DisputeCategory
    {
        /// <summary>
        /// Оспаривание ненормативных правовых актов
        /// </summary>
        [Display("Оспаривание ненормативных правовых актов")]
        DispNPA = 10,

        /// <summary>
        /// Оспаривание решений
        /// </summary>
        [Display("Оспаривание решений")]
        DispDecign = 20,

        /// <summary>
        /// Оспаривание действий (бездействий)
        /// </summary>
        [Display("Оспаривание действий (бездействий)")]
        DispActInact = 30,

        /// <summary>
        /// Оспаривание решений о привлечении к административной ответственности
        /// </summary>
        [Display("Оспаривание решений о привлечении к административной ответственности")]
        DispKOAP = 40,

        /// <summary>
        /// Исковое производство
        /// </summary>
        [Display("Исковое производство")]
        Lawsuit = 50,

        /// <summary>
        /// Иное
        /// </summary>
        [Display("Иное")]
        Other = 60

    }
}