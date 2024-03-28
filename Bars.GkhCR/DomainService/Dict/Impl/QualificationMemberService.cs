using Bars.B4.DataAccess;

namespace Bars.GkhCr.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class QualificationMemberService : IQualificationMemberService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<QualificationMemberRole> ServiceRole { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var qualMemberId = baseParams.Params["qualMemberId"].ToInt();

                var roleNames = string.Empty;
                var roleIds = string.Empty;

                var dataRoles = ServiceRole.GetAll()
                    .Where(x => x.QualificationMember.Id == qualMemberId)
                    .Select(x => new
                        {
                            RoleId = x.Role.Id,
                            x.Role.Name
                        })
                    .ToArray();

                foreach (var item in dataRoles)
                {
                    if (!string.IsNullOrEmpty(roleNames))
                    {
                        roleNames += ", ";
                    }

                    roleNames += item.Name;

                    if (!string.IsNullOrEmpty(roleIds))
                    {
                        roleIds += ", ";
                    }

                    roleIds += item.RoleId.ToStr();
                }

                return new BaseDataResult(new
                {
                    roleNames,
                    roleIds
                }) { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Message = exc.Message, Success = false };
            }
        }

        public IDataResult AddRoles(BaseParams baseParams)
        {
            using (var tx = Container.Resolve<IDataTransaction>())
            {

                try
                {
                    var qualMemberId = baseParams.Params["qualMemberId"].ToLong();

                    var rolesId = baseParams.Params.GetAs<long[]>("rolesId");

                    var dictInspectors =
                        ServiceRole.GetAll()
                            .Where(x => x.QualificationMember.Id == qualMemberId)
                            .GroupBy(x => x.Role.Id)
                            .AsEnumerable()
                            .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                    var serviceRole = Container.Resolve<IDomainService<Role>>();
                    var qualMember = Container.Resolve<IDomainService<QualificationMember>>().Load(qualMemberId);
                    foreach (var id in rolesId)
                    {
                        if (dictInspectors.ContainsKey(id))
                        {
                            // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                            // без добавления в БД
                            dictInspectors.Remove(id);
                            continue;
                        }

                        var newObj = new QualificationMemberRole
                            {
                                QualificationMember = qualMember,
                                Role = serviceRole.Load(id)
                            };

                        ServiceRole.Save(newObj);
                    }

                    foreach (var keyValue in dictInspectors)
                    {
                        ServiceRole.Delete(keyValue.Value);
                    }

                    tx.Commit();
                    return new BaseDataResult {Success = true};
                }
                catch (ValidationException exc)
                {
                    tx.Rollback();
                    return new BaseDataResult {Message = exc.Message, Success = false};
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public IDataResult ListRoles(BaseParams baseParams)
        {
            var memberId = baseParams.Params["memberId"].ToInt();
            var loadParams = baseParams.GetLoadParam();

            var data = ServiceRole.GetAll()
                .Where(x => x.QualificationMember.Id == memberId)
                .Select(x => new
                    {
                        x.Role.Id,
                        x.Role.Name
                    })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).ToList(), data.Count());
        }
    }
}
