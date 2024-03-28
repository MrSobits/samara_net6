namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using Castle.Windsor;

    public class TaskActionIsolatedItemService : ITaskActionIsolatedItemService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddItems(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var taskActionIsolatedItemIds = baseParams.Params.GetAs<long[]>("ids");

            var itemsToSave = taskActionIsolatedItemIds.Select(itemId => new TaskActionIsolatedItem
            {
                Task = new TaskActionIsolated { Id = documentId },
                Item = new SurveySubject { Id = itemId }
            });

            TransactionHelper.InsertInManyTransactions(this.Container, itemsToSave);

            return new BaseDataResult();
        }
    }
}
