namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Gkh.Entities;
    using System.Text;

    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class WarningInspectionService : IWarningInspectionService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AppealCitsStatSubject> AppealStatSubjectDomain { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        /// <inheritdoc />
        public IDataResult CheckAppealCits(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("appealCitizensId");
            return this.CheckAppealCits(this.AppealCitsDomain.Load(id));
        }

        /// <inheritdoc />
        public IDataResult CreateWithAppealCits(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var warningInspectionDomain = this.Container.Resolve<IDomainService<WarningInspection>>();
                var inspectionRobjectDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

                try
                {
                    var contragentId = baseParams.Params.GetAs<long>("contragentId");
                    var warningInspection = baseParams.Params.GetAs<WarningInspection>("baseStatement");
                    var appealCitsId = baseParams.Params.GetAs<long>("appealCits");
                    var realityObjId = baseParams.Params.GetAs<long>("realtyObjId");

                    if (contragentId > 0)
                    {
                        warningInspection.Contragent = new Contragent{ Id = contragentId };
                    }

                    warningInspection.SourceFormType = TypeBase.CitizenStatement;
                    warningInspection.InspectionBasis = InspectionCreationBasis.AppealCitizens;
                    warningInspection.AppealCits = new AppealCits() { Id = appealCitsId };

                    warningInspectionDomain.Save(warningInspection);
                    
                    if (realityObjId > 0)
                    {
                        var newInspRo = new InspectionGjiRealityObject
                        {
                            Inspection = warningInspection,
                            RealityObject = new RealityObject(){Id = realityObjId}
                        };

                        inspectionRobjectDomain.Save(newInspRo);
                    }

                    transaction.Commit();

                    return new BaseDataResult{ Success = true, Data = warningInspection };
                }
                catch (ValidationException exc)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = exc.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(warningInspectionDomain);
                    this.Container.Release(inspectionRobjectDomain);
                }
            }
        }

        /// <inheritdoc />
        public IDataResult ListByAppealCits(BaseParams baseParams)
        {
            var inspectionAppCittsDomain = this.Container.Resolve<IDomainService<WarningInspection>>();
            var warningDocDomain = this.Container.Resolve<IDomainService<WarningDoc>>();
            var inspectionGjiRealityObjectDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            using (this.Container.Using(inspectionAppCittsDomain,
                       warningDocDomain,
                       inspectionGjiRealityObjectDomain))
            {
                var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");

                var warningInspectionQuery = inspectionAppCittsDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitizensId);

                var warningDocDict = warningDocDomain.GetAll()
                    .Where(x => warningInspectionQuery.Any(y => y.Id == x.Inspection.Id))
                    .Select(x => new
                    {
                        x.Inspection.Id,
                        x.DocumentNumber,
                        x.DocumentDate
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => new { x.DocumentDate, x.DocumentNumber })
                    .ToDictionary(x => x.Key, y => y.First());

                var realtyObjDict = inspectionGjiRealityObjectDomain.GetAll()
                    .Where(x => warningInspectionQuery.Any(y => y.Id == x.Inspection.Id))
                    .Select(x => new
                    {
                        x.Inspection.Id,
                        x.RealityObject.Address
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.Address)
                    .ToDictionary(x => x.Key, x => x.AggregateWithSeparator(y => y, ", "));

                return warningInspectionQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.InspectionNumber,
                        x.TypeBase
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.InspectionNumber,
                        x.TypeBase,
                        DocumentNumber = warningDocDict.Get(x.Id).ReturnSafe(y => y.DocumentNumber),
                        DocumentDate = warningDocDict.Get(x.Id).ReturnSafe(y => y.DocumentDate),
                        RealtyObject = realtyObjDict.Get(x.Id)
                    })
                    .ToListDataResult(baseParams.GetLoadParam());
            }
        }

        private IDataResult CheckAppealCits(AppealCits appealCits)
        {
            var msg = new StringBuilder();

            if (!this.CheckThematics(appealCits.Id))
                msg.AppendLine("не заполнены тематики;");

            if (!this.CheckAppealCitsRealityObjects(appealCits.Id))
                msg.AppendLine("не заполнено место возникновения проблемы;");

            if (!this.CheckAppealCitsConsideration(appealCits))
                msg.AppendLine("не заполнено одно или несколько полей вкладки \'Рассмотрение\'");

            if (msg.Length != 0)
            {
                msg.Insert(0, $"Ошибки заполнения обращения {appealCits.DocumentNumber}: ");
            }

            return new BaseDataResult(msg.Length == 0, msg.ToString());
        }

        private bool CheckAppealCitsConsideration(AppealCits appealCits)
        {
            if (appealCits.ExecuteDate == null ||
                appealCits.SuretyDate == null ||
                appealCits.Surety == null ||
                appealCits.Executant == null ||
                appealCits.SuretyResolve == null)
            {
                return false;
            }

            return true;
        }

        private bool CheckThematics(long id)
        {
            return this.AppealStatSubjectDomain.GetAll().Any(x => x.AppealCits.Id == id);
        }

        private bool CheckAppealCitsRealityObjects(long id)
        {
            var servAppealCitRealObj = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();

            return servAppealCitRealObj.GetAll().Any(x => x.AppealCits.Id == id);
        }
    }
}