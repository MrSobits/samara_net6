namespace Bars.B4.Modules.Analytics.Reports.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Proxies;
    using Pivot;
    using B4.Utils;
    using Castle.Windsor;
    using Modules.Reports;
    using NHibernate.Linq;
    using Security;
    using Utils;

    /// <summary>
    /// Реализация интерфейса панели отчетов
    /// </summary>
    public class ReportPanelService : IReportPanelService
    {
        private readonly IWindsorContainer container;

        public ReportPanelService(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public List<ReportCategory> Search(string query)
        {
            query = query.Trim().ToLower(); 

            var authService = this.container.Resolve<IAuthorizationService>();
            var userIdentity = this.container.Resolve<IUserIdentity>();

            var pivotHandlers = this.container.Kernel.GetAssignableHandlers(typeof(IPivotModel));

            var repository = this.container.Resolve<IDomainService<PrintForm>>();
            var storedReportDomain = this.container.Resolve<IDomainService<StoredReport>>();
            var rolePermDomain = this.container.Resolve<IDomainService<RolePermission>>();
            var userRoleDomain = this.container.Resolve<IDomainService<UserRole>>();

            try
            {
                var oldReports = repository.GetAll()
                .Select(x => new { x.Id, x.Name, CategoryName = x.Category.Name, x.ClassName, x.Description, CategoryId = x.Category.Id })
                .AsEnumerable()
                .Where(x => this.container.Kernel.HasComponent(x.ClassName))
                .WhereIf(!string.IsNullOrWhiteSpace(query), x => x.Name.ToLower().Contains(query) || x.CategoryName.ToLower().Contains(query))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    Category = x.CategoryName,
                    x.ClassName,
                    x.Description,
                    Class = this.container.Resolve<IPrintForm>(x.ClassName),
                    x.CategoryId
                })
                .Where(x => string.IsNullOrEmpty(x.Class.RequiredPermission) || authService.Grant(userIdentity, x.Class.RequiredPermission))
                .Select(z => new Report
                {
                    Id = z.Id,
                    Name = z.Name,
                    Description = z.Name,
                    ClassName = z.ClassName,
                    IsPivot = pivotHandlers.Any(h => h.ComponentModel.Name == z.ClassName),
                    ParamsController = z.Class.ParamsController,
                    IsOld = true,
                    CategoryName = z.Category,
                    CategoryId = z.CategoryId
                }).ToList();

                var userRoleIds = userRoleDomain.GetAll()
                    .Where(x => x.User.Id == userIdentity.UserId)
                    .Select(x => x.Role.Id)
                    .ToArray();

                var reportIdsByRole = rolePermDomain.GetAll()
                    .WhereIf(userRoleIds.Any(), x => userRoleIds.Contains(x.Role.Id))
                    .Select(x => new
                    {
                        PermissionId = x.PermissionId.ToLong()
                    })
                    .Select(x => x.PermissionId)
                    .ToArray();


                var reports = storedReportDomain.GetAll().Fetch(x => x.Category)
                    .Where(PredicateBuilder.OracleInRange<StoredReport, long>(x => x.Id, reportIdsByRole).Or(x => x.ForAll))
                    .Where(x => x.Category != null)
                    .WhereIf(!string.IsNullOrWhiteSpace(query),
                        x => x.Name.ToLower().Contains(query) || x.Category.Name.ToLower().Contains(query))
                    
                    .Select(x => new Report
                    {
                        Id = x.Id,
                        Code = x.Code,
                        CategoryName = x.Category.Name,
                        CategoryId = x.Category.Id,
                        Name = x.Name,
                        IsOld = false,
                        Description = x.Description ?? string.Empty
                    });

                var data = oldReports.Union(reports)
                    .GroupBy(x => x.CategoryId)
                    .Select(x => new ReportCategory
                    {
                        Id = x.Key,
                        Name = x.First().CategoryName,
                        Reports = x.OrderBy(y => y.Name).ToList()
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                return data;
            }
            finally
            {
                this.container.Release(authService);
                this.container.Release(userIdentity);
                this.container.Release(repository);
                this.container.Release(storedReportDomain);
                this.container.Release(userRoleDomain);
            }
        }
    }
}