namespace Bars.GkhGji.Regions.Voronezh.StateChanges
{
    using System;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Castle.Windsor;
    using Entities;

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
            var appealCitsRepo = Container.Resolve<IRepository<AppealCits>>();
            var appealCitsRealityObjectDomain = Container.ResolveDomain<AppealCitsRealityObject>();
            var manOrgContractRealityObjectDomain = Container.ResolveDomain<ManOrgContractRealityObject>();
            try
            {
              
                var appeal = statefulEntity as AppealCits;
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
                var appealCits = appealCitsRepo.Get(appeal.Id);
                appealCits.CaseDate = DateTime.Now;
                //запиливаем новый контрольный срок
                DateTime newControlDate = DateTime.Now;

                newControlDate = appeal.DateFrom.Value.AddDays(9);


                var prodCalendarContainer = this.Container.ResolveDomain< ProdCalendar>().GetAll()
    .Where(x => x.ProdDate >= appeal.DateFrom.Value && x.ProdDate <= appeal.DateFrom.Value.AddDays(9)).Select(x => x.ProdDate).ToList();

                if (prodCalendarContainer.Contains(newControlDate))
                {
                    for (int i = 0; i <= prodCalendarContainer.Count; i++)
                    {
                        if (prodCalendarContainer.Contains(newControlDate))
                        {
                            newControlDate = newControlDate.AddDays(-1);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (newControlDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    newControlDate = newControlDate.AddDays(-1);
                }
                else if (newControlDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    newControlDate = newControlDate.AddDays(-2);
                }
                //если стоит контрольный срок, расчетный не ставится

                appeal.CheckTime = newControlDate;
                appealCitsRepo.Update(appealCits);

                try
                {
                    string email = contragent.Email;
                    if (!string.IsNullOrEmpty(email))
                    {
                        try
                        {
                            EmailSender emailSender = EmailSender.Instance;
                            //Хабаровск
                           // emailSender.SendFKR(email, "Уведомление о размещении документа ГЖИ", MakeMessageBodyKhv(), null);
                            //Воронеж
                            emailSender.Send(email, "Уведомление о размещении документа ГЖИ", MakeMessageBodyVrn(), null);
                        }
                        catch(Exception e)
                        {
                            return ValidateResult.No("Не удалось отправить email. НЕобработанное исключение: " + e.Message + " " + e.InnerException);
 
                        }
                    }

                }
                catch
                {
                    return ValidateResult.No("Не удалось создать уведомление для УК");
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
                Container.Release(appealCitsRepo);
            }
                                      
            return ValidateResult.Yes();
        }

        string MakeMessageBodyVrn()
        {

            string body = $"Уважаемый(ая) пользователь!\r\n";
            body += $"Государственная жилищная инспекция Воронежской области уведомляет Вас о том, что вам поступило новое обращение в реестре СОПР.\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }
        string MakeMessageBodyKhv()
        {

            string body = $"Уважаемый(ая) пользователь!\r\n";
            body += $"Главное управление регионального государственного контроля и лицензирования Правительства Хабаровского края уведомляет Вас о том, что вам поступило новое обращение в реестре СОПР.\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }


    }
}
