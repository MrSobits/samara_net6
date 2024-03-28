namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities;
    using Enums;

    public sealed class BankDocumentResolver
    {
        private readonly IWindsorContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public BankDocumentResolver(IWindsorContainer container)
        {
            _container = container;
        }

        public void ResolveUnacceptedPayments()
        {
            var unacceptedPaymentRepo = _container.ResolveRepository<UnacceptedPayment>();
            var documentRepo = _container.ResolveRepository<ImportedPayment>();

            var sessions = _container.Resolve<ISessionProvider>();

            var join = documentRepo.GetAll()
                .Select(x => new
                {
                    Key = string.Format(
                        "{0}#{1}#{2}#{3}",
                        x.BankDocumentImport.DocumentNumber ?? "",
                        x.Account,
                        x.Sum,
                        x.PaymentDate),
                    x.Id
                })
                .ToList()
                .Join(unacceptedPaymentRepo.GetAll()
                    .Where(x => !documentRepo.GetAll()
                        .Any(d => d.PaymentState == ImportedPaymentState.Rno && d.PaymentId == x.Id))
                    .Select(pay => new
                    {
                        Key = string.Format(
                            "{0}#{1}#{2}#{3}",
                            pay.DocNumber ?? "",
                            pay.PersonalAccount.PersonalAccountNum,
                            pay.Sum,
                            pay.PaymentDate),
                        pay.Id
                    }).ToList(),
                    imported => imported.Key,
                    pay => pay.Key,
                    (imported, pay) => new
                    {
                        RecordId = imported.Id,
                        PayId = pay.Id
                    })
                .Distinct()
                .ToList();

            var update = "update REGOP_IMPORTED_PAYMENT set payment_id = :payId where Id = :id";

            var session = sessions.GetCurrentSession();

            foreach (var item in join)
            {
                session.CreateSQLQuery(update)
                    .SetInt64("id", item.RecordId)
                    .SetInt64("payId", item.PayId)
                    .ExecuteUpdate();
            }
        }

        public ListDataResult GetUnresolvedDocs(BaseParams @params)
        {
            var type = @params.Params.GetAs<ImportedPaymentState>("type");

            var loadParams = @params.GetLoadParam();

            var docRepo = _container.ResolveRepository<ImportedPayment>();

            var data = docRepo.GetAll()
                .Where(x => x.PaymentId == null && x.PaymentState == type)
                .Select(x => new DocumentForResolveDto
                {
                    DocPaymentRecordId = x.Id,
                    AccountNum = x.Account,
                    PaymentDate = x.PaymentDate,
                    Sum = x.Sum
                })
                .Filter(loadParams, _container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }

        public ListDataResult GetSuspects(ImportedPaymentState type, BaseParams @params)
        {
            var loadParams = @params.GetLoadParam();

            if (type == ImportedPaymentState.Rno)
                return GetSuspectsFromPayments(loadParams);

            return GetSuspectsFromSuspense(loadParams);
        }

        public void Resolve(IEnumerable<DocumentForResolveDto> dtos)
        {
            var update = "update REGOP_IMPORTED_PAYMENT set payment_id = :entityId where id = :id";
            var sessions = _container.Resolve<ISessionProvider>();

            var session = sessions.OpenStatelessSession();

            foreach (var dto in dtos)
            {
                session.CreateSQLQuery(update)
                    .SetInt64("entityId", dto.EntityId)
                    .SetInt64("id", dto.DocPaymentRecordId)
                    .ExecuteUpdate();
            }
        }
        
        private ListDataResult GetSuspectsFromSuspense(LoadParam @params)
        {
            var targetRepo = _container.ResolveRepository<SuspenseAccount>();
            var docRepo = _container.ResolveRepository<ImportedPayment>();

            var data = targetRepo.GetAll()
                .Where(x => !docRepo.GetAll().Any(d => d.PaymentState == ImportedPaymentState.Rns && d.PaymentId == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.AccountBeneficiary,
                    x.Sum,
                    x.DateReceipt,
                    x.DistributeState
                })
                .Select(x => new DocumentForResolveDto
                {
                    EntityId = x.Id,
                    AccountNum = x.AccountBeneficiary,
                    PaymentDate = x.DateReceipt,
                    State = (int) x.DistributeState,
                    Sum = x.Sum
                })
                .Filter(@params, _container);

            return new ListDataResult(data.Order(@params).Paging(@params), data.Count());
        }

        private ListDataResult GetSuspectsFromPayments(LoadParam @params)
        {
            var targetRepo = _container.ResolveRepository<UnacceptedPayment>();
            var docRepo = _container.ResolveRepository<ImportedPayment>();

            var data = targetRepo.GetAll()
                .Where(x => !docRepo.GetAll().Any(d => d.PaymentState == ImportedPaymentState.Rno && d.PaymentId == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.PersonalAccount.PersonalAccountNum,
                    x.PaymentDate,
                    x.Packet.State,
                    x.Sum
                })
                .Select(x => new DocumentForResolveDto
                {
                    EntityId = x.Id,
                    AccountNum = x.PersonalAccountNum,
                    PaymentDate = x.PaymentDate,
                    State = (int) x.State,
                    Sum = x.Sum
                })
                .Filter(@params, _container);
            
            return new ListDataResult(data.Order(@params).Paging(@params), data.Count());
        }
    }

    public sealed class DocumentForResolveDto
    {
        public long DocPaymentRecordId { get; set; }

        public long EntityId { get; set; }

        public string AccountNum { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Sum { get; set; }

        public int State { get; set; }
    }
}