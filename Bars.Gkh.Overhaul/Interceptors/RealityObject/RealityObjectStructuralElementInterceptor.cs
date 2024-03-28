using Bars.B4.DataAccess;

namespace Bars.Gkh.Overhaul.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhCr.Entities;
    using Overhaul.Entities;

    public class RealityObjectStructuralElementInterceptor : EmptyDomainInterceptor<RealityObjectStructuralElement>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectStructuralElement> service, RealityObjectStructuralElement entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            entity.Name = entity.StructuralElement.Name;
            using (Container.Using(stateProvider))
            {
                if (!string.IsNullOrEmpty(entity.StructuralElement.MutuallyExclusiveGroup))
                {
                    var mutuallyExclusiveElems =
                        service.GetAll()
                            .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                            .Where(x => x.StructuralElement.Id != entity.StructuralElement.Id)
                            .Where(x => x.State.StartState)
                            .Where(
                                x =>
                                    x.StructuralElement.MutuallyExclusiveGroup ==
                                    entity.StructuralElement.MutuallyExclusiveGroup)
                            .Where(x => x.StructuralElement.Group.Id == entity.StructuralElement.Group.Id)
                            .Select(x => x.StructuralElement.Name)
                            .ToArray();

                    var message = string.Empty;

                    if (mutuallyExclusiveElems.Length > 0)
                    {
                        message = mutuallyExclusiveElems.Aggregate(message,
                            (current, str) => current + string.Format(" {0}; ", str));
                    }

                    if (!string.IsNullOrEmpty(message))
                    {
                        message = string.Format("Выбраны взаимоисключающие конструктивные элементы: {0};{1}",
                            entity.StructuralElement.Name, message);
                        return Failure(message);
                    }
                }

                // Перед сохранением проставляем начальный статус
                stateProvider.SetDefaultState(entity);

                return Success();
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<RealityObjectStructuralElement> service, RealityObjectStructuralElement entity)
        {
            var roSeIdHistoryDomain = Container.ResolveDomain<RealObjStructElementIdHistory>();

            using (Container.Using(roSeIdHistoryDomain))
            {
                roSeIdHistoryDomain.Save(new RealObjStructElementIdHistory
                {
                    RealityObject = entity.RealityObject,
                    RealObjStructElId = entity.Id
                });

                return Success();
            }
        }


        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectStructuralElement> service, RealityObjectStructuralElement entity)
        {
            var roSeIdHistoryDomain = Container.ResolveDomain<RealObjStructElementIdHistory>();
            var structElAttrDomain = Container.ResolveDomain<RealityObjectStructuralElementAttributeValue>();

            structElAttrDomain.GetAll()
                               .Where(x => x.Object.Id == entity.Id)
                               .Select(x => x.Id)
                               .AsEnumerable()
                               .ForEach(x => structElAttrDomain.Delete(x));

            if (!roSeIdHistoryDomain.GetAll().Any(x => x.RealObjStructElId == entity.Id))
            {
                roSeIdHistoryDomain.Save(new RealObjStructElementIdHistory
                {
                    RealityObject = entity.RealityObject,
                    RealObjStructElId = entity.Id
                });
            }

            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<RealityObjectStructuralElement> service, RealityObjectStructuralElement entity)
        {
            var workDomain = Container.Resolve<IDomainService<Work>>();
            var typeWorkRepo = Container.Resolve<IRepository<TypeWorkCr>>();
            var StructuralElementWorkDomain = Container.Resolve<IDomainService<StructuralElementWork>>();
            try
            {
                var work = StructuralElementWorkDomain.GetAll()
               .Where(x => x.StructuralElement == entity.StructuralElement)
               .Select(x => x.Job.Work)
               .FirstOrDefault();
                if (work != null)
                {
                    var typeWorkList = typeWorkRepo.GetAll()
                        .Where(x => x.Work == work)
                        .Where(x=> x.IsActive)
                        .Where(x=> x.ObjectCr.ProgramCr != null)
                        .Where(x => x.ObjectCr.RealityObject == entity.RealityObject)
                        .Select(x=> x.Id).ToList();
                    if (typeWorkList.Count > 0)
                    {
                        typeWorkList.ForEach(x =>
                        {
                            var twcr = typeWorkRepo.Get(x);
                            twcr.Volume = entity.Volume;
                            typeWorkRepo.Update(twcr);
                        });
                    }
                }
                return Success();
                //   return AfterUpdateAction(service, entity);
            }
            catch (Exception e)
            {
                return Success();
            }
            //finally
            //{
            //    Container.Release(versionRecordStageDomain);
            //    Container.Release(workDomain);
            //    Container.Release(typeWorkRepo);
            //    Container.Release(StructuralElementWorkDomain);
            //}

        }
    }
}