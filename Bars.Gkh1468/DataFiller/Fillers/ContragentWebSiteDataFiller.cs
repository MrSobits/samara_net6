namespace Bars.Gkh1468.DataFiller.Fillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh1468.Entities;
    using Castle.Windsor;

    using ValueType = Bars.Gkh1468.Enums.ValueType;

    public class ContragentWebSiteDataFiller : IDataFiller
    {
        public IWindsorContainer Container { get; set; }

        // Атрибут, значения которого надо проставить
        public MetaAttribute MetaAttribute { get; set; }

        // Дом, на основе которого берутся данные
        public RealityObject RealityObject { get; set; }

        public List<BaseProviderPassportRow> Result { get; set; }

        string IDataFiller.Code
        {
            get
            {
                return Code;
            }
        }

        public static string Code
        {
            get
            {
                return "ContragentWebSiteDataFiller";
            }
        }

        public string Name
        {
            get
            {
                return "Официальный сайт";
            }
        }

        public bool Multiple
        {
            get
            {
                return false;
            }
        }

        public ValueType ValueType
        {
            get
            {
                return ValueType.String;
            }
        }

        public void To1468()
        {
            if (RealityObject == null || MetaAttribute == null || Result == null)
            {
                throw new Exception("Не переданы параметры для расчета значения!");
            }

            var contractData = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                .GetAll()
                .Where(x => x.RealityObject.Id == RealityObject.Id)
                .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= DateTime.Now)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now)
                .Select(x => new { x.ManOrgContract.Id, x.ManOrgContract.ManagingOrganization.Contragent.OfficialWebsite })
                .AsEnumerable()
                .Distinct()
                .ToDictionary(x => x.Id, x => x.OfficialWebsite);

            var result = string.Empty;

            if (contractData.Count > 1)
            {
                var contractId =
                    this.Container.Resolve<IDomainService<ManOrgContractRelation>>().GetAll()
                   .Where(x => contractData.Keys.Contains(x.Parent.Id))
                   .Where(x => contractData.Keys.Contains(x.Children.Id))
                   .Where(x => x.TypeRelation == TypeContractRelation.TransferTsjUk)
                   .Select(x => (long?)x.Children.Id)
                   .FirstOrDefault();

                result = contractId.HasValue ? contractData[contractId.Value] : contractData.First().Value;
            }
            else
            {
                result = contractData.Any() ? contractData.First().Value : string.Empty;
            }

            Result.Add(new BaseProviderPassportRow
            {
                MetaAttribute = MetaAttribute,
                Value = result
            });
        }

        // Пока не придумали - оставляем пустым
        public void From1468()
        {
        }
    }
}