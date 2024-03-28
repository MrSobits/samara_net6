namespace Bars.Gkh.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class InstructionService : IInstructionService
    {
        public IWindsorContainer Container { get; set; }

        public long GetMainInstruction()
        {
            var mainInstruction = Container.Resolve<IDomainService<Instruction>>().GetAll().FirstOrDefault(x => x.Code == "Main");
            return mainInstruction != null ? mainInstruction.File.Id : 0;
        }
    }
}