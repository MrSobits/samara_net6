namespace Bars.GkhGji.Regions.Saha.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Saha.Entities;

    using Bars.B4.Utils;

    public class ActRemovalServiceInterceptor : GkhGji.Interceptors.ActRemovalServiceInterceptor
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ActRemoval> service, ActRemoval entity)
        {
            var violGroupDomain = Container.Resolve<IDomainService<DocumentViolGroup>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return result;
                }

                // удаляем связку документа с группой нарушений
                violGroupDomain.GetAll()
                               .Where(x => x.Document.Id == entity.Id)
                               .Select(x => x.Id)
                               .ForEach(x => violGroupDomain.Delete(x));


                return this.Success();
            }
            finally
            {
                Container.Release(violGroupDomain);
            }
        }
    }
}
