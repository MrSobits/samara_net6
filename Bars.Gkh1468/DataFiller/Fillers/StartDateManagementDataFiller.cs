namespace Bars.Gkh1468.DataFiller.Fillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh1468.Entities;

    using Castle.Windsor;

    using ValueType = Bars.Gkh1468.Enums.ValueType;

    public class StartDateManagementDataFiller : IDataFiller
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
                return "StartDateManagementDataFiller";
            }
        }

        public string Name
        {
            get
            {
                return "Дата начала управления домом";
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
                .Select(x => new { contId = x.ManOrgContract.Id, value = x.ManOrgContract.StartDate })
                .AsEnumerable()
                .Distinct()
                .ToDictionary(x => x.contId, v => v.value);

            DateTime? dateStart = null;

            if (contractData.Count > 1)
            {
                var contrId =
                    this.Container.Resolve<IDomainService<ManOrgContractRelation>>().GetAll()
                   .Where(x => contractData.Keys.Contains(x.Parent.Id))
                   .Where(x => contractData.Keys.Contains(x.Children.Id))
                   .Where(x => x.TypeRelation == TypeContractRelation.TransferTsjUk)
                   .Select(x => (long?)x.Children.Id)
                   .FirstOrDefault();

                dateStart = contrId.HasValue ? contractData[contrId.ToLong()] : contractData.First().Value;
            }
            else
            {
                dateStart = contractData.First().Value;
            }            

            var result = dateStart.HasValue ? dateStart.ToDateTime().ToShortDateString() : string.Empty;
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