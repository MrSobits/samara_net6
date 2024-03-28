namespace Bars.Gkh.Repair.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Repair.Entities.RepairControlDate;

    public class RepairControlDateInterceptor : EmptyDomainInterceptor<RepairControlDate>
    {
        public IDomainService<RepairControlDate> RepairControlDateDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<RepairControlDate> service, RepairControlDate entity)
        {
            if (RepairControlDateDomain.GetAll()
                         .Any(x => x.RepairProgram.Id == entity.RepairProgram.Id && x.Work.Id == entity.Work.Id))
            {
                return Failure("Контрольный срок по текущей работе уже есть в программе.");
            }

            return Success();
        }

    }
}
