namespace Bars.Gkh.Permissions
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes.UI;

    public class GkhConfigPermissionMap : PermissionMap
    {
        public GkhConfigPermissionMap(IGkhConfigProvider provider)
        {
            Namespace("Gkh.Config", "Единые настройки приложения");

            var permissions =
                provider.Map.Where(x => x.Value.AttributeProvider.HasAttribute<PermissionableAttribute>(true))
                        .Select(
                            x =>
                            new
                                {
                                    name = x.Value.DisplayName,
                                    key = x.Key,
                                    permission = string.Format("Gkh.Config.{0}", x.Key),
                                    isSection = typeof(IGkhConfigSection).IsAssignableFrom(x.Value.Type)
                                })
                        .ToArray();

            var keys = new List<string>(permissions.Length);
            foreach (var permission in permissions)
            {
                var id = permission.permission;
                keys.Add(id);
                if (permission.isSection)
                {
                    this.Namespace(id, permission.name);
                    this.Permission(string.Format("{0}.View", id), string.Format("{0} - Просмотр", permission.name));
                }
                else
                {
                    this.Permission(string.Format("{0}_View", id), permission.name);
                }
            }

            foreach (var permission in permissions)
            {
                var parentKey = provider.Map[permission.key].Parent;
                while (!string.IsNullOrEmpty(parentKey))
                {
                    var parent = provider.Map[parentKey];
                    if (!keys.Contains(parentKey))
                    {
                        this.Namespace(string.Format("Gkh.Config.{0}", parentKey), parent.DisplayName);
                        keys.Add(parentKey);
                    }

                    parentKey = parent.Parent;
                }
            }
        }
    }
}