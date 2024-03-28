﻿namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    using System;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Castle.Windsor;
    
    public class AppealSOPRStateRule : IRuleChangeStatus
    {
        public virtual IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "AppealSOPRStateRule"; }
        }

        public string Name { get { return "Перенаправление обращения исполнителю"; } }
        public string TypeId { get { return "gji_appeal_citizens"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет создано уведомления для УК";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var appealOrderDomain = Container.ResolveDomain<AppealOrder>();
            var appealCitsRealityObjectDomain = Container.ResolveDomain<AppealCitsRealityObject>();
            var manOrgContractRealityObjectDomain = Container.ResolveDomain<ManOrgContractRealityObject>();
            try
            {
              
                var appeal = statefulEntity as AppealCits;
                appeal.CaseDate = DateTime.Now;
                var existingOrder = appealOrderDomain.GetAll()
                    .FirstOrDefault(x => x.AppealCits == appeal);
                Contragent contragent = new Contragent();
                
                if (existingOrder != null)
                {
                    return ValidateResult.No("Уведомление уже создано. Повторное уведомление запрещено.");
                }
                if (appeal.OrderContragent == null)
                {
                    var realityObject = appealCitsRealityObjectDomain.FirstOrDefault(x => x.AppealCits == appeal);
                    if (realityObject == null)
                    {
                        return ValidateResult.No("Не указан контрагент");
                    }
                    var contract = manOrgContractRealityObjectDomain.GetAll()
                        .Where(x => x.RealityObject == realityObject.RealityObject)
                        .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate.Value <= DateTime.Now)
                        .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value > DateTime.Now).FirstOrDefault();
                    if (contract == null)
                    {
                        return ValidateResult.No("Не указан контрагент");
                    }
                    if (contract.ManOrgContract.ManagingOrganization != null)
                    {
                        contragent = contract.ManOrgContract.ManagingOrganization.Contragent;
                    }
                    else
                    {
                        return ValidateResult.No("Не указан контрагент");
                    }


                }
                else
                {
                    contragent = appeal.OrderContragent;
                }
                if(contragent == null)
                {
                    return ValidateResult.No("Не удалось определить контрагента");
                }
                AppealOrder newOrder = new AppealOrder
                {
                    AppealCits = new AppealCits { Id = appeal.Id},
                    AppealText = appeal.Description,
                    Executant = contragent,
                    OrderDate = DateTime.Now,
                    YesNoNotSet = Gkh.Enums.YesNoNotSet.No,
                    Confirmed = Gkh.Enums.YesNoNotSet.NotSet
                };
                appealOrderDomain.Save(newOrder);

                string email = contragent.Email;
                if (!string.IsNullOrEmpty(email))
                {
                    try
                    {
                        EmailSender emailSender = EmailSender.Instance;
                        emailSender.Send(email, "Уведомление о передаче на оперативное исполнение обращения", MakeMessageBody(appeal), null);
                    }
                    catch
                    { }
                }

            }
            catch(Exception e)
            {
                return ValidateResult.No("Не удалось создать уведомление для УК");
            }
            finally
            {
                Container.Release(appealOrderDomain);
                Container.Release(appealCitsRealityObjectDomain);
                Container.Release(manOrgContractRealityObjectDomain);
            }
                                      
            return ValidateResult.Yes();
        }

        string MakeMessageBody(AppealCits appeal)
        {          

            string body = $"Уважаемый(ая) пользователь!\r\n";
            body += $"Главное управление Государственная жилищная инспекция  Челябинской области сообщает Вам, что обращение {appeal.NumberGji} от {appeal.DateFrom.Value.ToShortDateString()} передано Вам для оперативного исполнения и размещено в реестре СОПР АИС ГЖИ Челябинской области.\r\n";
            return body;
        }


    }
}
