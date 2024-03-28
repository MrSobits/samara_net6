namespace Bars.GkhGji.Regions.Smolensk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class ActSurveySmolInterceptor : GkhGji.Interceptors.ActSurveyServiceInterceptor<ActSurveySmol>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ActSurveySmol> service, ActSurveySmol entity)
        {

            var longDescService = Container.Resolve<IDomainService<ActSurveyLongDescription>>();

            try
            {
                longDescService.GetAll()
                    .Where(x => x.ActSurvey.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => longDescService.Delete(x));

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(longDescService);
            }
        }
    }
}