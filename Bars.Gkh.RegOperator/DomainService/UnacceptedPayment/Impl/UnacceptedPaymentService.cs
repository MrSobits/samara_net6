namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System.Linq;
    using B4;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;
    using Domain;
    using Domain.Repository.MoneyOperations;
    using Entities.ValueObjects;
    using Entities;
    using Castle.Windsor;
    using Gkh.Domain;
    using Tasks.UnacceptedPayment;

    public partial class UnacceptedPaymentService : IUnacceptedPaymentService
    {
        #region Properties

        public IWindsorContainer Container { get; set; }
        
        public IDomainService<UnacceptedPayment> UnaccPaymentDomain { get; set; }

        public IDomainService<Transfer> TransferDomain { get; set; }

        public IMoneyOperationRepository MoneyOperationRepo { get; set; }

        public IRealtyObjectPaymentSession PaymentSession { get; set; }

        public ITaskManager TaskManager { get; set; }

        #endregion Properties

        public IDataResult AcceptPayments(BaseParams baseParams)
        {
            return TaskManager.CreateTasks(new UnnacceptedPaymentTaskProvider(), baseParams);
        }

        public IDataResult CancelPayments(BaseParams baseParams)
        {
            var packetIds = baseParams.Params.GetAs<long[]>("packetIds", ignoreCase: true);

            return CancelPayments(packetIds);
        }

        public IDataResult CancelPayments(long[] packetIds = null)
        {
            IDataResult message = new BaseDataResult();

            try
            {
                if (EnumerableExtension.IsEmpty(packetIds))
                {
                    return new BaseDataResult(false, "Необходимо выбрать записи");
                }

                Container.InTransaction(() =>
                {
                    var payments = UnaccPaymentDomain.GetAll()
                        .WhereIf(packetIds.IsNotEmpty(), x => packetIds.Contains(x.Packet.Id));

                    message = CancelPayments(payments);

                    PaymentSession.Complete();
                });
            }
            catch (ValidationException e)
            {
                return BaseDataResult.Error(e.Message);
            }
            catch
            {
                PaymentSession.Rollback();
                throw;
            }

            return new BaseDataResult(true, message.Message);
        }

        public IDataResult RemovePayments(BaseParams baseParams)
        {
            var packetIds = baseParams.Params.GetAs<long[]>("packetIds", ignoreCase: true);

            IDataResult message = new BaseDataResult();

            try
            {
                if (EnumerableExtension.IsEmpty(packetIds))
                {
                    return new BaseDataResult(false, "Необходимо выбрать записи");
                }

                Container.InTransaction(() =>
                {
                    var payments = UnacceptedPaymentPacketDomain.GetAll()
                        .WhereIf(packetIds.IsNotEmpty(), x => packetIds.Contains(x.Id));

                    message = RemovePayments(payments);
                });
            }
            catch (ValidationException e)
            {
                return BaseDataResult.Error(e.Message);
            }

            return new BaseDataResult(true, message.Message);
        }
    }
}