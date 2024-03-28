namespace Bars.Gkh.ViewModel.Licensing
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Licensing;
    using Bars.Gkh.Enums.Licensing;

    /// <summary>
    /// Представление для <see cref="GovernmenServiceDetail"/>
    /// </summary>
    public class GovernmenServiceDetailViewModel : BaseViewModel<GovernmenServiceDetail>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<GovernmenServiceDetail> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var type = baseParams.Params.GetAs<ServiceDetailSectionType>("type");

            var data = domainService.GetAll()
                .Where(x => x.FormGovernmentService.Id == id && x.DetailGroup.ServiceDetailSectionType == type)
                .OrderBy(x => x.DetailGroup.RowNumber)
                .ToList();

            var listResult = new List<Descriptor>();

            // здесь мы генерируем данные для того, чтобы потом в UI отрисовать правильно
            // где-то есть необходимость обернуть в fieldset данные
            GroupDescriptor groupDescriptor = null;
            foreach (var governmenServiceDetail in data)
            {
                if (governmenServiceDetail.DetailGroup.GroupName.IsNotEmpty())
                {
                    if (governmenServiceDetail.DetailGroup.GroupName != groupDescriptor?.DisplayValue)
                    {
                        groupDescriptor = null;
                    }

                    if (groupDescriptor.IsNull())
                    {
                        groupDescriptor = new GroupDescriptor
                        {
                            DisplayValue = governmenServiceDetail.DetailGroup.GroupName
                        };

                        listResult.Add(groupDescriptor);
                    }

                    groupDescriptor.InnerDescriptors.Add(this.ParseItem(governmenServiceDetail));
                }
                else
                {
                    groupDescriptor = null;
                    listResult.Add(this.ParseItem(governmenServiceDetail));
                }
            }

            return new BaseDataResult(new
            {
                ServiceDetailSectionType = type,
                Descriptors = listResult
            });
        }

        private Descriptor ParseItem(GovernmenServiceDetail governmenServiceDetail)
        {
            return new Descriptor
            {
                Id = governmenServiceDetail.Id,
                Value = governmenServiceDetail.Value,
                DisplayValue = governmenServiceDetail.DetailGroup.Name,
                ServiceDetailSectionType = governmenServiceDetail.DetailGroup.ServiceDetailSectionType
            };
        }

        private class Descriptor
        {
            public long Id { get; set; }

            public decimal? Value { get; set; }

            public string DisplayValue { get; set; }

            public ServiceDetailSectionType ServiceDetailSectionType { get; set; }
        }

        private class GroupDescriptor : Descriptor
        {
            public GroupDescriptor()
            {
                this.InnerDescriptors = new List<Descriptor>();
            }

            public IList<Descriptor> InnerDescriptors { get; }
        }
    }
}