namespace Bars.Gkh.Config.Attributes
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;

    using Fasterflect;

    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Проверка прав доступа при выполнении методов
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ControllerPermissionAttribute : ActionTrackFilter
    {
        private readonly string permission;

        /// <summary>
        /// <param name="permission">Ключ права доступа</param>
        /// </summary>
        public ControllerPermissionAttribute(string permission)
        {
            this.permission = permission;
        }

        /// <inheritdoc cref="ActionTrackFilter.OnActionExecuting"/>
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            var userId = actionExecutingContext.HttpContext.User.Identity.TryGetPropertyValue("UserId");

            var container = ApplicationContext.Current.Container;

            IDomainService<UserRole> userRoleDomain;

            using (container.Using(userRoleDomain = container.ResolveDomain<UserRole>()))
            {
                var rolePermissions = userRoleDomain.GetAll()
                    .Where(x => (object) x.User.Id == userId)
                    .SelectMany(x => x.Role.Permissions)
                    .Select(x => x.PermissionId).ToList();

                var isUserAccces = rolePermissions.Contains(this.permission);

                if (!isUserAccces)
                {
                    throw new AuthorizationFailureException("Пользователю не разрешено использование данного действия");
                }
            }
        }
    }
}