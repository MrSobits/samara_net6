namespace Bars.GkhCr.Interceptors
{
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using B4;
    using Entities;
    using Enums;
    using Bars.B4.IoC;
    using Bars.GkhCr.Entities;
    using Gkh.Domain;

    public class QualificationMemberServiceInterceptor : EmptyDomainInterceptor<QualificationMember>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<QualificationMember> service, QualificationMember entity)
        {
            var voiceMemberDomain = Container.ResolveDomain<VoiceMember>();
            var qualMemberRoleDomain = Container.ResolveDomain<QualificationMemberRole>();

            using (Container.Using(voiceMemberDomain, qualMemberRoleDomain))
            {
                if (voiceMemberDomain.GetAll().Any(
                            x => x.QualificationMember.Id == entity.Id &&
                                (x.TypeAcceptQualification != TypeAcceptQualification.NotDefined || x.DocumentDate.HasValue)))
                {
                    return Failure("Существуют связанные записи в следующих таблицах: Голоса участников квалификационного отбора;");
                }

                voiceMemberDomain.GetAll()
                    .Where(x => x.QualificationMember.Id == entity.Id)
                    .Select(x => x.Id)
                    .AsEnumerable()
                    .ForEach(x => voiceMemberDomain.Delete(x));


                qualMemberRoleDomain.GetAll()
                    .Where(x => x.QualificationMember.Id == entity.Id)
                    .Select(x => x.Id)
                    .AsEnumerable()
                    .ForEach(x => qualMemberRoleDomain.Delete(x));


                return Success();
            }
        }
    }
}