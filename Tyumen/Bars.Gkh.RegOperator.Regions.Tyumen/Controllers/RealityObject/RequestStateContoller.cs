using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.Helpers;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.RegOperator.Regions.Tyumen.Entities;
using Bars.GkhCr.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace Bars.Gkh.RegOperator.Regions.Tyumen.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер запросов доступа к редактированию дома
    /// </summary>
    public class RequestStateContoller : BaseController
    {
        public IUserIdentity UserIdentity { get; set; }
        public IEMailSender EMailSender { get; set; }
        public IFileManager FileManager { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<RequestState> RequestStateDomain { get; set; }
        public IDomainService<RequestStatePerson> RequestStatePersonDomain { get; set; }

        public ActionResult SendEmail(BaseParams baseParams)
        {
            try
            {
                var house = RealityObjectDomain.Get(baseParams.Params.GetAs<long>("realityObjectId"));
                var description = baseParams.Params.GetAs<string>("Description");
                var file = baseParams.Files.Values.FirstOrDefault();
                var userManager = this.Container.Resolve<IGkhUserManager>();
                var activeUser = userManager.GetActiveUser();

                RequestStateDomain.Save(new RequestState
                {
                    RealityObject = house,
                    UserId = UserIdentity.UserId,
                    UserName = UserIdentity.Name,
                    Description = baseParams.Params.GetAs<string>("Description"),
                    File = file == null ? null : FileManager.SaveFile(new MemoryStream(file.Data), file.FileName + '.' + file.Extention),
                });

                string typeHouse = "";
                switch (house.TypeHouse)
                {
                    case Gkh.Enums.TypeHouse.BlockedBuilding:
                        {
                            typeHouse = "Блокированой застройки";
                        }
                        break;

                    case Gkh.Enums.TypeHouse.Individual:
                        {
                            typeHouse = "Индивидуальный";
                        }
                        break;
                    case Gkh.Enums.TypeHouse.ManyApartments:
                        {
                            typeHouse = "Многоквартирный";
                        }
                        break;
                    case Gkh.Enums.TypeHouse.NotSet:
                        {
                            typeHouse = "Не задан";
                        }
                        break;
                    case Gkh.Enums.TypeHouse.SocialBehavior:
                        {
                            typeHouse = "Общежитие";
                        }
                        break;
                }

                string condition = "";
                switch (house.ConditionHouse)
                {
                    case Gkh.Enums.ConditionHouse.Dilapidated:
                        {
                            condition = "Ветхий";
                        }
                        break;

                    case Gkh.Enums.ConditionHouse.Emergency:
                        {
                            condition = "Аварийный";
                        }
                        break;
                    case Gkh.Enums.ConditionHouse.NotSelected:
                        {
                            condition = "Не задано";
                        }
                        break;

                    case Gkh.Enums.ConditionHouse.Razed:
                        {
                            condition = "Снесен";
                        }
                        break;
                    case Gkh.Enums.ConditionHouse.Serviceable:
                        {
                            condition = "Исправный";
                        }
                        break;
                }

                var crObjectDomain = this.Container.Resolve<IDomainService<ObjectCr>>();
                var programs = crObjectDomain.GetAll()
                    .Where(x => x.RealityObject.Id == house.Id)
                    .Select(x => x.ProgramCr.Name).Distinct().ToList();

                var prgVersionDomain = this.Container.Resolve<IDomainService<VersionRecord>>();

                var versionPr = prgVersionDomain.GetAll()
                    .Where(x => x.RealityObject.Id == house.Id && x.Show)
                    .Where(x=> x.ProgramVersion.IsMain)
                    .Select(x => new
                    {
                        x.ProgramVersion.Name
                    }).FirstOrDefault();

                var subpr = prgVersionDomain.GetAll()
                    .Where(x => x.RealityObject.Id == house.Id && x.Show && x.SubProgram)
                    .Where(x => x.ProgramVersion.IsMain)
                    .Select(x => x.ProgramVersion.Name).FirstOrDefault();

                var operatorDomain = this.Container.Resolve<IDomainService<Operator>>();
                var operatorMunicipalityDomain = this.Container.Resolve<IDomainService<OperatorMunicipality>>();
                var operatorContragentDomain = this.Container.Resolve<IDomainService<OperatorContragent>>();

                var currentUserOperator = operatorDomain.GetAll()
                    .Where(x => x.IsActive && x.User == activeUser).FirstOrDefault();

                var municipalities = operatorMunicipalityDomain.GetAll()
                    .Where(x => x.Operator != null && x.Operator.User == activeUser && x.Municipality != null)
                    .Select(x => x.Municipality.Name).Distinct().ToList();
                string municipalitiesNames = String.Join(", ", municipalities);

                var operatorContragent = operatorContragentDomain.GetAll()
                     .Where(x => x.Operator != null && x.Operator.User == activeUser && x.Contragent != null)
                     .Select(x => x.Contragent).FirstOrDefault();
                string crName = "Не указана";
                string crInn = string.Empty;
                string crOgrn = string.Empty;
                string crPhone = string.Empty;
                if (operatorContragent != null)
                {
                    crName = operatorContragent.Name;
                    crInn = operatorContragent.Inn;
                    crOgrn = operatorContragent.Ogrn;
                    crPhone = operatorContragent.Phone;
                }

                string insumdpkr = string.IsNullOrEmpty(subpr) ? "Нет" : "Да";
                string shortProgramsNames = String.Join(", ", programs);
                string message = $"Пользователь {activeUser.Name} запросил доступ на редактирование дома {house.Address} c Id {house.Id}\n" +
                    $"Статус дома: {house.State.Name} \n" +
                    $"Тип дома: {typeHouse} \n" +
                    $"Состояние дома: {condition} \n"+
                    $"Код дома: {house.Id} \n"+
                    $"Участвует в программах КПР: {shortProgramsNames} \n" +
                    $"Участвует в программе ДПР: {versionPr} \n" +
                    $"Подпрограмма: {insumdpkr} \n"+
                    $"\nСсылка на дом: http://monjf.admtyumen.ru/gkh/#realityobjectedit/{house.Id}/edit \n\n"+
                    $"Информация о пользователе \n"+
                    $"ФИО: {activeUser.Name} \n" +
                    $"Логин: {activeUser.Login} \n" +
                    $"Муниципальные образования: {municipalitiesNames} \n"+
                    $"Электронная почта: {currentUserOperator.User.Email} \n" +
                    $"Телефон: {currentUserOperator.Phone} \n"+                  
                    $"Организация: {crName}, ИНН {crInn}, ОГРН {crOgrn}\n"+
                    $"Телефон организации: {crPhone} \n";
                if (!String.IsNullOrEmpty(description))
                    message +=  "Примечание:\n" + description;

                var emailList = RequestStatePersonDomain.GetAll()
                    .Where(x=> x.Status == Enums.RequestStatePersonEnum.Edit).ToList();

                foreach (RequestStatePerson rsp in emailList)
                {
                    EMailSender.Send(rsp.Email,
                                     $"Уважаемый(ая) {rsp.Name}, поступил запрос доступа на редактирование от пользователя {activeUser.Name}",
                                     message,
                                     file == null ? null : new List<Attachment> { new Attachment(new MemoryStream(file.Data), file.FileName + '.' + file.Extention) });
                }
                return JsSuccess();
            }
            catch (Exception e)
            {
                return JsFailure(e.Message + ": " + e.InnerException?.Message);
            }
        }
    }
}
