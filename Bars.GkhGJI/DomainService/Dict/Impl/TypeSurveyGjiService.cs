namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class TypeSurveyGjiService : ITypeSurveyGjiService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddAdministrativeRegulations(BaseParams baseParams)
        {
            var service = Container.ResolveDomain<TypeSurveyAdminRegulationGji>();
            try
            {
                var typeSurveyId = baseParams.Params.ContainsKey("typeSurveyId")
                                       ? baseParams.Params["typeSurveyId"].ToLong()
                                       : 0;

                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                    ? baseParams.Params["objectIds"].ToString()
                                    : string.Empty;

                if (typeSurveyId == 0 || string.IsNullOrEmpty(objectIds))
                {
                    return new BaseDataResult
                               {
                                   Success = false,
                                   Message = "Не удалось получить тип обследования и/или виды обследования"
                               };
                }

                var existRecords =
                    service.GetAll().Where(x => x.TypeSurvey.Id == typeSurveyId).Select(x => x.NormativeDoc.Id).ToList();

                foreach (var id in objectIds.ToLongArray())
                {
                    if (!existRecords.Contains(id))
                    {
                        var newRec = new TypeSurveyAdminRegulationGji
                                         {
                                             NormativeDoc = new NormativeDoc { Id = id },
                                             TypeSurvey =
                                                 new TypeSurveyGji { Id = typeSurveyId }
                                         };

                        service.Save(newRec);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                Container.Release(service);
            }
        }

        public IDataResult AddGoals(BaseParams baseParams)
        {
            var service = Container.ResolveDomain<TypeSurveyGoalInspGji>();
            try
            {
                var typeSurveyId = baseParams.Params.ContainsKey("typeSurveyId")
                                       ? baseParams.Params["typeSurveyId"].ToLong()
                                       : 0;

                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                    ? baseParams.Params["objectIds"].ToString()
                                    : string.Empty;

                if (typeSurveyId == 0 || string.IsNullOrEmpty(objectIds))
                {
                    return new BaseDataResult
                               {
                                   Success = false,
                                   Message = "Не удалось получить тип обследования и/или виды обследования"
                               };
                }

                var existRecords =
                    service.GetAll()
                           .Where(x => x.TypeSurvey.Id == typeSurveyId)
                           .Select(x => x.SurveyPurpose.Id)
                           .ToList();

                foreach (var id in objectIds.ToLongArray())
                {
                    if (!existRecords.Contains(id))
                    {
                        var newRec = new TypeSurveyGoalInspGji
                                         {
                                             SurveyPurpose = new SurveyPurpose { Id = id },
                                             TypeSurvey = new TypeSurveyGji { Id = typeSurveyId }
                                         };

                        service.Save(newRec);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                Container.Release(service);
            }
        }

        public IDataResult AddTaskInsp(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<TypeSurveyTaskInspGji>>();
            try
            {
                var typeSurveyId = baseParams.Params.ContainsKey("typeSurveyId")
                                       ? baseParams.Params["typeSurveyId"].ToLong()
                                       : 0;

                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                    ? baseParams.Params["objectIds"].ToString()
                                    : string.Empty;

                if (typeSurveyId == 0 || string.IsNullOrEmpty(objectIds))
                {
                    return new BaseDataResult
                    {
                        Success = false,
                        Message = "Не удалось получить тип обследования и/или виды обследования"
                    };
                }

                var existRecords =
                    service.GetAll().Where(x => x.TypeSurvey.Id == typeSurveyId).Select(x => x.SurveyObjective.Id).ToList();

                foreach (var id in objectIds.ToLongArray())
                {
                    if (!existRecords.Contains(id))
                    {
                        var newRec = new TypeSurveyTaskInspGji
                        {
                            SurveyObjective = new SurveyObjective { Id = id },
                            TypeSurvey = new TypeSurveyGji { Id = typeSurveyId }
                        };

                        service.Save(newRec);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                Container.Release(service);
            }
        }

        public IDataResult AddInspFoundation(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>();
            try
            {
                var typeSurveyId = baseParams.Params.ContainsKey("typeSurveyId")
                                       ? baseParams.Params["typeSurveyId"].ToLong()
                                       : 0;

                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                    ? baseParams.Params["objectIds"].ToString()
                                    : string.Empty;

                if (typeSurveyId == 0 || string.IsNullOrEmpty(objectIds))
                {
                    return new BaseDataResult
                    {
                        Success = false,
                        Message = "Не удалось получить тип обследования и/или виды обследования"
                    };
                }

                var existRecords =
                    service.GetAll().Where(x => x.TypeSurvey.Id == typeSurveyId).Select(x => x.NormativeDoc.Id).ToList();

                foreach (var id in objectIds.ToLongArray())
                {
                    if (!existRecords.Contains(id))
                    {
                        var newRec = new TypeSurveyInspFoundationGji
                        {
                            NormativeDoc = new NormativeDoc { Id = id },
                            TypeSurvey = new TypeSurveyGji { Id = typeSurveyId }
                        };

                        service.Save(newRec);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                Container.Release(service);
            }
        }

		public IDataResult AddInspFoundationChecks(BaseParams baseParams)
		{
			var service = Container.Resolve<IDomainService<TypeSurveyInspFoundationCheckGji>>();
			try
			{
				var typeSurveyId = baseParams.Params.ContainsKey("typeSurveyId")
									   ? baseParams.Params["typeSurveyId"].ToLong()
									   : 0;

				var objectIds = baseParams.Params.ContainsKey("objectIds")
									? baseParams.Params["objectIds"].ToString()
									: string.Empty;

				if (typeSurveyId == 0 || string.IsNullOrEmpty(objectIds))
				{
					return new BaseDataResult
					{
						Success = false,
						Message = "Не удалось получить тип обследования и/или виды обследования"
					};
				}

				var existRecords =
					service.GetAll().Where(x => x.TypeSurvey.Id == typeSurveyId).Select(x => x.NormativeDoc.Id).ToList();

				foreach (var id in objectIds.ToLongArray())
				{
					if (!existRecords.Contains(id))
					{
						var newRec = new TypeSurveyInspFoundationCheckGji
						{
							NormativeDoc = new NormativeDoc { Id = id },
							TypeSurvey = new TypeSurveyGji { Id = typeSurveyId }
						};

						service.Save(newRec);
					}
				}

				return new BaseDataResult();
			}
			catch (ValidationException e)
			{
				return new BaseDataResult { Success = false, Message = e.Message };
			}
			finally
			{
				Container.Release(service);
			}
		}

		public IDataResult AddKindInsp(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<TypeSurveyKindInspGji>>();
            try
            {
                var typeSurveyId = baseParams.Params.ContainsKey("typeSurveyId")
                                       ? baseParams.Params["typeSurveyId"].ToLong()
                                       : 0;

                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                    ? baseParams.Params["objectIds"].ToString()
                                    : string.Empty;

                if (typeSurveyId == 0 || string.IsNullOrEmpty(objectIds))
                {
                    return new BaseDataResult
                               {
                                   Success = false,
                                   Message = "Не удалось получить тип обследования и/или виды обследования"
                               };
                }

                var existRecords =
                    service.GetAll().Where(x => x.TypeSurvey.Id == typeSurveyId).Select(x => x.KindCheck.Id).ToList();

                foreach (var id in objectIds.ToLongArray())
                {
                    if (!existRecords.Contains(id))
                    {
                        var newRec = new TypeSurveyKindInspGji
                                         {
                                             KindCheck = new KindCheckGji { Id = id },
                                             TypeSurvey = new TypeSurveyGji { Id = typeSurveyId }
                                         };

                        service.Save(newRec);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                Container.Release(service);
            }
        }

        public IDataResult AddProvidedDocuments(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<TypeSurveyProvidedDocumentGji>>();

            try
            {
                var typeSurveyId = baseParams.Params.ContainsKey("typeSurveyId")
                                       ? baseParams.Params["typeSurveyId"].ToLong()
                                       : 0;

                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                    ? baseParams.Params["objectIds"].ToString()
                                    : string.Empty;

                if (typeSurveyId == 0 || string.IsNullOrEmpty(objectIds))
                {
                    return new BaseDataResult
                               {
                                   Success = false,
                                   Message =
                                       "Не удалось получить тип обследования и/или предоставляемые документы"
                               };
                }

                var existRecords =
                    service.GetAll().Where(x => x.TypeSurvey.Id == typeSurveyId).Select(x => x.ProvidedDocGji.Id).ToList();

                foreach (var id in objectIds.ToLongArray())
                {
                    if (!existRecords.Contains(id))
                    {
                        var newRec = new TypeSurveyProvidedDocumentGji
                                         {
                                             ProvidedDocGji = new ProvidedDocGji { Id = id },
                                             TypeSurvey =
                                                 new TypeSurveyGji { Id = typeSurveyId }
                                         };

                        service.Save(newRec);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}