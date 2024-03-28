using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Hmao
{
    using System.Linq;

    using B4;
    using B4.DataAccess;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;
    using Gkh.Entities.RealEstateType;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
            
        }

        public override IModuleDependencies Init()
        {
            var typeWorkCrVersionDomain = Container.Resolve<IDomainService<TypeWorkCrVersionStage1>>();
            References.Add(new EntityReference
            {
                BaseEntity = typeof(TypeWorkCr),
                DeleteAnyDependences = id => typeWorkCrVersionDomain.GetAll()
                    .Where(x => x.TypeWorkCr.Id == id)
                    .Select(x => x.Id).AsEnumerable()
                    .ForEach(x => typeWorkCrVersionDomain.Delete(x))
            });

            var realEstateTypeRealityObjectDomain = Container.Resolve<IDomainService<RealEstateTypeRealityObject>>();
            References.Add(new EntityReference
            {
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id => realEstateTypeRealityObjectDomain.GetAll()
                    .Where(x => x.RealityObject.Id == id)
                    .Select(x => x.Id).AsEnumerable()
                    .ForEach(x => realEstateTypeRealityObjectDomain.Delete(x))
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Элемент участвует в ДПКР, такой объект запрещено удалять",
                BaseEntity = typeof(RealityObjectStructuralElement),
                CheckAnyDependences = id => Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>().GetAll().Any(x => x.StructuralElement.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Удаление не возможно, т.к. данный конструктивный элемент сохранен в версии программы.",
                BaseEntity = typeof(RealityObjectStructuralElement),
                CheckAnyDependences = id => Container.Resolve<IDomainService<VersionRecordStage1>>().GetAll().Any(x => x.StructuralElement.Id == id)
            });

            return this;
        }
    }
}