namespace Bars.Gkh.Regions.Tatarstan.Controller
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;
    using System.Collections.Generic;
    /// <summary>
    /// Контроллер для сущности Исполнительное производство
    /// </summary>
    public class ExecutoryProcessController : FileStorageDataController<ExecutoryProcess>
    {
        /// <summary>
        /// Проверить существование дочерних документов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult CheckChildDocs(BaseParams baseParams)
        {
            var seizureOfPropertyDocDomain = this.Container.ResolveDomain<SeizureOfProperty>();
            var departureRestrictionDomain = this.Container.ResolveDomain<DepartureRestriction>();
            var executoryProcessDomain = this.Container.ResolveDomain<ExecutoryProcess>();
            var executoryProcessDocumentDomain = this.Container.ResolveDomain<ExecutoryProcessDocument>();

            var execProcId = baseParams.Params.GetAsId();
            var executoryProcess = executoryProcessDomain.Get(execProcId);

            try
            {
                if (seizureOfPropertyDocDomain.GetAll().Any(x => x.ClaimWork.Id == executoryProcess.ClaimWork.Id)
                    || departureRestrictionDomain.GetAll().Any(x => x.ClaimWork.Id == executoryProcess.ClaimWork.Id)
                    || executoryProcessDocumentDomain.GetAll().Any(x => x.ExecutoryProcess.Id == execProcId))
                {
                    return this.JsSuccess("У документа существуют дочерние документы. Вы уверены, что хотите его удалить?");
                }

                return this.JsSuccess();
            }
            catch (Exception exception)
            {
                return this.JsFailure(exception.Message);
            }
            finally
            {
                this.Container.Release(seizureOfPropertyDocDomain);
                this.Container.Release(departureRestrictionDomain);
                this.Container.Release(executoryProcessDomain);
            }
        }

        public override ActionResult Delete(BaseParams baseParams)
        {
            var departureRestrictionDomain = this.Container.ResolveDomain<DepartureRestriction>();
            var seizureOfPropertyDomain = this.Container.ResolveDomain<SeizureOfProperty>();
            var executoryProcessDomain = this.Container.ResolveDomain<ExecutoryProcess>();

            var entityId = baseParams.Params.GetAs<List<long>>("records")[0];

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var claimWorkId = executoryProcessDomain.Get(entityId).ClaimWork.Id;

                    //удаляем все постановления
                    var departureRestriction = departureRestrictionDomain.GetAll()
                        .FirstOrDefault(x => x.ClaimWork.Id == claimWorkId);

                    if (departureRestriction != null)
                    {
                        departureRestrictionDomain.Delete(departureRestriction.Id);
                    }

                    var seizureOfProperty = seizureOfPropertyDomain.GetAll()
                       .FirstOrDefault(x => x.ClaimWork.Id == claimWorkId);

                    if (seizureOfProperty != null)
                    {
                        seizureOfPropertyDomain.Delete(seizureOfProperty.Id);
                    }

                    IDataResult dataResult = this.DomainService.Delete(baseParams);
                    var result = (ActionResult)new JsonNetResult((object)new
                    {
                        success = dataResult.Success,
                        message = dataResult.Message,
                        data = dataResult.Data
                    })
                    {
                        ContentType = "text/html; charset=utf-8"
                    };
                    
                    transaction.Commit();

                    return result;
                }
                catch (ValidationException ex)
                {
                    transaction.Rollback();
                    JsonNetResult jsonNetResult = JsonNetResult.Failure(ex.Message);
                    jsonNetResult.ContentType = "text/html; charset=utf-8";
                    return (ActionResult)jsonNetResult;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(departureRestrictionDomain);
                    this.Container.Release(seizureOfPropertyDomain);
                    this.Container.Release(executoryProcessDomain);
                }
            }
        }
    }
}