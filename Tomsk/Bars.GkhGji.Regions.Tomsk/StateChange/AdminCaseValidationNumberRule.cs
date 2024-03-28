namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdminCaseValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id 
        {
            get { return "gji_tomsk_admincase_validation_rule"; }
        }

        public override string Name
        {
            get { return "Проверка возможности формирования номера административного дела Томска"; }
        }

        public override string TypeId
        {
            get { return "gji_document_admincase"; }
        }

        public override string Description
        {
            get { return "Данное правило проверяет формирование номера административного дела в соответствии с правилами Томска"; }
        }

        protected override ValidateResult Action(DocumentGji document)
        {
            var adminCase = document as AdministrativeCase;

            //если номер уже присвоен, то ничего не делаем
            //например, если руками присвоили номер
            if (adminCase != null && adminCase.DocumentNumber.IsEmpty())
            {
                var serviceAdminCase = Container.Resolve<IDomainService<AdministrativeCase>>();

                var query = serviceAdminCase.GetAll()
                    .Where(x => x.Inspection.Id == adminCase.Inspection.Id)
                    .Where(x => x.Id != adminCase.Id);

                var documentNumber = query
                    .OrderBy(x => x.DocumentSubNum)
                    .Select(x => x.DocumentNumber)
                    .FirstOrDefault();

                var maxSubNumber = query
                    .Select(x => x.DocumentSubNum)
                    .Max();

                //если в проверке нет административных дел, то пробуем взять номер у обращения связанного с проверкой
                if (documentNumber.IsEmpty())
                {
                    documentNumber =
                        Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                            .Where(x => x.Inspection.Id == adminCase.Inspection.Id)
                            .OrderBy(x => x.Id)
                            .Select(x => x.AppealCits.NumberGji)
                            .FirstOrDefault(x => x != null);
                }

                adminCase.DocumentSubNum = maxSubNumber.HasValue ? maxSubNumber.Value + 1 : 0;
                adminCase.DocumentNumber = documentNumber;

                if (adminCase.DocumentSubNum.Value > 0 && !adminCase.DocumentNumber.IsEmpty())
                {
                    adminCase.DocumentNumber += "/" + adminCase.DocumentSubNum;
                }
            }

            return ValidateResult.Yes();
        }
    }
}