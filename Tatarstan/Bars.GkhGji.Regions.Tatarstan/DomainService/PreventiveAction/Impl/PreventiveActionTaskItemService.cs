namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using Castle.Windsor;

    public class PreventiveActionTaskItemService : IPreventiveActionTaskItemService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddItems(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var preventiveActionItemIds = baseParams.Params.GetAs<long[]>("ids");

            var itemsToSave = preventiveActionItemIds.Select(itemId => new PreventiveActionTaskItem
            {
                Task = new PreventiveActionTask { Id = documentId },
                Item = new PreventiveActionItems { Id = itemId }
            });

            TransactionHelper.InsertInManyTransactions(this.Container, itemsToSave);

            return new BaseDataResult();
        }
    }
}