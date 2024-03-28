namespace Bars.GkhGji.Regions.Smolensk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class ActCheckSmolServiceInterceptor : ActCheckServiceInterceptor<ActCheckSmol>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActCheckSmol> service, ActCheckSmol entity)
        {
            entity.HaveViolation = YesNoNotSet.NotSet;

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ActCheckSmol> service, ActCheckSmol entity)
        {
            var violGroupDomain = Container.Resolve<IDomainService<DocumentViolGroup>>();

            try
            {
                // удаляем связку документа с группой нарушений
                violGroupDomain.GetAll()
                               .Where(x => x.Document.Id == entity.Id)
                               .Select(x => x.Id)
                               .ForEach(x => violGroupDomain.Delete(x));


                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(violGroupDomain);
            }

        }
    }
}