namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;

    public class FeatureViolGjiInterceptor : EmptyDomainInterceptor<FeatureViolGji>
    {

        public override IDataResult BeforeCreateAction(IDomainService<FeatureViolGji> service, FeatureViolGji entity)
        {
            if (entity.Parent != null)
            {
                var newFullName = entity.Name;
                GetFullName(service, ref newFullName, entity.Parent.Id);

                if ( newFullName != entity.FullName )
                {
                    entity.FullName = newFullName;
                }
            }
            else
            {
                entity.FullName = entity.Name;
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<FeatureViolGji> service, FeatureViolGji entity)
        {
            if (entity.Parent != null)
            {
                var newFullName = entity.Name;
                GetFullName(service, ref newFullName, entity.Parent.Id);

                if (newFullName != entity.FullName)
                {
                    entity.FullName = newFullName;
                }
            }
            else
            {
                entity.FullName = entity.Name;
            }
            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<FeatureViolGji> service, FeatureViolGji entity)
        {
            // запусскаем обновление детей потому как могло изменится наименование и у детей должен вызватся интерцептор который поменяет FullName
            var childrens = service.GetAll().Where(x => x.Parent.Id == entity.Id);

            foreach (var children in childrens)
            {
                service.Update(children);
            }
            return Success();
        }

        private void GetFullName(IDomainService<FeatureViolGji> service, ref string result, long parentId)
        {
            var parentData = service.GetAll().Where(x => x.Id == parentId).Select(x => new {parId = x.Parent != null ? x.Parent.Id : 0, x.Name}).FirstOrDefault();

            if(parentData != null)
            {
                result = parentData.Name.Trim() + "/ " + result;
                
                if (parentData.parId > 0)
                {
                    GetFullName(service, ref result, parentData.parId);
                }
            }

        }
        
        public override IDataResult BeforeDeleteAction(IDomainService<FeatureViolGji> service, FeatureViolGji entity)
        {
            var appCitsStatsubjDomain = this.Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            var violFeaurureDomain = this.Container.Resolve<IDomainService<ViolationFeatureGji>>();
            var statSubjFeaurureDomain = this.Container.Resolve<IDomainService<StatSubsubjectFeatureGji>>();

            try
            {
                var refFuncs = new List<Func<long, string>>
                               {
                                  id => appCitsStatsubjDomain.GetAll().Any(x => x.Feature.Id == id) ? "Характеристики нарушений реестра обращений" : null,
                                  id => violFeaurureDomain.GetAll().Any(x => x.FeatureViolGji.Id == id) ? "Пункты нарушений" : null
                               };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }

                var ids =
                    statSubjFeaurureDomain.GetAll().Where(x => x.FeatureViol.Id == entity.Id).Select(x => x.Id).ToList();

                foreach (var id in ids)
                {
                    statSubjFeaurureDomain.Delete(id);
                }

            }
            finally 
            {
                Container.Release(appCitsStatsubjDomain);
                Container.Release(violFeaurureDomain);
                Container.Release(statSubjFeaurureDomain);
            }
            
            return Success();
        }
    }
}