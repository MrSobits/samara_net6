namespace Bars.Gkh.Regions.Tyumen.DomainServices.Suggestions.Impl
{
    using System;
    using System.Linq;
    using B4;
    using Castle.Windsor;
    using Entities.Suggestion;
    using Enums;

    public class SuggestionCommentService : ISuggestionCommentService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<SuggestionComment> ServiceComment { get; set; }

        public IDomainService<Transition> ServiceTransition { get; set; }

        public IDataResult ApplyExecutor(BaseParams baseParams)
        {
            var commentId = baseParams.Params.GetAs<long>("commentId");
            var executorType = baseParams.Params.GetAs<ExecutorType>("executorType");

            var comment = this.ServiceComment.GetAll().First(x => x.Id == commentId);

            try
            {
                comment.ExecutorZonalInspection = null;
                comment.ExecutorManagingOrganization = null;
                comment.ExecutorMunicipality = null;
                comment.ExecutorCrFund = null;

                switch (executorType)
                {
                    case ExecutorType.Gji:
                        comment.ExecutorZonalInspection = comment.GetExecutorZonalInspection();
                        return new BaseDataResult(comment.ExecutorZonalInspection.Name);
                    case ExecutorType.Mo:
                        comment.ExecutorManagingOrganization = comment.GetExecutorManagingOrganization();
                        if (comment.ExecutorManagingOrganization == null)
                        {
                            return new BaseDataResult(false, "Данный дом не обслуживается управляющей компанией. Выберите другого исполнителя");
                        }
                        else
                        {
                            return new BaseDataResult(comment.ExecutorManagingOrganization.Contragent.Name);
                        }
                        
                    case ExecutorType.Mu:
                        comment.ExecutorMunicipality = comment.CitizenSuggestion.RealityObject.Municipality;
                        return new BaseDataResult(comment.ExecutorMunicipality.Name);
                    case ExecutorType.CrFund:
                        comment.ExecutorCrFund = comment.GetExecutorExecutorCrFund();
                        return new BaseDataResult(comment.ExecutorCrFund.Name);
                }

                throw new ArgumentOutOfRangeException("executorType");
            }
            finally
            {
                this.ServiceComment.Update(comment);
            }
        }
    }
}