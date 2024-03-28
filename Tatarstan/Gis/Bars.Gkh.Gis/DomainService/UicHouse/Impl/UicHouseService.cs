namespace Bars.Gkh.Gis.DomainService.UicHouse.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Utils;
    using Castle.Windsor;
    using Entities.House;
    using Entities.PersonalAccount;
    using Entities.Register.HouseRegister;
    using Gkh.Entities;

    /// <summary>
    /// Старый режим.
    /// Совсем не работает (раньше работал навьюшках из БД)
    /// </summary>
    public class UicHouseService : IUicHouseService
    {
        private readonly IDomainService<RealityObject> _realityObjectRepository;
        private readonly IDomainService<GisPersonalAccount> _personalAccountRepository;
        private readonly IDomainService<PersonalAccountUic> _personalAccountUicRepository;
        private readonly IDomainService<GisHouseProxy> _houseRepository;
        private readonly IDomainService<Fias> _fiasRepository;
        public IWindsorContainer Container { get; set; }
        public UicHouseService(IDomainService<RealityObject> realityObjectRepository,
            IDomainService<GisPersonalAccount> personalAccountRepository,
            IDomainService<PersonalAccountUic> personalAccountUicRepository,
            IDomainService<GisHouseProxy> houseRepository,
            IDomainService<Fias> fiasRepository)
        {
            _realityObjectRepository = realityObjectRepository;
            _personalAccountRepository = personalAccountRepository;
            _personalAccountUicRepository = personalAccountUicRepository;
            _houseRepository = houseRepository;
            _fiasRepository = fiasRepository;
        }

        public void GenerateUic(HouseRegister houseRegister)
        {
            throw new NotImplementedException("Метод устарел, не работает в данной архитектуре!");
            /*
            var tmpHouseRegister = houseRegister;

            var listHouse = _realityObjectRepository
                .GetAll()
                .Where(x => x.FiasAddress.Id == tmpHouseRegister.FiasAddress.Id)
                .Select(x => x.CodeErc)
                .Join(
                    _houseRepository.GetAll(),
                    code => code,
                    house => house.Code,
                    (code, house) => house.Id)
                .ToList();

            var personalAccounts = _personalAccountRepository
                    .GetAll()
                    .Where(x => listHouse.Contains(x.HouseId))
                    .Select(k => new
                    {
                        k.Id,
                        k.AccountId,
                        k.PSS,
                        k.PaymentCode,
                        k.ApartmentNumber,
                        k.RoomNumber,
                        k.Prefix,
                        k.IsOpen,
                    });

            if (!personalAccounts.Any())
            {
                return;
            }

            var accsWithUic = personalAccounts.Join(
                _personalAccountUicRepository.GetAll(),
                p => new { PersonalAccountId = p.Id, AccountNumber = p.AccountId },
                puic => new { puic.PersonalAccountId, puic.AccountNumber },
                (p, puic) => new
                {
                    p.Id,
                    p.AccountId,
                    p.PSS,
                    p.PaymentCode,
                    p.ApartmentNumber,
                    p.RoomNumber,
                    p.Prefix,
                    p.IsOpen,
                    PersAccUicId = puic.Id,
                    puic.Uic,
                    HouseRegisterId = puic.HouseRegister.Id
                });

            if (accsWithUic.Any(x => x.HouseRegisterId == tmpHouseRegister.Id))
            {
                // УИК уже сгенерирован для этого ЛС
                return;
            }

            var maxUic = accsWithUic.Any()
                ? accsWithUic
                    .Select(k => k.Uic.Substring(13, 7).ToInt())
                    .ToList()
                    .Max()
                : 0;

            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var listPersonalAccUic = new List<PersonalAccountUic>();
                    foreach (var personalAccount in personalAccounts)
                    {
                        var newPersonalAccountUic = new PersonalAccountUic
                        {
                            Uic = GetGeneratedUic(tmpHouseRegister.FiasAddress, ++maxUic),
                            AccountNumber = personalAccount.AccountId,
                            FlatNumber = personalAccount.ApartmentNumber,
                            HouseRegister = tmpHouseRegister,
                            PersonalAccountId = personalAccount.Id
                        };

                        listPersonalAccUic.Add(newPersonalAccountUic);
                    }

                    listPersonalAccUic.ForEach(x => session.Insert(x));
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
          */
        }

        private string GetGeneratedUic(FiasAddress fiasAddress, int counter)
        {
            var fias = _fiasRepository
                    .GetAll()
                    .FirstOrDefault(x => x.AOGuid == fiasAddress.StreetGuidId);

            return string.Format("{0}{1}{2}{3}{4}{5}01",
                string.IsNullOrWhiteSpace(fiasAddress.StreetCode) ? "0" : "1",
                fias.Return(x => x.CodeArea, "000"),
                fias.Return(x => x.CodeCity, "000"),
                fias.Return(x => x.CodeCtar, "000"),
                fias.Return(x => x.CodePlace, "000"),
                counter.ToString("0000000"));
        }
    }
}
