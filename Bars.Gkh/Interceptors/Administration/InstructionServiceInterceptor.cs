namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class InstructionServiceInterceptor : EmptyDomainInterceptor<Instruction>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Instruction> service, Instruction entity)
        {
            if (entity.Code == "Main")
            {
                if (service.GetAll().Any(x => x.Code == "Main"))
                {
                    return Failure("Инструкция с кодом \"Main\" уже существует");
                }
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Instruction> service, Instruction entity)
        {
            if (entity.Code == "Main")
            {
                if (service.GetAll().Any(x => x.Code == "Main" && x.Id != entity.Id))
                {
                    return Failure("Инструкция с кодом \"Main\" уже существует");
                }
            }

            return Success();
        }       
    }
}
