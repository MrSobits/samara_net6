using System;
using Bars.B4.DataAccess;
using Bars.B4.Utils;

namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class InstructionGroupInterceptor : EmptyDomainInterceptor<InstructionGroup>
    {
        public IDomainService<Instruction> InstructionService { get; set; }
        public IDomainService<InstructionGroupRole> InstructionGroupRoleService { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<InstructionGroup> service, InstructionGroup entity)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    var instructions = InstructionService.GetAll().Where(x => x.InstructionGroup.Id == entity.Id).Select(x => x.Id);
                    foreach (var id in instructions)
                    {
                        InstructionService.Delete(id);
                    }

                    var roles = InstructionGroupRoleService.GetAll().Where(x => x.InstructionGroup.Id == entity.Id).Select(x => x.Id);
                    foreach (var id in roles)
                    {
                        InstructionGroupRoleService.Delete(id);
                    }
                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Произошла не известная ошибка при откате транзакции",exc);
                    }

                    throw;
                }
                return base.BeforeDeleteAction(service, entity);
            }
        }

        private IDataTransaction BeginTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }
    }
}
