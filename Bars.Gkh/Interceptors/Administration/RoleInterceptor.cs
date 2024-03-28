namespace Bars.Gkh.Interceptors
{
    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using System.Linq;

    public class RoleInterceptor : EmptyDomainInterceptor<Role>
    {
        public IDomainService<LocalAdminRoleRelations> LocalAdminRoleRelationsDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<Role> service, Role entity)
        {
            if (service.GetAll().Where(x => x.Name == entity.Name).Any())
                return Failure("Такая роль уже существует!");

            return Success();
        }

        //public override IDataResult BeforeUpdateAction(IDomainService<Role> service, Role entity)
        //{
        //    if (service.GetAll().Where(x => x.Name.Trim().ToUpper() == entity.Name.Trim().ToUpper()).Any())
        //        return Failure("Такой пользователь уже существует");

        //    return Success();
        //}

        public override IDataResult BeforeDeleteAction(IDomainService<Role> service, Role entity)
        {
            LocalAdminRoleRelationsDomain.GetAll()
                .Where(x => x.ParentRole.Id == entity.Id || x.ChildRole.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => this.LocalAdminRoleRelationsDomain.Delete(x));

            return Success();
        }
    }
}