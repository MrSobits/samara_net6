namespace Bars.Gkh.DomainService.Administration.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.B4.Security;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Newtonsoft.Json.Linq;

    public class LocalAdminRoleService : ILocalAdminRoleService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<LocalAdminRoleRelations> LocalAdminRoleRelationsDomain { get; set; }

        /// <inheritdoc />
        public IList<Role> GetAll()
        {
            return this.LocalAdminRoleRelationsDomain.GetAll()
                .Select(x => x.ParentRole)
                .DistinctBy(x => x.Id)
                .ToList();
        }

        /// <inheritdoc />
        public IDataResult GetAll(BaseParams baseParams)
        {
            return this.LocalAdminRoleRelationsDomain.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.ParentRole)
                .Select(x => new
                {
                    x.Key.Id,
                    x.Key.Name,
                    ChildRoles = x.Select(y => new
                    {
                        y.ChildRole.Id,
                        y.ChildRole.Name
                    })
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }

        /// <inheritdoc />
        public IDataResult GetChildRoleList(BaseParams baseParams)
        {
            var adminRoleId = baseParams.Params.GetAsId();

            return this.LocalAdminRoleRelationsDomain.GetAll()
                .Where(x => x.ParentRole.Id == adminRoleId)
                .Select(x => x.ChildRole)
                .ToListDataResult(baseParams.GetLoadParam());
        }

        /// <inheritdoc />
        public IList<Role> GetChildRoleList(long localAdminId)
        {
            return this.LocalAdminRoleRelationsDomain.GetAll()
                .Where(x => x.ParentRole.Id == localAdminId)
                .Select(x => x.ChildRole)
                .ToList();
        }

        /// <inheritdoc />
        public bool IsLocalAdmin(long roleId)
        {
            return this.LocalAdminRoleRelationsDomain.GetAll()
                .Any(x => x.ParentRole.Id == roleId);
        }

        /// <inheritdoc />
        public bool IsThisUserLocalAdmin()
        {
            var userRole = this.Container.Resolve<IGkhUserManager>().GetActiveUser().Roles.First().Role;

            return this.IsLocalAdmin(userRole.Id);
        }

        /// <inheritdoc />
        public JToken ConvertTreeToJObject(PermissionTreeNode treeNode, ISet<string> allowPermissionSet, ISet<string> allowPermissionNodeSet = null)
        {
            var overlaps = true;
            if (allowPermissionNodeSet != null)
            {
                overlaps = allowPermissionSet.Overlaps(allowPermissionNodeSet);
            }

            JToken result;

            var tree = overlaps
                ? this.ConvertNodeToJObject(treeNode, string.Empty, allowPermissionSet, allowPermissionNodeSet)
                : null;

            if (tree != null && tree.TryGetValue("children", out result))
            {
                return result;
            }
            return new JArray();
        }

        protected JObject ConvertNodeToJObject(PermissionTreeNode node, string idPrefix, ISet<string> allowPermissionSet, ISet<string> allowPermissionNodeSet)
        {
            var jnode = new JObject();

            var permissionId = idPrefix + node.IDPart;
            var isGrantNode = true;

            if (allowPermissionNodeSet != null)
            {
                isGrantNode = string.IsNullOrEmpty(idPrefix) || allowPermissionNodeSet.Contains(permissionId);
            }

            jnode["id"] = permissionId;
            jnode["order"] = node.Order;
            jnode["PermissionID"] = permissionId;
            jnode["text"] = node.Description;
            if (!node.IsNamespace)
            {
                jnode["checked"] = allowPermissionSet.Contains(permissionId);
            }
            var hasChildren = node.Children.Any();

            jnode["leaf"] = !hasChildren;

            if (hasChildren)
            {
                var children = new JArray();

                var nextIdPrefix = idPrefix + node.IDPart + ".";
                if (nextIdPrefix == ".")
                {
                    nextIdPrefix = string.Empty;
                }

                foreach (var childNode in node.Children.Values.OrderBy(child => child.Order))
                {
                    var child = this.ConvertNodeToJObject(childNode, nextIdPrefix, allowPermissionSet, allowPermissionNodeSet);
                    if (child != null)
                    {
                        children.Add(child);
                    }
                }

                if (!children.HasValues)
                {
                    return null;
                }

                jnode["children"] = children;
            }

            return isGrantNode ? jnode : null;
        }
    }
}