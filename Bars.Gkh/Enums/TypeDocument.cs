namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа
    /// </summary>
    public enum TypeDocument
    {
        [Display("Отсутствие задолженности по налогам")]
        LackOfTaxDebts = 10,

        [Display("Отсутствие кредиторской задолженности")]
        LackOfCreditorDebts = 20,

        [Display("Отсутствие предписаний ГЖИ")]
        LackOfRegulationsGji = 30,

        [Display("Непроведение ликвидации/неприостановление деятельности")]
        NoLiquidationNoActivity = 40,

        [Display("Сертификат соответствия стандартам качества")]
        CertificateConformance = 50,

        [Display("Учредительные документы")]
        ConstituentDocuments = 60,

        [Display("Свидетельство о постановке на учет в налоговый орган")]
        TestimonyToTaxAuthority = 70,

        [Display("Выписка из ЕГР юр.лиц/ИП")]
        ExtractFromEgr = 80,

        [Display("Документ, подтверждающий УСН")]
        DocumentConfirmUsn = 90,

        [Display("Разрешение на проведение работ по КР")]
        WorkPerminKr = 100,

        [Display("Справка из банка об отсутствии картотеки на счете")]
        BankReference = 110,

        [Display("Членство в общественных и профессиональных организациях")]
        MembershipOrganizations = 120
    }
}
