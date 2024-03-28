namespace Bars.Gkh.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Navigation;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;

    public class BaseMenuController : BaseController
    {
        private List<string> _statePermissions;
        private List<Role> _operatorRoles;

        protected void InitActiveOperatorAndRoles()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var _operator = userManager.GetActiveOperator();
            if (_operator != null)
            {
                _operatorRoles = _operator.User.Roles.Select(x => x.Role).ToList();
            }
        }

        protected void InitStatePermissions(State state)
        {
            if (state == null)
            {
                return;
            }

            var statePermissionService = Container.Resolve<IDomainService<StateRolePermission>>();
            _statePermissions = statePermissionService.GetAll()
                .Where(x => x.State.Id == state.Id)
                .WhereIf(_operatorRoles != null, x => _operatorRoles.Contains(x.Role))
                .Select(x => x.PermissionId)
                .ToList();
        }

        public ActionResult GetMenu(StoreLoadParams storeParams)
        {
            var menuName = storeParams.Params.GetAs("menuName", string.Empty);

            return new JsonNetResult(GetMenuItems(menuName));
        }

        protected IEnumerable<MenuItem> GetMenuItems(string menuName)
        {
            IEnumerable<MenuItem> menuItems = null;
            var menu = Container.Resolve<INavigationContainer>().With(x => x.GetMenu(menuName));

            if (menu != null)
            {
                menuItems = menu.Items;

                var service = Container.ResolveAll<IAuthorizationService>().FirstOrDefault();
                var userIdentity = Container.Resolve<IUserIdentity>();

                menuItems = FilterInacessibleItems(menuItems, service, userIdentity);
            }

            return menuItems;
        }

        protected IEnumerable<MenuItem> FilterByNoAvialableConrollers(IEnumerable<MenuItem> menuItems, IEnumerable<string> noAvailibleControllers)
        {
            menuItems = menuItems.Where(x => !noAvailibleControllers.Contains(x.Href));

            menuItems = menuItems.Select(item => new MenuItem
            {
                Caption = item.Caption,
                Href = item.Href,
                Icon = item.Icon,
                Items = item.Items.Any() ? FilterByNoAvialableConrollers(item.Items, noAvailibleControllers).ToList() : item.Items,
                RequiredPermissions = item.RequiredPermissions,
                ScriptHref = item.ScriptHref,
                Options = item.Options
            })
             .ToArray();

            return menuItems;
        }
        
        protected IEnumerable<MenuItem> FilterInacessibleItems(IEnumerable<MenuItem> items, IAuthorizationService service, IUserIdentity userIdentity)
        {
            // Алгоритм:
            // 1) Из списка элементов убираем недоступные
            // 2) Копируем список, чтобы можно было его изменять
            // 3) Применяем фильтрацию элементов рекурсивно

            if (service != null)
            {
                // 1)
                items = items.Where(item =>
                {
                    var ok = item.RequiredPermissions
                        .All(permission => service.Grant(userIdentity, permission));

                    return ok;
                })
                .ToList();
            }


            // 2)
            items = items.Select(item => new MenuItem
            {
                Caption = item.Caption,
                Href = item.Href,
                Icon = item.Icon,
                // 3)
                Items = FilterInacessibleItems(item.Items, service, userIdentity).ToList(),
                RequiredPermissions = item.RequiredPermissions,
                ScriptHref = item.ScriptHref,
                Options = item.Options
            })
            .ToList();

            return items;
        }

        protected IEnumerable<MenuItem> FilterInacessibleStateItems(IEnumerable<MenuItem> items)
        {
            if (_statePermissions == null)
            {
                return items;
            }

            items = items.Where(
                x =>
                {
                    if (x.RequiredPermissions.Count == 0)
                    {
                        return true;
                    }

                    return x.RequiredPermissions.Any(y => _statePermissions.Contains(y));
                });

            items = items.Select(item => new MenuItem
            {
                Caption = item.Caption,
                Href = item.Href,
                Icon = item.Icon,
                Items = FilterInacessibleStateItems(item.Items).ToList(),
                RequiredPermissions = item.RequiredPermissions,
                ScriptHref = item.ScriptHref,
                Options = item.Options
            }).ToList();

            return items;
        }
    }
}