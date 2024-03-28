namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Entities;
    using Enums;

    using Castle.Windsor;

    public class PropertyOwnerProtocolInspectorService : IPropertyOwnerProtocolInspectorService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddInspectors(BaseParams baseParams)
        {

            var service = Container.Resolve<IDomainService<PropertyOwnerProtocolInspector>>();
            var serviceDocument = Container.Resolve<IDomainService<PropertyOwnerProtocols>>();
            List<PropertyOwnerProtocolInspector> listInspectorsToSave = new List<PropertyOwnerProtocolInspector>();

            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var inspectorIds = baseParams.Params.GetAs<string>("inspectorIds");

                // В этом словаре будут существующие инспектора
                // key - идентификатор Инспектора
                // value - объект Инспектора в акте проверки
                var dictInspectors = service.GetAll()
                    .Where(x => x.PropertyOwnerProtocols.Id == documentId)
                    .Select(x => x.Inspector.Id).Distinct().ToList();

                if (!string.IsNullOrEmpty(inspectorIds) && documentId > 0)
                {
                    var document = serviceDocument.GetAll().FirstOrDefault(x => x.Id == documentId);
                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in inspectorIds.Split(','))
                    {
                        var newId = id.ToLong();

                        // Если с таким id и order уже есть в списке то удалем его из списка и просто пролетаем дальше
                        // без добавления в БД
                        if (dictInspectors.Contains(newId))
                        {
                            dictInspectors.Remove(newId);
                            continue;
                        }
                       else if (newId > 0)
                        {
                            var newObj = new PropertyOwnerProtocolInspector
                            {
                                PropertyOwnerProtocols = document,
                                Inspector = new Inspector { Id = newId }
                            };

                            listInspectorsToSave.Add(newObj);
                            dictInspectors.Remove(newId);
                        }

                    }

                    using (var transaction = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            foreach (var item in listInspectorsToSave)
                            {
                                service.Save(item);
                            }

                            // Если какието инспектора остались в dictInspectors то их удаляем
                            // поскольку среди переданных inspectorIds их небыло, но в БД они остались
                            foreach (var keyValue in dictInspectors)
                            {
                                var existsToRemove = service.GetAll()
                                      .Where(x => x.PropertyOwnerProtocols.Id == documentId)
                                      .Where(x => x.Inspector.Id == keyValue).FirstOrDefault().Id;
                                service.Delete(existsToRemove);
                            }



                            transaction.Commit();
                            return new BaseDataResult();
                        }
                        catch (ValidationException e)
                        {
                            transaction.Rollback();
                            return new BaseDataResult { Success = false, Message = e.Message };
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(service);
                Container.Release(serviceDocument);
            }
        }

        public IDataResult AddDicisions(BaseParams baseParams)
        {

            var service = Container.Resolve<IDomainService<PropertyOwnerProtocolsDecision>>();
            var serviceDocument = Container.Resolve<IDomainService<PropertyOwnerProtocols>>();
            List<PropertyOwnerProtocolsDecision> listDecisionsToSave = new List<PropertyOwnerProtocolsDecision>();

            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var decisionIds = baseParams.Params.GetAs<string>("inspectorIds");

                // В этом словаре будут существующие инспектора
                // key - идентификатор Инспектора
                // value - объект Инспектора в акте проверки
                var dictDecisions = service.GetAll()
                    .Where(x => x.Protocol.Id == documentId)
                    .Select(x => x.Decision.Id).Distinct().ToList();

                if (!string.IsNullOrEmpty(decisionIds) && documentId > 0)
                {
                    var document = serviceDocument.GetAll().FirstOrDefault(x => x.Id == documentId);
                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in decisionIds.Split(','))
                    {
                        var newId = id.ToLong();

                        // Если с таким id и order уже есть в списке то удалем его из списка и просто пролетаем дальше
                        // без добавления в БД
                        if (dictDecisions.Contains(newId))
                        {
                            dictDecisions.Remove(newId);
                            continue;
                        }
                        else if (newId > 0)
                        {
                            var newObj = new PropertyOwnerProtocolsDecision
                            {
                                Protocol = document,
                                Decision = new OwnerProtocolTypeDecision { Id = newId },
                            };

                            listDecisionsToSave.Add(newObj);
                            dictDecisions.Remove(newId);
                        }

                    }

                    using (var transaction = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            foreach (var item in listDecisionsToSave)
                            {
                                service.Save(item);
                            }

                            // Если какието инспектора остались в dictInspectors то их удаляем
                            // поскольку среди переданных inspectorIds их небыло, но в БД они остались
                            foreach (var keyValue in dictDecisions)
                            {
                                var existsToRemove = service.GetAll()
                                      .Where(x => x.Protocol.Id == documentId)
                                      .Where(x => x.Decision.Id == keyValue).FirstOrDefault().Id;
                                service.Delete(existsToRemove);
                            }



                            transaction.Commit();
                            return new BaseDataResult();
                        }
                        catch (ValidationException e)
                        {
                            transaction.Rollback();
                            return new BaseDataResult { Success = false, Message = e.Message };
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(service);
                Container.Release(serviceDocument);
            }
        }

        /// <summary>
		/// Получить информацию
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<PropertyOwnerProtocolInspector>>();
            var serviceDocument = Container.Resolve<IDomainService<PropertyOwnerProtocols>>();
            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var inspList = service.GetAll()
                    .Where(x => x.PropertyOwnerProtocols.Id == documentId)
                   .Select(x => new { InspectorId = x.Inspector.Id, x.Inspector.Fio })
                        .ToList();
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                foreach (var item in inspList)
                {
                    if (!string.IsNullOrEmpty(inspectorNames))
                    {
                        inspectorNames += ", ";
                    }

                    inspectorNames += item.Fio;

                    if (!string.IsNullOrEmpty(inspectorIds))
                    {
                        inspectorIds += ", ";
                    }

                    inspectorIds += item.InspectorId.ToString();
                }

                return new BaseDataResult(new { inspectorNames, inspectorIds});
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(service);
                Container.Release(serviceDocument);
            }
        }

    }

      
}