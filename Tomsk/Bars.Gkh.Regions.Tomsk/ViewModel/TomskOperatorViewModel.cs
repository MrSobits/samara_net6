namespace Bars.Gkh.Regions.Tomsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Regions.Tomsk.Entities;
    using Bars.Gkh.ViewModel;

    // Болванка потому что от этого класса могли наследоваться
    public class TomskOperatorViewModel : OperatorViewModel<TomskOperator>
    {

        public override IDataResult Get(IDomainService<TomskOperator> domain, BaseParams baseParams)
        {
            var userRolesService = Container.Resolve<IDomainService<UserRole>>();
            try
            {
                var id = baseParams.Params.GetAs<long>("id");
                var obj = domain.GetAll().FirstOrDefault(x => x.Id == id);

                return obj != null ? new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.User.Name,
                        obj.User.Login,
                        Password = string.Empty,
                        NewPassword = string.Empty,
                        Role = userRolesService.GetAll().Where(x => x.User.Id == obj.User.Id).Select(x => x.Role).FirstOrDefault(),
                        obj.User.Email,
                        obj.TypeWorkplace,
                        obj.Phone,
                        obj.IsActive,
                        obj.Inspector,
                        Contragent = obj.Contragent != null ? new { obj.Contragent.Id, obj.Contragent.Name } : null,
                        obj.ContragentType,
                        obj.ShowUnassigned
                    }) : new BaseDataResult();
            }
            finally
            {
                Container.Release(userRolesService);
            }

        }
    }
}
