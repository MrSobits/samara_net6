namespace Bars.GkhRf.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhRf.Entities;

    public class TransferRfInterceptor : EmptyDomainInterceptor<TransferRf>
    {
        public override IDataResult BeforeCreateAction(IDomainService<TransferRf> service, TransferRf entity)
        {
            if (!CheckContract(service, entity))
            {
                return Failure("Данный договор уже добавлен. Пожалуйста выберите другой договор!");
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<TransferRf> service, TransferRf entity)
        {
            if (!CheckContract(service, entity))
            {
                return Failure("Данный договор уже добавлен. Пожалуйста выберите другой договор!");
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<TransferRf> service, TransferRf entity)
        {
            if (Container.Resolve<IDomainService<TransferRfRecord>>().GetAll().Any(x => x.TransferRf.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Сведения о перечислениях по договору;");
            }

            return Success();
        }

        private bool CheckContract(IDomainService<TransferRf> service, TransferRf entity)
        {
            return !service.GetAll().Any(x => x.ContractRf.Id == entity.ContractRf.Id && x.Id != entity.Id);
        }
    }
}