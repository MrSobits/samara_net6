namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System;
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Castle.Windsor;

    public class RequirementStateChangeRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Requirement> Requirement { get; set; }

        public string Id 
        {
            get { return "gji_requirement_validation_number"; }
        }

        public string Name 
        {
            get { return "Проверка возможности формирования номера требования Томска"; }
        }

        public string TypeId 
        {
            get { return "gji_requirement"; }
        }

        public string Description
        {
            get { return "Данное правило проверяет формирование номера требования в соответствии с правилами Томска"; }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is Requirement)
            {
                var req = statefulEntity as Requirement;

                if (req.Document.DocumentNumber.IsEmpty())
                {
                    return ValidateResult.No("У родительского документа отсутствует номер");
                }

                var disposalStageId = req.Document.Stage != null && req.Document.Stage.Parent != null
                                          ? req.Document.Stage.Parent.Id
                                          : req.Document.Stage.Id;

                var maxNum = Requirement.GetAll()
                    .Where(x => x.Document.Stage.Id == disposalStageId || x.Document.Stage.Parent.Id == disposalStageId)
                    .Select(x => x.DocumentNum)
                    .Max();

                req.DocumentNum = maxNum.HasValue ? maxNum.Value + 1 : 0;

                req.DocumentNumber = req.Document.DocumentNumber +
                                     (req.DocumentNum > 0 
                                        ? "/" + req.DocumentNum.Value 
                                        : null);
            }
            else
            {
                return ValidateResult.No("Объект не является требованием");
            }

            return ValidateResult.Yes();
        }
    }
}
