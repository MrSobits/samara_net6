using Bars.B4.Utils;

namespace Bars.GkhGji.Enums
{
    /// <summary>
    /// Тип корреспондента
    /// </summary>
    public enum TypeCorrespondent
    {
        [Display("Гражданин")]
        CitizenHe = 10,

        [Display("Гражданка")]
        CitizenShe = 20,

        [Display("Коллективное обращение")]
        CitizenThey = 30,

        [Display("Индивидуальный предприниматель")]
        IndividualEntrepreneur = 40,

        [Display("Юридическое лицо")]
        LegalEntity = 50,

        [Display("Орган государственной власти")]
        PublicAuthorities = 60,

        [Display("Орган местного самоуправления")]
        LocalAuthorities = 70,

        [Display("Средство массовой информации")]
        MassMedia = 80,

        [Display("Прокуратура")]
        Prosecutor = 90
    }
}