namespace Bars.GkhCr.StateChange
{
	using Bars.B4;
	using Bars.B4.Modules.States;
	using Bars.GkhCr.Entities;
	using Bars.GkhCr.Localizers;
	using Castle.Windsor;
	using System.Linq;

    public class ProtocolCrValidationRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get
            {
                return "ProtocolCrValidationRule";
            }
        }

        public string Name
        {
            get
            {
                return "Проверка возможности перевода статуса у Объекта КР (по протоколам)";
            }
        }

        public string TypeId
        {
            get
            {
                return "cr_object";
            }
        }

        public string Description
        {
            get
            {
                return "Блокирует смену статуса, если у протокол КР \"Акт сверки данных о расходах\" нет в наличии документа и при отсутсвии данных в поле \"Сумма Акта сверки данных о расходах.\"";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var entity = statefulEntity as ObjectCr;

            if (entity == null)
            {
                return ValidateResult.No("Внутренняя ошибка.");
            }

            var protocolCrs =
                this.Container.Resolve<IDomainService<ProtocolCr>>()
                         .GetAll()
                         .Where(x => x.ObjectCr.Id == entity.Id && x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActAuditDataExpenseKey)
                         .Select(x => new { x.SumActVerificationOfCosts, x.File })
                         .ToArray();
            if (protocolCrs.Length == 0)
            {
                return ValidateResult.Yes();
            }

            if (protocolCrs.Any(protocolCr => protocolCr.File == null || protocolCr.SumActVerificationOfCosts == null))
            {
               return ValidateResult.No("Для протоколов с типом \"Акт сверки данных о расходах\", обязательны поля \"Сумма Акта сверки данных о расходах\" и \"Файл\"");
            }

            return ValidateResult.Yes();
        }
    }
}
