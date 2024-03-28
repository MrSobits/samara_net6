namespace Bars.GkhGji.Regions.Nso.Interceptors
{
	using Bars.B4;
	using Bars.B4.Modules.States;
	using Bars.B4.Utils;
	using Bars.Gkh.Entities;
	using Bars.GkhGji.Regions.Nso.Entities;
	using System.Linq;
	using B4.DataAccess;
	using B4.Modules.FIAS;

    public class MkdChangeNotificationInterceptor : EmptyDomainInterceptor<MkdChangeNotification>
	{
		private const string DirictManaging = "Непосредственное управление";
		private const string RequiredFieldsError = "Не заполнены обязательные поля";

		public IDomainService<RealityObject> RealityObjectDomain { get; set; }
		public IDomainService<RealityObjectFantom> RealityObjectFantomDomain { get; set; }
		public IDomainService<MkdChangeNotificationFile> MkdChangeNotificationFileDomain { get; set; }

		public override IDataResult BeforeCreateAction(IDomainService<MkdChangeNotification> service, MkdChangeNotification entity)
		{
			if (!CheckRequiredFields(entity))
			{
				return Failure(RequiredFieldsError);
			}

			var max = service.GetAll()
				.Max(x => (int?) x.RegistrationNumber) ?? 0;

			entity.RegistrationNumber = max + 1;

			CheckDirectManaging(entity);

			var stateProvider = Container.Resolve<IStateProvider>();
			stateProvider.SetDefaultState(entity);

            return SetRealityObjectAddress(entity);
        }

		public override IDataResult BeforeUpdateAction(IDomainService<MkdChangeNotification> service, MkdChangeNotification entity)
        {
			if (!CheckRequiredFields(entity))
			{
				return Failure(RequiredFieldsError);
			}

			CheckDirectManaging(entity);
			return SetRealityObjectAddress(entity);
        }

		public override IDataResult BeforeDeleteAction(IDomainService<MkdChangeNotification> service, MkdChangeNotification entity)
		{
			MkdChangeNotificationFileDomain.GetAll()
				.Where(x => x.MkdChangeNotification.Id == entity.Id)
				.Select(x => x.Id)
				.ForEach(x => MkdChangeNotificationFileDomain.Delete(x));

			return Success();
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
			if (entity.OldMkdManagementMethod.Name == DirictManaging)
			{
				entity.OldManagingOrganization = null;
				entity.OldInn = null;
				entity.OldOgrn = null;
			}

			if (entity.NewMkdManagementMethod.Name == DirictManaging)
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

				entity.RealityObjectFantom.RealityObject = RealityObjectDomain.GetAll()
					.FirstOrDefault(x => x.FiasAddress.AddressName == entity.FiasAddress.AddressName);

				if (entity.RealityObjectFantom.RealityObject == null)
				{
					entity.RealityObjectFantom.Fantom = entity.FiasAddress.AddressName;
				    entity.RealityObjectFantom.MunicipalityFantom = Gkh.Utils.Utils.GetMunicipality(Container, entity.FiasAddress);
                    entity.RealityObjectFantom.SettlementFantom = GetSettlementForReality(entity.FiasAddress);
				}
				else
				{
					entity.RealityObjectFantom.Fantom = null;
				}

				if (entity.RealityObjectFantom.Id > 0)
				{
					RealityObjectFantomDomain.Update(entity.RealityObjectFantom);
				}
				else
				{
					RealityObjectFantomDomain.Save(entity.RealityObjectFantom);
				}
			}

			return Success();
		}

        public Municipality GetSettlementForReality(FiasAddress fiasAddress)
        {
            var fiasDomain = Container.ResolveDomain<Fias>();
            var muDomain = Container.ResolveRepository<Municipality>();

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
                Container.Release(fiasDomain);
                Container.Release(muDomain);
            }
        }
    }
}