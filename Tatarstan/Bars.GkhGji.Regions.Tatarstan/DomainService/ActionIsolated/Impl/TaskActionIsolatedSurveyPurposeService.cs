namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using Castle.Windsor;

    public class TaskActionIsolatedSurveyPurposeService: ITaskActionIsolatedSurveyPurposeService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        
        /// <inheritdoc />
        public IDataResult AddPurposes(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var tasksIds = baseParams.Params.GetAs<long[]>("ids");
            var taskActionIsolatedSurveyPurposeDomain = this.Container.ResolveDomain<SurveyPurpose>();

            using (this.Container.Using(taskActionIsolatedSurveyPurposeDomain))
            {
                var tasksToSave = tasksIds.Select(id => new TaskActionIsolatedSurveyPurpose()
                {
                    TaskActionIsolated = new TaskActionIsolated() { Id = documentId },
                    SurveyPurpose = new SurveyPurpose() { Id = id }
                });

                TransactionHelper.InsertInManyTransactions(this.Container, tasksToSave);
            }

            return new BaseDataResult();
        }
    }
}