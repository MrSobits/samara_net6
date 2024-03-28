namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class MkdChangeNotificationInterceptor : EmptyDomainInterceptor<MkdChangeNotification>
	{
		private const string DirictManaging = "Непосредственное управление";
		private const string RequiredFieldsError = "Не заполнены обязательные поля";

		public IDomainService<RealityObject> RealityObjectDomain { get; set; }
		public IDomainService<RealityObjectFantom> RealityObjectFantomDomain { get; set; }
		public IDomainService<MkdChangeNotificationFile> MkdChangeNotificationFileDomain { get; set; }

		public override IDataResult BeforeCreateAction(IDomainService<MkdChangeNotification> service, MkdChangeNotification entity)
		{
			if (!this.CheckRequiredFields(entity))
			{
				return this.Failure(MkdChangeNotificationInterceptor.RequiredFieldsError);
			}

			var max = service.GetAll()
				.Max(x => (int?) x.RegistrationNumber) ?? 0;

			entity.RegistrationNumber = max + 1;

			this.CheckDirectManaging(entity);

			var stateProvider = this.Container.Resolve<IStateProvider>();
			stateProvider.SetDefaultState(entity);

            return this.SetRealityObjectAddress(entity);
        }

		public override IDataResult BeforeUpdateAction(IDomainService<MkdChangeNotification> service, MkdChangeNotification entity)
        {
			if (!this.CheckRequiredFields(entity))
			{
				return this.Failure(MkdChangeNotificationInterceptor.RequiredFieldsError);
			}

			this.CheckDirectManaging(entity);
			return this.SetRealityObjectAddress(entity);
        }

		public override IDataResult BeforeDeleteAction(IDomainService<MkdChangeNotification> service, MkdChangeNotification entity)
		{
			this.MkdChangeNotificationFileDomain.GetAll()
				.Where(x => x.MkdChangeNotification.Id == entity.Id)
				.Select(x => x.Id)
				.ForEach(x => this.MkdChangeNotificationFileDomain.Delete(x));

			return this.Success();
		}

		private bool CheckRequiredFields(MkdChangeNotification entity)
		{
			if (entity.NotificationCause == null ||
			    entity.InboundNumber == null ||
			    entity.OldMkdManagementMethod == null ||
			    entity.NewMkdManagementMethod == null)
			{
				return false;
			}
		
			return true;
		}

		private void CheckDirectManaging(MkdChangeNotification entity)
		{
			if (entity.OldMkdManagementMethod.Name == MkdChangeNotificationInterceptor.DirictManaging)
			{
				entity.OldManagingOrganization = null;
				entity.OldInn = null;
				entity.OldOgrn = null;
			}

			if (entity.NewMkdManagementMethod.Name == MkdChangeNotificationInterceptor.DirictManaging)
			{
				entity.NewManagingOrganization = null;
				entity.NewInn = null;
				entity.NewOgrn = null;
				entity.NewJuridicalAddress = null;
				entity.NewManager = null;
				entity.NewPhone = null;
				entity.NewEmail = null;
				entity.NewOfficialSite = null;
			}
		}

		private IDataResult SetRealityObjectAddress(MkdChangeNotification entity)
		{
			if (entity.FiasAddress != null)
			{
				if (entity.RealityObjectFantom == null)
				{
					entity.RealityObjectFantom = new RealityObjectFantom();
				}

				entity.RealityObjectFantom.RealityObject = this.RealityObjectDomain.GetAll()
					.FirstOrDefault(x => x.FiasAddress.AddressName == entity.FiasAddress.AddressName);

				if (entity.RealityObjectFantom.RealityObject == null)
				{
					entity.RealityObjectFantom.Fantom = entity.FiasAddress.AddressName;
				    entity.RealityObjectFantom.MunicipalityFantom = Gkh.Utils.Utils.GetMunicipality(this.Container, entity.FiasAddress);
                    entity.RealityObjectFantom.SettlementFantom = this.GetSettlementForReality(entity.FiasAddress);
				}
				else
				{
					entity.RealityObjectFantom.Fantom = null;
				}

				if (entity.RealityObjectFantom.Id > 0)
				{
					this.RealityObjectFantomDomain.Update(entity.RealityObjectFantom);
				}
				else
				{
					this.RealityObjectFantomDomain.Save(entity.RealityObjectFantom);
				}
			}

			return this.Success();
		}

        public Municipality GetSettlementForReality(FiasAddress fiasAddress)
        {
            var fiasDomain = this.Container.ResolveDomain<Fias>();
            var muDomain = this.Container.ResolveRepository<Municipality>();

            try
            {
                if (fiasAddress == null)
                {
                    return null;
                }

                var fias = fiasDomain.FirstOrDefault(x => x.AOGuid == fiasAddress.PlaceGuidId);

                if (fias != null)
                {
                    var oktmo = fias.OKTMO.ToLong().ToString(); // чтобы убрать ведущий 0

                    var mo = muDomain
                            .GetAll()
                            .FirstOrDefault(x => x.Oktmo.ToString() == oktmo);

                    return mo;
                }

                return null;
            }
            finally
            {
                this.Container.Release(fiasDomain);
                this.Container.Release(muDomain);
            }
        }
    }
}