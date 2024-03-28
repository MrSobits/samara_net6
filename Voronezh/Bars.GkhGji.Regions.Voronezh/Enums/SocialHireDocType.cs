using Bars.B4.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    public enum SocialHireDocType
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Паспорт гражданина СССР")]
        USSRPassport = 1,

        [Display("Свидетельство о рождении")]
        BirthCertificate = 2,

        [Display("Удостоверение личности офицера")]
        OfficerID = 3,

        [Display("Справка об освобождении из места лишения свободы")]
        PrisonDoc = 4,

        [Display("Паспорт Минморфлота")]
        MorFlotPassport = 5,

        [Display("Военный билет солдата (матроса, сержанта, старшины)")]
        MilitaryID = 6,

        [Display("Временное удостоверение, выданное взамен военного билета")]
        TemporaryMilitary = 7,

        [Display("Дипломатический паспорт гражданина Российской Федерации")]
        DiplomaticPassport = 8,

        [Display("Паспорт иностранного гражданина")]
        ForeignPassport = 9,

        [Display("Свидетельство о рассмотрении ходатайства о признании беженцем на территории Российской Федерации по существу")]
        RefugeeConsiderance = 10,

        [Display("Вид на жительство")]
        Residence = 11,
        
        [Display("Удостоверение беженца в Российской Федерации")]
        RefugeeDoc = 12,
        
        [Display("Временное удостоверение личности гражданина Российской Федерации")]
        TemporaryPassport = 13,
        
        [Display("Разрешение на временное проживание в Российской Федерации")]
        TemporaryResidence = 14,
        
        [Display("Свидетельство о предоставлении временного убежища на территории Российской Федерации")]
        TemporaryRefugee = 15,

        [Display("Паспорт гражданина Российской Федерации")]
        Passport = 16,
        
        [Display("Загранпаспорт гражданина Российской Федерации")]
        InternationalPassport = 17,
        
        [Display("Свидетельство о рождении, выданное уполномоченным органом иностранного государства")]
        InternationalBirthCertificate = 18,

        [Display("Удостоверение личности военнослужащего Российской Федерации")]
        SoldierPassport = 19,

        [Display("Паспорт моряка")]
        SailorPassport = 20,

        [Display("Военный билет офицера запаса")]
        ReserveOfficer = 21,
        
        [Display("Иные документы")]
        OtherDocuments = 22

    }
}
