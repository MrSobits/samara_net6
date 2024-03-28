namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Обжалование постановления Роспотребнадзора
    /// </summary>
    public enum ResolutionRospotrebnadzorAppealed
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Заместитель руководителя инспекции")]
        SubHeadInspection = 20,

        [Display("Руководитель инспекции")]
        HeadInspection = 30,

        [Display("Суд")]
        Law = 40,

        [Display("Жилищная инсекция")]
        RealityObjInspection = 50,

        [Display("Роспотребнадзор")]
        Rospotrebnadzor = 60
    }
}