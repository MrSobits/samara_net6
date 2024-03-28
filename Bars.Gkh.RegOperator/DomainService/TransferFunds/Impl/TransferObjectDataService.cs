namespace Bars.Gkh.RegOperator.DomainService.TransferFunds.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.GkhRf.DomainService;

    public class TransferObjectDataService : ITransferObjectDataService
    {
        public ITransferObjectService TransferObjectService { get; set; }

        public Dictionary<long, decimal> GetPaids(DateTime date, IQueryable<long> chargeAccountRoIds)
        {
            return this.TransferObjectService.GetPaids(date, chargeAccountRoIds);
        }
    }
}
