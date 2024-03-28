using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain;
using Bars.GkhCr.Enums;

namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;

    public class QualificationServiceInterceptor : EmptyDomainInterceptor<Qualification>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<Qualification> service, Qualification entity)
        {
            var voiceMemberDomain = Container.Resolve<IDomainService<VoiceMember>>();
            var voiceMemberIds =
                voiceMemberDomain.GetAll().Where(x => x.Qualification.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in voiceMemberIds)
            {
                voiceMemberDomain.Delete(id);
            }

            Container.Release(voiceMemberDomain);

            return Success();
        }
    }
}