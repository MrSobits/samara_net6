namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    public enum ResolutionAppealed
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
        RealityObjInspection = 50
    }
}