namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;

    public class SpecialQualificationServiceInterceptor : EmptyDomainInterceptor<SpecialQualification>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<SpecialQualification> service, SpecialQualification entity)
        {
            var voiceMemberDomain = this.Container.Resolve<IDomainService<SpecialVoiceMember>>();
            var voiceMemberIds =
                voiceMemberDomain.GetAll().Where(x => x.Qualification.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in voiceMemberIds)
            {
                voiceMemberDomain.Delete(id);
            }

            this.Container.Release(voiceMemberDomain);

            return this.Success();
        }
    }
}