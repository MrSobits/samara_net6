namespace Bars.GisIntegration.Base.Interceptors
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.GisRole;
    using System.Linq;
    using Bars.B4.Utils;

    /// <summary>
    /// Interceptor для GisRole
    /// </summary>
    public class GisRoleInterceptor : EmptyDomainInterceptor<GisRole>
    {
        /// <summary>
        /// Событие перед удалением роли ГИС
        /// </summary>
        /// <param name="service">Домен GisRole</param>
        /// <param name="entity">Удаляемая роль</param>
        /// <returns></returns>
        public override IDataResult BeforeDeleteAction(IDomainService<GisRole> service, GisRole entity)
        {
            var gisRoleMethodDomain = this.Container.ResolveDomain<GisRoleMethod>();

            gisRoleMethodDomain.GetAll()
                .Where(x => x.Role.Id == entity.Id)
                .Select(x => x.Id).AsEnumerable()
                .ForEach(x => gisRoleMethodDomain.Delete(x));

            return this.Success();
        }
    }
}
