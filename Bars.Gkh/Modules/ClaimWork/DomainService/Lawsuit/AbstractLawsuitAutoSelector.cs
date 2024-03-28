namespace Bars.Gkh.Modules.ClaimWork.DomainService.Lawsuit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using NCalc;
    using CourtType = Bars.Gkh.Enums.ClaimWork.CourtType;

    /// <summary>
    /// Абстрактный автоселектор значения искового значения
    /// </summary>
    public abstract class AbstractLawsuitAutoSelector : ILawsuitAutoSelector
    {
        private readonly IWindsorContainer container;
        private readonly IFormulaService formulaService;

        private readonly StateDutyPetition[] petitions;
        private readonly JurInstitution[] courts;
        private readonly JurInstitution[] ssps;

        private readonly Dictionary<long, HashSet<long>> roIds;

        /// <summary>
        /// Конфигурация модуля ПИР
        /// </summary>
        protected ClaimWorkConfig Section { get; private set; }

        /// <summary>
        /// .ctor
        /// </summary>
        protected AbstractLawsuitAutoSelector(IWindsorContainer container)
        {
            this.container = container;

            this.formulaService = container.Resolve<IFormulaService>();
            this.Section = container.Resolve<IGkhConfigProvider>().Get<ClaimWorkConfig>();

            var items = container.ResolveDomain<JurInstitution>().GetAll().ToArray();

            this.courts = items.Where(x => x.JurInstitutionType == JurInstitutionType.Court).ToArray();
            this.ssps = items.Where(x => x.JurInstitutionType == JurInstitutionType.Bailiffs).ToArray();

            this.roIds = container.ResolveDomain<JurInstitutionRealObj>().GetAll()
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    JiId = x.JurInstitution.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.JiId)
                .ToDictionary(x => x.Key, z => z.Select(x => x.RoId).ToHashSet());

            this.petitions = container.ResolveDomain<StateDutyPetition>().GetAll().ToArray();
        }

        /// <summary>
        /// попытаться проставить все значения
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        public bool TrySetAll(Lawsuit lawsuit)
        {
            if (!this.Validate(lawsuit)) return false;

            this.SelectTypeCourt(lawsuit);
            this.SelectJurSection(lawsuit);
            this.SelectSsp(lawsuit);
            this.SelectStateFee(lawsuit);

            return true;
        }

        /// <summary>
        /// Проставить тип суда
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        private void SelectTypeCourt(Lawsuit lawsuit)
        {
            if (this.Section.Common.General.CourtType == FillType.Manual)
                return;

            var debtorType = this.GetDebtorType(lawsuit);
            var reasonType = this.GetReasonType(lawsuit);
            var sum = lawsuit.DebtSum + lawsuit.PenaltyDebt;

            var isExaminationСasesСourt = this.Section.Common.CourtProceeding
                .Where(x => x.DebtorType == debtorType)
                .Where(x => x.MinClaimAmount <= sum && x.MaxClaimAmount >= sum)
                .FirstOrDefault(x => x.ReasonType == reasonType);

            if (isExaminationСasesСourt != null)
            {
                lawsuit.WhoConsidered = AbstractLawsuitAutoSelector.ConvertToLawsuitType(isExaminationСasesСourt.CourtType);
            }
        }

        /// <summary>
        /// Проставить судебный участок
        /// </summary>
        /// <param name="lawsuit"></param>
        private void SelectJurSection(Lawsuit lawsuit)
        {
            if (this.Section.Common.General.CourtType == FillType.Manual &&
                lawsuit.WhoConsidered == LawsuitConsiderationType.NotSet)
                {
                    lawsuit.WhoConsidered = LawsuitConsiderationType.WorldCourt;
                }

            if (this.Section.Common.General.CourtName == FillType.Manual)
            {
                return;
            }
         
            JurInstitution court;

            var ro = this.GetRobject(lawsuit);
            if (ro == null)
            {
                var roDetailsIds = this.GetRoIds(lawsuit);

                var courtIds = this.roIds.Where(x => x.Value.Any(z => roDetailsIds.Contains(z)))
                    .Select(x => x.Key)
                    .ToList();

                var magistrateCourts = this.courts
                    .Where(x => x.CourtType == AbstractLawsuitAutoSelector.ConvertToCourtType(lawsuit.WhoConsidered))
                    .Where(x => courtIds.Contains(x.Id))
                    .ToList();

                court = magistrateCourts.Count == 1 ? magistrateCourts.FirstOrDefault() : null;
            }
            else
            {
                court = this.courts
                    .Where(x => x.CourtType == AbstractLawsuitAutoSelector.ConvertToCourtType(lawsuit.WhoConsidered))
                    .FirstOrDefault(x => this.roIds.ContainsKey(x.Id) && this.roIds[x.Id].Contains(ro.Id));
            }
            if (lawsuit.JurInstitution != null)
            {
                if (court == null)
                {
                    return;
                }
                else
                {
                    lawsuit.JurInstitution = court;
                    lawsuit.JuridicalSectorMu = court.Return(x => x.Municipality);
                }
            }
        }

        /// <summary>
        /// проставить ссп (службу судебный приставов)
        /// </summary>
        /// <param name="lawsuit"></param>
        private void SelectSsp(Lawsuit lawsuit)
        {
            if (this.Section.Common.General.BailiffsService == FillType.Manual)
                return;

            var ro = this.GetRobject(lawsuit);

            if (ro == null) return;

            var ssp = this.ssps.FirstOrDefault(x => this.roIds.ContainsKey(x.Id) && this.roIds[x.Id].Contains(ro.Id));

            lawsuit.CbStationSsp = ssp;
        }

        /// <summary>
        /// проставить гос.пошлину
        /// </summary>
        /// <param name="lawsuit"></param>
        private void SelectStateFee(Lawsuit lawsuit)
        {
            if (this.Section.Common.General.StateFee == CalculationType.Manual)
                return;

            if (lawsuit.PetitionType == null) return;

            var stateDuty = this.petitions
                .Where(x => x.PetitionToCourtType.Id == lawsuit.PetitionType.Id)
                .Select(x => x.StateDuty)
                .FirstOrDefault(x => x.CourtType == AbstractLawsuitAutoSelector.ConvertToCourtType(lawsuit.WhoConsidered));

            if (stateDuty == null) return;

            lawsuit.Duty = this.CalculateDuty(stateDuty, lawsuit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected abstract bool Validate(Lawsuit lawsuit);

        /// <summary>
        /// Получить тип должника
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected abstract DebtorType GetDebtorType(Lawsuit lawsuit);

        /// <summary>
        /// Получить тип основания
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected abstract ReasonType GetReasonType(Lawsuit lawsuit);

        /// <summary>
        /// Получить жилой дом
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected abstract RealityObject GetRobject(Lawsuit lawsuit);

        /// <summary>
        /// Получить id домов из детализации ПИР по ЛС
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected abstract List<long> GetRoIds(Lawsuit lawsuit);

        /// <summary>
        /// Преобразовать "тип суда" в "кем рассмотрено заявление"
        /// </summary>
        /// <param name="courtType"></param>
        /// <returns></returns>
        private static LawsuitConsiderationType ConvertToLawsuitType(CourtType courtType)
        {
            switch (courtType)
            {
                case CourtType.Arbitration:
                    return LawsuitConsiderationType.ArbitrationCourt;
                case CourtType.District:
                    return LawsuitConsiderationType.RaionCourt;
                case CourtType.Magistrate:
                    return LawsuitConsiderationType.WorldCourt;
                default:
                    return LawsuitConsiderationType.NotSet;
            }
        }

        /// <summary>
        /// Преобразовать "кем рассмотрено заявление" в "тип суда"
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Enums.CourtType ConvertToCourtType(LawsuitConsiderationType type)
        {
            switch (type)
            {
                case LawsuitConsiderationType.ArbitrationCourt:
                    return Enums.CourtType.Arbitration;
                case LawsuitConsiderationType.RaionCourt:
                    return Enums.CourtType.District;
                case LawsuitConsiderationType.WorldCourt:
                    return Enums.CourtType.Magistrate;
            }

            return 0;
        }

        /// <summary>
        /// Посчитать гос.пошлину
        /// </summary>
        /// <param name="stateDuty"></param>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        private decimal CalculateDuty(StateDuty stateDuty, Lawsuit lawsuit)
        {
            var checkResult = this.formulaService.CheckFormula(stateDuty.Formula);

            //success = false - формула имеет ошибки
            if (!checkResult.Success)
            {
                return 0m;
            }

            var dict = new Dictionary<string, object>();

            foreach (var formulaParam in stateDuty.FormulaParameters)
            {
                var resolver = this.ResolveParameter(formulaParam.Code);
                resolver.SetDataProviders(new DynamicDictionary { { "Lawsuit", lawsuit } });
                dict[formulaParam.Name] = resolver.GetValue().ToDouble();
            }

            var expr = new Expression(stateDuty.Formula) { Parameters = dict };

            decimal result;

            try
            {
                result = expr.Evaluate().To<decimal>();
            }
            catch (Exception e)
            {
                throw new Exception("Не удалось расчитать выражение. Необходимо проверить корректность формулы.");
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IFormulaParameter ResolveParameter(string code)
        {
            return this.container.Resolve<IFormulaParameter>(code);
        }
    }
}