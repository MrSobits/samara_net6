namespace Bars.Gkh.Import.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;

    class AddressUidImporter : BaseBillingImporter<FiasAddressUid>
    {
        public override string FileName
        {
            get { return "addressUids"; }
        }

        public override int Order
        {
            get { return 10; }
        }

        public override int SplitCount
        {
            get { return 3; }
        }

        public IDomainService<FiasAddressUid> AddressUidService { get; set; }

        public IDomainService<FiasAddress> AddressService { get; set; }

        private Dictionary<long, FiasAddress> _fiasAddr;

        private Dictionary<long, FiasAddress> FiasAddr
        {
            get
            {
                return _fiasAddr ??
                       (_fiasAddr =
                           AddressService.GetAll().ToList().ToDictionary(x => x.Id, arg => arg));
            }
        }

        private Dictionary<long, List<FiasAddressUid>> _adrUids;

        private Dictionary<long, List<FiasAddressUid>> AdrUids
        {
            get
            {
                return _adrUids ??
                       (_adrUids =
                           AddressUidService.GetAll()
                           .ToList()
                           .GroupBy(x => x.Address.Id)
                           .ToDictionary(x => x.Key, arg => arg.ToList()));
            }
        }

        public override void ProcessLine(string archiveName, string[] splits, ILogImport logger)
        {
            var adrId = splits[0].ToLong();

            if (FiasAddr.ContainsKey(adrId))
            {
                var adr = FiasAddr[adrId];

                if (AdrUids.ContainsKey(adr.Id))
                {
                    var adrUid = AdrUids[adr.Id];

                    if (adrUid.FirstOrDefault(x => x.Uid == splits[1] && x.BillingId == splits[2]) != null)
                    {
                        return;
                    }

                    var newEl = new FiasAddressUid
                    {
                        Address = adr,
                        Uid = splits[1],
                        BillingId = splits[2]
                    };

                    Add(newEl);

                    adrUid.Add(newEl);
                }
                else
                {
                    var adrUid = new FiasAddressUid
                    {
                        Address = adr,
                        Uid = splits[1],
                        BillingId = splits[2]
                    };

                    Add(adrUid);

                    AdrUids.Add(adrUid.Address.Id, new List<FiasAddressUid> { adrUid });
                }
            }
        }

        public override Func<FiasAddressUid, bool> Predicate(FiasAddressUid other)
        {
            return x => true;
        }
    }
}
