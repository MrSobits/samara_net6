namespace Bars.Gkh.RegOperator.DataProviders
{
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Castle.Windsor;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Провадйер для отчета по лицевому счету
    /// </summary>
    public class PersonalAccountClaimworkDataProvider : BaseCollectionDataProvider<PersonalAccountProxy>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container"></param>
        public PersonalAccountClaimworkDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Получение данных по лицевому счету
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        protected override IQueryable<PersonalAccountProxy> GetDataInternal(BaseParams baseParams)
        {
            var record = new PersonalAccountProxy { Id = this.AccountId.ToString() };

            var records = new List<PersonalAccountProxy> { record };

            return records.AsQueryable();
        }

        /// <summary>
        /// Наименование провайдера отчета
        /// </summary>
        public override string Name
        {
            get { return "Справка по лицевому счету"; }
        }

        /// <summary>
        /// Ключ провайдера
        /// </summary>
        public override string Key
        {
            get { return typeof(PersonalAccountReferenceDataProvider).Name; }
        }

        /// <summary>
        /// Описание провайдера
        /// </summary>
        public override string Description
        {
            get { return Name; }
        }

        /// <summary>
        /// Id лс
        /// </summary>
        public long AccountId { get; set; }
    }
}