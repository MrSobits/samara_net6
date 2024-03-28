using Bars.B4.Modules.States;
using Bars.Gkh.Domain.CollectionExtensions;

namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using System;
    using System.Linq;

    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.smevHistoryServiceV2;

    public class ManOrgLicenseRequestInterceptor : EmptyDomainInterceptor<ManOrgLicenseRequest>
    {
        public IDomainService<SmevHistory> SmevHistoryDomain { get; set; }
        //public override IDataResult BeforeCreateAction(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        //{
        //    var stateProvider = Container.Resolve<IStateProvider>();
        //    stateProvider.SetDefaultState(entity);

        //    CreateNumber(service, entity);

        //    if (entity.RegisterNum.ToString() != entity.RegisterNumber)
        //    {
        //        entity.RegisterNumber = entity.RegisterNum.ToString();    
        //    }

        //    return ValidationNumber(service, entity) ? Failure(string.Format("Обращение с номером {0} уже существует", entity.RegisterNum)) : Success();
        //}

        public override IDataResult BeforeUpdateAction(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        {
            var smevHistory = SmevHistoryDomain.GetAll()
                .Where(x => x.RequestId == entity.Id && (x.LicenseRequestType == LicenseRequestType.IssuingDuplicateLicense || x.LicenseRequestType == LicenseRequestType.RenewalLicense))
                .FirstOrDefault();
            if (smevHistory != null)
            {
                var smevHistoryService = new smevHistoryServiceV2();
                switch (entity.State.Code)
                {
                    case "Черновик":
                    case "Получено с портала госуслуг":
                        if (smevHistory.Status != "4")
                        {
                            updateStatusRequest historyReq = new updateStatusRequest
                            {
                                uniqId = smevHistory.UniqId,
                                status = "4"
                            };
                            try
                            {
                                updateStatusResponse historyResp = smevHistoryService.updateStatus(historyReq);
                                smevHistory.Status = historyResp.statusId;
                                SmevHistoryDomain.Update(smevHistory);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        break;
                    case "Выдана лицензия":
                        if (smevHistory.Status != "7")
                        {
                            updateStatusRequest historyReq = new updateStatusRequest
                            {
                                uniqId = smevHistory.UniqId,
                                status = "7"
                            };
                            try
                            {
                                updateStatusResponse historyResp = smevHistoryService.updateStatus(historyReq);
                                smevHistory.Status = historyResp.statusId;
                                SmevHistoryDomain.Update(smevHistory);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        break;
                    case "Назначен ответственный":
                        if (smevHistory.Status != "1")
                        {
                            updateStatusRequest historyReq = new updateStatusRequest
                            {
                                uniqId = smevHistory.UniqId,
                                status = "1"
                            };
                            try
                            {
                                updateStatusResponse historyResp = smevHistoryService.updateStatus(historyReq);
                                smevHistory.Status = historyResp.statusId;
                                SmevHistoryDomain.Update(smevHistory);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        break;
                    case "Принято в работу ГЖИ":
                        if (smevHistory.Status != "2")
                        {
                            updateStatusRequest historyReq = new updateStatusRequest
                            {
                                uniqId = smevHistory.UniqId,
                                status = "2"
                            };
                            try
                            {
                                updateStatusResponse historyResp = smevHistoryService.updateStatus(historyReq);
                                smevHistory.Status = historyResp.statusId;
                                SmevHistoryDomain.Update(smevHistory);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        break;
                    case "Отказ выдачи лицензии":
                        if (smevHistory.Status != "6")
                        {
                            updateStatusRequest historyReq = new updateStatusRequest
                            {
                                uniqId = smevHistory.UniqId,
                                status = "6"
                            };
                            try
                            {
                                updateStatusResponse historyResp = smevHistoryService.updateStatus(historyReq);
                                smevHistory.Status = historyResp.statusId;
                                SmevHistoryDomain.Update(smevHistory);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        break;
                    case "Результат услуги готов к выдаче":
                        if (smevHistory.Status != "13")
                        {
                            updateStatusRequest historyReq = new updateStatusRequest
                            {
                                uniqId = smevHistory.UniqId,
                                status = "13"
                            };
                            try
                            {
                                updateStatusResponse historyResp = smevHistoryService.updateStatus(historyReq);
                                smevHistory.Status = historyResp.statusId;
                                SmevHistoryDomain.Update(smevHistory);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        break;
                }
            }
            return Success();
        }

        //private void CreateNumber(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        //{
        //    if (entity.RegisterNum.HasValue && entity.RegisterNum.Value > 0)
        //    {
        //        return;
        //    }

        //    var num = service.GetAll()
        //        .Where(x => x.Id != entity.Id)
        //        .Select(x => x.RegisterNum.HasValue ? x.RegisterNum.Value : 0)
        //        .SafeMax(x => x);

        //    entity.RegisterNum = num + 1;
        //}

        //private bool ValidationNumber(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        //{
        //    return service.GetAll().Where(x => x.Id != entity.Id && entity.RegisterNum == x.RegisterNum).Any();
        //}

        //public override IDataResult BeforeDeleteAction(IDomainService<ManOrgLicenseRequest> service, ManOrgLicenseRequest entity)
        //{
        //    var requestPersonDomain = Container.Resolve<IDomainService<ManOrgRequestPerson>>();
        //    var requestProvDocDomain = Container.Resolve<IDomainService<ManOrgRequestProvDoc>>();
        //    var licenseDomain = Container.Resolve<IDomainService<ManOrgLicense>>();

        //    try
        //    {
        //        if (licenseDomain.GetAll().Any(x => x.Request.Id == entity.Id))
        //        {
        //            return Failure("По данному запросу имеется лицензия");
        //        }

        //        requestPersonDomain.GetAll().Where(x => x.LicRequest.Id == entity.Id)
        //            .Select(x => x.Id).ForEach(x => requestPersonDomain.Delete(x));

        //        requestProvDocDomain.GetAll().Where(x => x.LicRequest.Id == entity.Id)
        //            .Select(x => x.Id).ForEach(x => requestProvDocDomain.Delete(x));

        //        return Success();
        //    }
        //    catch (Exception exc)
        //    {
        //        return Failure("Не удалось удалить связанные записи "+ exc.Message);
        //    }
        //    finally
        //    {
        //        Container.Release(requestPersonDomain);
        //        Container.Release(requestProvDocDomain);
        //        Container.Release(requestProvDocDomain);
        //    }
        //}
    }
}