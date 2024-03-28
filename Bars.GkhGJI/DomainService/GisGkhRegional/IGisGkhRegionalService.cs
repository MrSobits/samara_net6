namespace Bars.GkhGji.DomainService.GisGkhRegional
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Сервис для получения региональных ранных при работе с ГИС ЖКХ
    /// </summary>
    public interface IGisGkhRegionalService
    {
        /// <summary>
        /// Получение предмета проверки при переоформлении лицензии
        /// </summary>
        /// <param name="disposal">Распоряжение</param>
        string GetObjectiveLicReissuance(Disposal disposal);

        /// <summary>
        /// Получение статей закона постановления
        /// </summary>
        /// <param name="resolution">Постановление</param>
        List<ArticleLawGji> GetResolutionArtLaw(Resolution resolution);

        Inspector GetAppealPerformerForGisGkh(AppealCits appeal);

        AppealAnswerType GetAppealAnswerTypeForGisGkh(AppealCits appeal);

        List<AppealCitsAnswer> GetAppCitAnswersForGisGkh(AppealCits appeal, AppealAnswerType closeType);

        List<AppealCitsAnswer> GetAppCitRollOverAnswersForGisGkh(AppealCits appeal);

        List<AppealCitsAnswer> GetAppCitRedirectAnswersForGisGkh(AppealCits appeal);

        DateTime GetDateOfAppointment(AppealCits appeal);

        TypeExecutantForGisGkh GetTypeExecutant(Resolution resolution);

        List<string> GetDisposalControlMeasures(Disposal disposal);

        List<string> GetDecisionControlMeasures(Decision decision);

        //string GetExaminationObjective(Disposal disposal, InspectionGji inspection);

        string GetFunctionRegistryNumber(bool licensecontrol);

        string FindExaminationPlace(InspectionGji inspection);

        void FindActRemovalAnnexes(DocumentGji docChildAct, out List<long> ids, out List<FileInfo> files, out List<string> gisGkhGuids);

        void SaveActRemovalAnnex(long id, string fileGuid);

        int FindActRemovalWitness(ActRemoval actRemoval, out string witnessesStr, out bool NotFamiliarize, out string FamiliarizedPerson);

        void GetActRemovalPeriods(ActRemoval actRemoval, out List<DateTime?> ActDatesStart, out List<DateTime?> ActDatesEnd);
    }
}