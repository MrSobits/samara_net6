namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using B4;
    using Gkh.Authentification;
    using Entities;

    public class VoiceMemberServiceInterceptor : EmptyDomainInterceptor<VoiceMember>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<VoiceMember> service, VoiceMember entity)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var qualMemberRoleService = Container.Resolve<IDomainService<QualificationMemberRole>>();

            try
            {
                var activeOperator = userManager.GetActiveOperator();
                if (activeOperator != null)
                {
                    var activeRoles = activeOperator.User.Roles.Select(x => x.Role.Id).ToList();

                    var roleExist =
                        qualMemberRoleService.GetAll()
                                             .Where(x => x.QualificationMember.Id == entity.QualificationMember.Id)
                                             .Any(x => activeRoles.Contains(x.Role.Id));

                    return !roleExist
                        ? Failure(string.Format("Вы не можете проголосовать по данному органу : {0}", entity.QualificationMember.Name))
                        : Success();
                }

                return Success();
            }
            finally 
            {
                Container.Release(userManager);
                Container.Release(qualMemberRoleService);
            }
        }

        public override IDataResult BeforeCreateAction(IDomainService<VoiceMember> service, VoiceMember entity)
        {
            return BeforeUpdateAction(service, entity);
        }
    }
}