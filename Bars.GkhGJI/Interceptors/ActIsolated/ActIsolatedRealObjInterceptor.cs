namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActIsolatedRealObjInterceptor : EmptyDomainInterceptor<ActIsolatedRealObj>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ActIsolatedRealObj> service, ActIsolatedRealObj entity)
        {
            var violationDomain = this.Container.Resolve<IDomainService<ActIsolatedRealObjViolation>>();
            var measureDomain = this.Container.Resolve<IDomainService<ActIsolatedRealObjMeasure>>();
            var eventDomain = this.Container.Resolve<IDomainService<ActIsolatedRealObjEvent>>();
            try
            {
                violationDomain.GetAll()
                    .Where(x => x.ActIsolatedRealObj.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => violationDomain.Delete(x));

                measureDomain.GetAll()
                    .Where(x => x.ActIsolatedRealObj.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => measureDomain.Delete(x));

                eventDomain.GetAll()
                    .Where(x => x.ActIsolatedRealObj.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => eventDomain.Delete(x));
            }
            finally
            {
                this.Container.Release(violationDomain);
                this.Container.Release(measureDomain);
                this.Container.Release(eventDomain);
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}