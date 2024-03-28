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

    /// <summary>
    /// ИНН для загрузки в паспорт МКД
    /// </summary>
    public class ManOrgInnDataFiller : IDataFiller
    {
        /// <summary>
        /// Code
        /// </summary> 
        public static string Code
        {
            get
            {
                return "ManOrgInnDataFiller";
            }
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Атрибут, значения которого надо проставить
        /// </summary>
        public MetaAttribute MetaAttribute { get; set; }
        
        /// <summary>
        /// Дом, на основе которого берутся данные
        /// </summary>
        public RealityObject RealityObject { get; set; }

        /// <summary>
        /// Возвращаемый результат
        /// </summary>
        public List<BaseProviderPassportRow> Result { get; set; }
        
        /// <summary>
        /// Gets the code.
        /// </summary>
        string IDataFiller.Code
        {
            get
            {
                return "ManOrgInnDataFiller";
            }
        }

        /// <summary>
        /// Название
        /// </summary>
        public string Name
        {
            get
            {
                return "ИНН";
            }
        }

        /// <summary>
        /// Gets a value indicating whether multiple.
        /// </summary>
        public bool Multiple
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Тип данных
        /// </summary>
        public ValueType ValueType
        {
            get
            {
                return ValueType.Int;
            }
        }

        /// <summary>
        /// Загрузка
        /// </summary>
        public void To1468()
        {
            if (RealityObject == null || MetaAttribute == null || this.Result == null)
            {
                throw new Exception("Не переданы параметры для расчета значения!");
            }

            var contractData = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                .GetAll()
                .Where(x => x.RealityObject.Id == RealityObject.Id)
                .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= DateTime.Now)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now)
                .Select(x => new { x.ManOrgContract.Id, x.ManOrgContract.ManagingOrganization.Contragent.Inn })
                .AsEnumerable()
                .Distinct()
                .ToDictionary(x => x.Id, x => x.Inn);

            string result;
            if (contractData.Count > 1)
            {
                var contractId = this.Container.Resolve<IDomainService<ManOrgContractRelation>>().GetAll()
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
            
            this.Result.Add(new BaseProviderPassportRow
            {
                MetaAttribute = MetaAttribute,
                Value = result ?? string.Empty
            });
        }

        /// <summary>
        /// Выгрузка
        /// </summary>
        public void From1468()
        {
            // Пока не придумали
        }
    }
}
