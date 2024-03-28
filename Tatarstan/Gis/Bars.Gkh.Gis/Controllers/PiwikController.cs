namespace Bars.Gkh.Gis.Controllers
{
    using B4;
    using B4.Utils;

    public class PiwikController : BaseController
    {
        public string GetUser()
        {
            var userIdentity = Container.Resolve<IUserIdentity>();

            try
            {
                if (userIdentity is AnonymousUserIdentity)
                {
                    return null;
                }

                return MD5.GetHashString64(userIdentity.UserId.ToStr());
            }
            finally
            {
                Container.Release(userIdentity);
            }
        }
    }
}