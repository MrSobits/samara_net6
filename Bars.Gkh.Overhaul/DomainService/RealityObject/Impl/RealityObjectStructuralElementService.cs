namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Mapping;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using System.Collections.Generic;

    using Bars.Gkh.DomainService;

    /// <inheritdoc />
    public class RealityObjectStructuralElementService : IRealityObjectStructuralElementService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult CheckBlankFields(RealityObject ro)
        {
            var roStructElementDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var fieldRequirementDomain = this.Container.ResolveDomain<FieldRequirement>();

            using (this.Container.Using(roStructElementDomain, fieldRequirementDomain))
            {
                var structElements = roStructElementDomain.GetAll()
                    .Where(x => x.RealityObject.Id == ro.Id)
                    .ToList();

                if (!structElements.Any())
                {
                    return new BaseDataResult(false, "Ни одна конструктивная характеристика не добавлена.");
                }

                var entityDescriptor = MappingSchema.GetEntityDescriptor<RealityObjectStructuralElement>();

                List<StructElementProxy> structElementFields = new List<StructElementProxy>();

                foreach (var structElement in structElements)
                {
                     structElementFields.AddRange(entityDescriptor.GetPropertyMapDescriptors()
                         .Select(
                             x => new StructElementProxy
                             {
                                 PropertyName = x.Key.Name,
                                 Description = x.Value.Name,
                                 Value = x.Key.GetValue(structElement) != null
                             })
                         .ToList());
                }
                
                var structElementRequirements = fieldRequirementDomain.GetAll()
                    .Where(x => x.RequirementId.StartsWith("Gkh.RealityObject.StructElem.Field"))
                    .Select(x => x.RequirementId.Replace("Gkh.RealityObject.StructElem.Field.", string.Empty).Replace("_Rqrd", string.Empty))
                    .ToList();

                var blankFields = structElementFields
                    .Where(x => !x.Value)
                    .Where(x => structElementRequirements.Contains(x.PropertyName))
                    .Select(x => x.Description)
                    .ToList();

                if (blankFields.Any())
                {
                    var text = blankFields.AggregateWithSeparator(x => x, ", ");

                    return new BaseDataResult(false, $"Заполнены не все обязательные поля карточки дома: {text}");
                }

                return new BaseDataResult();
            }
        }

        private class StructElementProxy
        {
            public string PropertyName { get; set; }

            public string Description { get; set; }

            public bool Value { get; set; }
        }
    }
}