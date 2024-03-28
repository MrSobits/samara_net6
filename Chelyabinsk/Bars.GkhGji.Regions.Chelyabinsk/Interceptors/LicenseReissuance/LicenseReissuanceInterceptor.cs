namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Modules.States;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using System.Linq;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.smevHistoryServiceV2;

    class LicenseReissuanceInterceptor : EmptyDomainInterceptor<LicenseReissuance>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }
        public IDomainService<SmevHistory> SmevHistoryDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<LicenseReissuance> service, LicenseReissuance entity)
        {

            if (entity.Contragent != null && entity.Contragent.Id != 0)
            {
                if (entity.State == null)
                {
                    var stateProvider = Container.Resolve<IStateProvider>();
                    stateProvider.SetDefaultState(entity);
                }

                var manOrgLicense = ManOrgLicenseDomain.GetAll()
                    .Where(X => X.Contragent == entity.Contragent)
                    .Where(x => x.State.Name == "Действующая")
                    .FirstOrDefault();
                if (manOrgLicense != null)
                {
                    entity.ManOrgLicense = manOrgLicense;
                }
                return Success();
                //if (manOrgLicense != null)
                //{
                //    entity.ManOrgLicense = manOrgLicense;
                //    var stateProvider = Container.Resolve<IStateProvider>();
                //    try
                //    {
                //        stateProvider.SetDefaultState(entity);
                //        if (entity.RegisterNum.ToString() != entity.RegisterNumber)
                //        {
                //            entity.RegisterNumber = entity.RegisterNum.ToString();
                //        }
                //        return Success();
                //    }
                //    catch
                //    {
                //        return Failure("Для обращения не задан начальный статус");
                //    }
                //    finally
                //    {
                //        Container.Release(stateProvider);
                //    }
                //    // entity.State=;
                //    return Success();
            //}
            
            }
            else
            {
                return Failure("Не выбран лицензиат");
            }

        }
        public override IDataResult BeforeUpdateAction(IDomainService<LicenseReissuance> service, LicenseReissuance entity)
        {
            if (entity.RegisterNum.ToString() != entity.RegisterNumber)
            {
                entity.RegisterNumber = entity.RegisterNum.ToString();
            }
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
                    case "Лицензия выдана":
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
    }
}
