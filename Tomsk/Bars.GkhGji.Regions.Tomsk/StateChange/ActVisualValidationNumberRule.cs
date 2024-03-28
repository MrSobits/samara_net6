namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;
    using Castle.Windsor;

    public class ActVisualValidationNumberRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "gji_tomsk_actvis_validation_rule"; }
        }

        public string Name 
        {
            get { return "Проверка возможности формирования номера акта визуального осмотра Томска"; }
        }

        public string TypeId 
        {
            get { return "gji_document_actvisual"; }
        }

        public string Description 
        {
            get { return "Данное правило проверяет формирование номера акта визуального осмотра в соответствии с правилами Томска"; }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is ActVisual)
            {
                var document = statefulEntity as ActVisual;

                if (document.ReturnSafe(x => x.Inspection.TypeBase) == TypeBase.CitizenStatement)
                {
                    var primaryAppealCits = Container.Resolve<IDomainService<PrimaryBaseStatementAppealCits>>();

                    var appealNumberGji = primaryAppealCits.GetAll()
                        .Where(x => x.BaseStatementAppealCits.Inspection.Id == document.Inspection.Id)
                        .Select(x => new
                        {
                            x.BaseStatementAppealCits.AppealCits.Id,
                            x.BaseStatementAppealCits.AppealCits.NumberGji
                        })
                        .FirstOrDefault(x => x.NumberGji != null);

                    if (appealNumberGji != null)
                    {
                        var baseStatementQuery = primaryAppealCits.GetAll()
                            .Where(x => x.BaseStatementAppealCits.AppealCits.Id == appealNumberGji.Id)
                            .Select(x => x.BaseStatementAppealCits.Inspection.Id);

                        var maxNum = Container.Resolve<IDomainService<ActVisual>>().GetAll()
                            .Where(y => baseStatementQuery.Any(x => x == y.Inspection.Id))
                            .Where(x => x.Id != document.Id)
                            .Select(x => x.DocumentNum)
                            .Max();

                        document.DocumentNum = maxNum.HasValue ? maxNum + 1 : 0;

                        document.DocumentNumber =
                            appealNumberGji.NumberGji +
                            (document.DocumentNum > 0
                                ? "/" + document.DocumentNum
                                : null);
                    }
                    else
                    {
                        ValidateResult.No("В проверке по обращению граждан отсутствует основное обращение");
                    }
                }
            }
            else
            {
                ValidateResult.No("Документ не является актом визуального осмотра");
            }

            return ValidateResult.Yes();
        }
    }
}