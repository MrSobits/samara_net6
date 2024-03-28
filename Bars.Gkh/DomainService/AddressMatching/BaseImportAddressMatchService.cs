namespace Bars.Gkh.DomainService.AddressMatching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;

    using Castle.Windsor;

    /// <summary>
    /// Базовый класс сервиса сопоставления адресов
    /// </summary>
    public abstract class BaseImportAddressMatchService : IImportAddressMatchService
    {
        /// <summary>
        /// Группы сопоставления
        /// </summary>
        protected abstract IDictionary<string, double> MatchGroups { get; }

        /// <summary>
        /// Паттерн импортируемого адреса
        /// </summary>
        protected abstract Regex ImportAddressRegex { get; }

        /// <summary>
        /// Паттерн адреса в системе
        /// </summary>
        protected abstract Regex LocalAddressRegex { get; }

        /// <summary>
        /// Порог ошибки
        /// </summary>
        protected virtual double ErrorThreshold { get; } = .51;

        public IWindsorContainer Container { get; set; }

        public IDomainService<AddressMatch> AddressMatchDomain { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        /// <inheritdoc />
        public virtual IDataResult<IEnumerable<AddressMatch>> MatchAutomatically(params AddressMatchDto[] addreses)
        {
            ArgumentChecker.NotNull(addreses, nameof(addreses));
            var now = DateTime.Now;

            var realityObjectCache = this.RealityObjectDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    HouseGuid = x.FiasAddress.HouseGuid.ToString(),
                    x.FiasAddress.PlaceName,
                    x.FiasAddress.AddressName
                })
                .AsEnumerable()
                .Select(x =>
                {
                    var match = this.LocalAddressRegex.Match(x.AddressName);
                    var matchArray = this.MatchGroups
                        .Select(y =>
                            match.Groups[y.Key]
                                .Captures
                                .Cast<Capture>()
                                .Select(str => str.Value.ToLower().Replace(".", ""))
                                .FirstOrDefault(z => z.IsNotEmpty()))
                        .ToArray();

                    if (matchArray.All(z => z.IsEmpty()))
                    {
                        return null;
                    }

                    return new
                    {
                        x.Id,
                        x.AddressName,
                        x.HouseGuid,
                        x.PlaceName,
                        MatchAddress = matchArray
                    };
                })
                .Where(x => x.IsNotNull())
                .ToArray();

            var maxEvualation = this.MatchGroups.Values.Sum();
            var matchWeights = this.MatchGroups.Values.ToArray();

            var matchResult = addreses
                .Where(x => x.HouseGuid.IsNotEmpty())
                .Join(
                    realityObjectCache,
                    x => x.HouseGuid,
                    x => x.HouseGuid,
                    (x, y) => new AddressMatch
                    {
                        ObjectCreateDate = now,
                        ObjectEditDate = now,
                        ExternalAddress = x.Address,
                        HouseGuid = x.HouseGuid,
                        RealityObject = new RealityObject { Id = y.Id }

                    })
                .ToList();

            addreses
                .Where(x => matchResult.All(y => y.ExternalAddress != x.Address))
                .Select(x =>
                {
                    var match = this.ImportAddressRegex.Match(x.Address);
                    var addressMatches = this.MatchGroups
                        .Select(y =>
                            match.Groups[y.Key]
                                .Captures
                                .Cast<Capture>()
                                .Select(str => str.Value.ToLower().Replace(".", ""))
                                .FirstOrDefault(z => z.IsNotEmpty()))
                        .ToArray();

                    var matchAddress = realityObjectCache
                        .AsParallel()
                        .Select(add => new
                        {
                            Address = add,
                            Evualation = addressMatches.Select((s, i) => add.MatchAddress[i] == s ? matchWeights[i] : 0d).Sum()
                        })
                        .Where(pair => pair.Evualation >= maxEvualation - this.ErrorThreshold)
                        .OrderByDescending(pair => pair.Evualation)
                        .ToArray();

                    return new
                    {
                        Address = x,
                        MatchAddress = matchAddress.FirstOrDefault()
                    };
                })
                .Where(x => x.MatchAddress.IsNotNull())
                .Select(x => new AddressMatch
                {
                    ObjectCreateDate = now,
                    ObjectEditDate = now,
                    ExternalAddress = x.Address.Address,
                    HouseGuid = x.Address.HouseGuid,
                    RealityObject = new RealityObject { Id = x.MatchAddress.Address.Id }
                })
                .AddTo(matchResult);

            this.Container.InStatelessTransaction(session =>
            {
                matchResult.ForEach(match => session.Insert(match));
            });

            return new GenericDataResult<IEnumerable<AddressMatch>>(matchResult, $"Сопоставлено {matchResult.Count} адресов из {addreses.Length}")
            {
                Success = matchResult.Count == addreses.Length
            };
        }
    }
}
