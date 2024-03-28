namespace Bars.Gkh.RegOperator.DomainService.Import.Ches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Contracts.Params;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Интерфейс сопоставления абонентов
    /// </summary>
    public class ChesAccountOwnerComparingService : IChesAccountOwnerComparingService
    {
        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }
        public IDomainService<IndividualAccountOwner> IndividualAccountOwnerDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }
        public IDomainService<ChesMatchAccountOwner> ChesMatchAccountOwnerDomain { get; set; }
        
        public IMapper mapper { get; set; }

        /// <inheritdoc />
        public IDataResult<IEnumerable<ChesMatchAccountOwner>> MatchAutomatically(IQueryable<ChesNotMatchAccountOwner> accountOwners)
        {
            // ReSharper disable once ReplaceWithOfType.3 (OfType не работает в Nhibernate)
            var data = this.MatchLegalAccOwner(accountOwners.Where(x => x is ChesNotMatchLegalAccountOwner).Select(x => (ChesNotMatchLegalAccountOwner)x))
                // ReSharper disable once ReplaceWithOfType.3 (OfType не работает в Nhibernate)
                //.Union(this.MatchIndividualAccOwner(accountOwners.Where(x => x is ChesNotMatchIndAccountOwner).Select(x => (ChesNotMatchIndAccountOwner)x)))
                .ToList();
            
            // TODO: производительность сопоставления физиков...
            data.ForEach(this.ChesMatchAccountOwnerDomain.Save);

            var ownerCountToMatch = accountOwners.Count();
            return new GenericListResult<ChesMatchAccountOwner>(data, data.Count)
            {
                Success = data.Count == ownerCountToMatch,
                Message = $"Сопоставлено {data.Count} абонентов из {ownerCountToMatch}"
            };
        }

        private IEnumerable<ChesMatchAccountOwner> MatchIndividualAccOwner(IQueryable<ChesNotMatchIndAccountOwner> owners)
        {
            var indOwners = this.IndividualAccountOwnerDomain.GetAll()
                .Where(x => owners.Any(y => y.Firstname == x.FirstName && y.Surname == x.Surname && y.Lastname == x.SecondName))
                .Select(x => new IndividualAccountOwnerProxy
                {
                    Id = x.Id,
                    Name = x.Name,
                    BirthDate = x.BirthDate
                })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .Where(this.FilterByBirthDateFunc)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.BirthDate)
                        .ToDictionary(
                            y => y.Key,
                            y => y.ToArray()));

            var persAccOwnerCollection = this.BasePersonalAccountDomain.GetAll()
                .Where(x => x.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual)
                .Select(x => new
                {
                    x.AccountOwner.Id,
                    x.PersonalAccountNum
                })
                .ToDictionary(x => x.PersonalAccountNum, x => x.Id);

            foreach (var chesNotMatchIndAccountOwner in owners)
            {
                Dictionary<DateTime?, IndividualAccountOwnerProxy[]> ownerGroup;
                if (indOwners.TryGetValue(chesNotMatchIndAccountOwner.Name, out ownerGroup))
                {
                    IndividualAccountOwnerProxy[] ownersByDate;
                    IndividualAccountOwnerProxy ownerProxy = null;

                    if (chesNotMatchIndAccountOwner.BirthDate.HasValue)
                    {
                        ownersByDate = ownerGroup.Get(chesNotMatchIndAccountOwner.BirthDate.GetValueOrDefault());

                        if (ownersByDate.Length == 1)
                        {
                            ownerProxy = ownersByDate[0];
                        }
                        else
                        {
                            ownerProxy = ownersByDate
                                .FirstOrDefault(x => x.Id == persAccOwnerCollection.Get(chesNotMatchIndAccountOwner.PersonalAccountNumber));
                        }
                    }
                    else
                    {
                        if (ownerGroup.Count == 1)
                        {
                            ownersByDate = ownerGroup.Values.First();
                            if (ownersByDate.Length == 1)
                            {
                                ownerProxy = ownersByDate[0];
                            }
                            else
                            {
                                ownerProxy = ownersByDate
                                    .FirstOrDefault(x => x.Id == persAccOwnerCollection.Get(chesNotMatchIndAccountOwner.PersonalAccountNumber));
                            }
                        }
                        else
                        {
                            ownerProxy = ownerGroup.SelectMany(x => x.Value)
                                .FirstOrDefault(x => x.Id == persAccOwnerCollection.Get(chesNotMatchIndAccountOwner.PersonalAccountNumber));
                        }
                    }

                    if (ownerProxy.IsNotNull())
                    {
                        var owner = mapper.Map<ChesNotMatchAccountOwner, ChesMatchAccountOwner>(chesNotMatchIndAccountOwner);

                        owner.AccountOwner = new IndividualAccountOwner { Id = ownerProxy.Id };

                        yield return owner;
                    }
                }
            }
        }

        private IEnumerable<ChesMatchAccountOwner> MatchLegalAccOwner(IQueryable<ChesNotMatchLegalAccountOwner> owners)
        {
            var legalOwners = this.LegalAccountOwnerDomain.GetAll()
                .Where(x => owners.Any(y => y.Inn == x.Contragent.Inn && y.Kpp == x.Contragent.Kpp))
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Inn,
                    x.Contragent.Kpp
                })
                .AsEnumerable()
                .GroupBy(x => $"{x.Inn}#{x.Kpp}", x => x.Id)
                .Where(x => x.Count() == 1)
                .ToDictionary(x => x.Key, x => x.First());

            return owners
                .AsEnumerable()
                .Where(x => legalOwners.ContainsKey($"{x.Inn}#{x.Kpp}"))
                .Select(x => new ChesMatchLegalAccountOwner
                {
                    Name = x.Name,
                    Inn = x.Inn,
                    Kpp = x.Kpp,
                    AccountOwner = new LegalAccountOwner { Id = legalOwners[$"{x.Inn}#{x.Kpp}"] },
                    OwnerType = PersonalAccountOwnerType.Legal,
                    PersonalAccountNumber = x.PersonalAccountNumber
                });
        }

        /// <summary>
        /// Метод фильтрации группировок Владельцев
        /// </summary>
        private bool FilterByBirthDateFunc(IGrouping<string, IndividualAccountOwnerProxy> owner)
        {
            var ownerCount = owner.Count();

            // Если 1 абонент, то дальше не проверяем
            if (ownerCount == 1)
            {
                return true;
            }

            // Если абонентов с одинаковым ФИО больше 1 и у всех пустая дата рождения, то исключаем такую группу
            var ownerWithEmptyBirthDate = owner.Count(x => !x.BirthDate.IsValid());
            if (ownerCount > 1 && ownerWithEmptyBirthDate == ownerCount)
            {
                return false;
            }

            // возвращаем результат, т.к. по уникальности даты рождения будем принимать решение дальше
            return true;
        }

        private class IndividualAccountOwnerProxy
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public DateTime? BirthDate { get; set; }
        }
    }
}