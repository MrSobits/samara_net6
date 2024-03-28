using Bars.B4;
using Bars.Gkh.Authentification;
using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    public class SubProgramCriteriasInterceptor : EmptyDomainInterceptor<SubProgramCriterias>
    {
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Действие, выполняемое до создания сущности 
        /// </summary>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<SubProgramCriterias> service, SubProgramCriterias entity)
        {
            entity.Operator = UserManager.GetActiveOperator();

            return Success();
        }

        /// <summary>
        /// Действие, выполняемое до изменения сущности 
        /// </summary>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<SubProgramCriterias> service, SubProgramCriterias entity)
        {
            entity.Operator = UserManager.GetActiveOperator();

            return Success();
        }
    }
}
