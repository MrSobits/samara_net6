using Bars.B4;
using Bars.Gkh.Authentification;
using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    public class DPKRActualCriteriasInterceptor : EmptyDomainInterceptor<DPKRActualCriterias>
    {
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Действие, выполняемое до создания сущности 
        /// </summary>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<DPKRActualCriterias> service, DPKRActualCriterias entity)
        {
            if (entity.IsNumberApartments && entity.NumberApartmentsCondition == Enum.Condition.NotSet)
                return Failure("Не выбрано условие для количества квартир");

            if (entity.IsStructuralElementCount && entity.StructuralElementCountCondition == Enum.Condition.NotSet)
                return Failure("Не выбрано условие для количества КЭ");

            if (entity.IsYearRepair && entity.YearRepairCondition == Enum.Condition.NotSet)
                return Failure("Не выбрано условие для года последнего капитального ремонта");

            entity.Operator = UserManager.GetActiveOperator();

            return Success();
        }

        /// <summary>
        /// Действие, выполняемое до изменения сущности 
        /// </summary>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<DPKRActualCriterias> service, DPKRActualCriterias entity)
        {
            if (entity.IsNumberApartments && entity.NumberApartmentsCondition == Enum.Condition.NotSet)
                return Failure("Не выбрано условие для количества квартир");

            if (entity.IsStructuralElementCount && entity.StructuralElementCountCondition == Enum.Condition.NotSet)
                return Failure("Не выбрано условие для количества КЭ");

            if (entity.IsYearRepair && entity.YearRepairCondition == Enum.Condition.NotSet)
                return Failure("Не выбрано условие для года последнего капитального ремонта");

            entity.Operator = UserManager.GetActiveOperator();

            return Success();
        }
    }
}
