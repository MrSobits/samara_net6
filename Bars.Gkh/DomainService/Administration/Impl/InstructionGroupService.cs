namespace Bars.Gkh.DomainService.Impl
{
    using System.Linq;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.B4;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис категории для документации
    /// </summary>
    public class InstructionGroupService : IInstructionGroupService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Список по ролю
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public IDataResult ListByRole(BaseParams baseParams)
        {
            var instructionsDomain = Container.Resolve<IDomainService<Instruction>>();
            var groupsDomain = Container.Resolve<IDomainService<InstructionGroup>>();
            var groupRolesDomain = Container.Resolve<IDomainService<InstructionGroupRole>>();
            var userRoleDomain = Container.Resolve<IDomainService<UserRole>>();
            var usersDomain = Container.Resolve<IDomainService<User>>();
            var identity = Container.Resolve<IUserIdentity>();

            BaseDataResult result;
            try
            {
                var user = usersDomain.GetAll().FirstOrDefault(x => x.Id == identity.UserId);
                var roles = new long[] {};
                if (user != null)
                {
                    roles = userRoleDomain.GetAll().Where(r => r.User.Id == user.Id).Select(r => r.Role.Id).ToArray();
                }

                var groups = groupsDomain.GetAll()
                    .LeftJoin(groupRolesDomain.GetAll(), ig => ig.Id, igr => igr.InstructionGroup.Id, (ig, igr) => new {Group = ig, Role = igr != null ? igr.Role.Id : 0})
                    .GroupBy(x => x.Group)
                    .Where(x => x.Count() == x.Count(y => y.Role == 0) || x.Any(y => roles.Contains(y.Role)))
                    .Select(x => x.Key)
                    .ToList();

                var data2 = groups.LeftJoin(instructionsDomain.GetAll(), g => g.Id, i => i.InstructionGroup.Id, (g, i) => new {Group = g, Instruction = i})
                    .GroupBy(x => x.Group).ToList()
                    .Select(x => new
                    {
                        leaf = false,
                        x.Key.Id,
                        x.Key.DisplayName,
                        IsInstruction = false,
                        children = x.Where(y => y.Instruction != null).Select(y => new
                        {
                            leaf = true,
                            y.Instruction.Id,
                            y.Instruction.DisplayName,
                            y.Instruction.File,
                            IsInstruction = true
                        })
                    }).ToList();

                var groupIds = groups.Select(x => x.Id).ToArray();
                var data = instructionsDomain.GetAll()
                    .WhereContains(x => x.InstructionGroup.Id, groupIds)
                    .GroupBy(x => x.InstructionGroup).ToList().Select(x => new
                    {
                        leaf = false,
                        x.Key.Id,
                        x.Key.DisplayName,
                        IsInstruction = false,
                        children = x.Select(y => new
                        {
                            leaf = true,
                            y.Id,
                            y.DisplayName,
                            y.File,
                            IsInstruction = true
                        })
                    }).ToList();
                result = new BaseDataResult(data2);
            }
            finally
            {
                Container.Release(identity);
                Container.Release(usersDomain);
                Container.Release(userRoleDomain);
                Container.Release(groupsDomain);
                Container.Release(groupRolesDomain);
                Container.Release(instructionsDomain);
            }

            return result;
        }
    }
}