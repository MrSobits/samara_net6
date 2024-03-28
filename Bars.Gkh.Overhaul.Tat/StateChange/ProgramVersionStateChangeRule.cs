namespace Bars.Gkh.Overhaul.Tat.StateChange
{
    using System.Linq;
    using B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Entities;
    using Enum;

    public class ProgramVersionStateChangeRule : IRuleChangeStatus
    {
        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionDomain { get; set; }

        public IDomainService<SubsidyRecordVersionData> SubsidyVersionDomain { get; set; }

        public IDomainService<VersionRecordStage2> VersionStage2Domain { get; set; }

        public IDomainService<ShortProgramRealityObject> ShortProgramObjDomain { get; set; }

        public string Id
        {
            get { return "ovrhl_prog_version_validation_rule"; }
        }

        public string Name
        {
            get { return "Правило утверждения версии ДПКР"; }
        }

        public string TypeId
        {
            get { return "ovrhl_program_version"; }
        }

        public string Description
        {
            get { return "Правило утверждения версии ДПКР"; }
        }

        public IWindsorContainer Container { get; set; }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is ProgramVersion)
            {
                var programVersion = statefulEntity as ProgramVersion;
                var config = Container.GetGkhConfig<OverhaulTatConfig>();
                var startYear = config.ProgrammPeriodStart;
                var shortTermPeriod = config.ShortTermProgPeriod;

                var subsidyData = SubsidyVersionDomain.GetAll()
                        .Where(x => x.Version.Id == programVersion.Id);

                if (!subsidyData.Any())
                {
                    return ValidateResult.No("Перевод статуса невозможен, т.к. не пройдены все этапы формирования региональной программы.");
                }

                var versionRecords = VersionStage2Domain.GetAll()
                    .Where(x => x.Stage3Version.ProgramVersion.Id == programVersion.Id)
                    .Where(x => x.TypeCorrection != TypeCorrection.Done);

                if (versionRecords.Any())
                {
                    return ValidateResult.No("Перевод статуса невозможен, т.к. необходимо произвести корректировку очереди.");
                }

                var recordsForShortTerm = DpkrCorrectionDomain.GetAll()
                                                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == programVersion.Id)
                                                    .Where(x => x.PlanYear >= startYear && x.PlanYear < (startYear + shortTermPeriod));

                // потому что есть муниципальные образования, у которых нет краткосрочной программы
                if (!recordsForShortTerm.Any())
                {
                    return ValidateResult.Yes();
                }

                var correctionRecords = DpkrCorrectionDomain.GetAll()
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == programVersion.Id)
                    .Where(x => x.TypeResult == TypeResultCorrectionDpkr.RemoveFromShortTerm || x.TypeResult == TypeResultCorrectionDpkr.AddInShortTerm);

                if (correctionRecords.Any())
                {
                    return ValidateResult.No("Перевод статуса невозможен, т.к. не сформирована краткосрочная программа капитального ремонта после корректировки очереди.");
                }

                var shortProgramRecs = ShortProgramObjDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == programVersion.Id);

                /* Поскольку в татарстане могут не выгружат ьв краткосрочку, значит без нее над отоже давать переводить статус
                if (!shortProgramRecs.Any())
                {
                    return ValidateResult.No("Перевод статуса невозможен, т.к. не сформирована краткосрочная программа капитального ремонта.");
                }
                */

                if (shortProgramRecs.Any(x => x.TypeActuality != TypeActuality.Actual))
                {
                    return ValidateResult.No("Перевод статуса невозможен, т.к. региональная программа не актуализирована.");
                }

                if (shortProgramRecs.Any(x => !x.State.FinalState))
                {
                    return ValidateResult.No("Перевод статуса невозможен, т.к. не все объекты краткосрочной программы имеют конечный статус.");
                }
            }

            return ValidateResult.Yes();
        }
    }
}