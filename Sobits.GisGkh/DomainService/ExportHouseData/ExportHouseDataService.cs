using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.States;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.FileManager;
using Castle.Windsor;
using GisGkhLibrary.HouseManagementAsync;
using GisGkhLibrary.Services;
using GisGkhLibrary.Utils;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    public class ExportHouseDataService : IExportHouseDataService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public IRepository<RealityObject> RealityObjectRepo { get; set; }

        public IRepository<Room> RoomRepo { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Constructors

        public ExportHouseDataService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, string roId)
        {
            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                var house = RealityObjectDomain.Get(long.Parse(roId));
                string GUID = "";
                if (house.HouseGuid != null && house.HouseGuid != "")
                {
                    GUID = house.HouseGuid;
                }
                else if (house.FiasAddress.HouseGuid != null && house.FiasAddress.HouseGuid.HasValue)
                {
                    GUID = house.FiasAddress.HouseGuid.Value.ToString();
                }
                if (GUID != "")
                {
                    log += $"{DateTime.Now} Формирование запроса на получение ЛС из ГИС ЖКХ по дому {house.Address} (ФИАС GUID: {GUID})\r\n";
                    //Guid guid = new Guid("4b631569-9bd3-4f9a-b109-24d2c5ca4248");
                    Guid guid = new Guid(GUID);
                    var request = HouseManagementAsyncService.exportHouseDataReq(guid);
                    var prefixer = new XmlNsPrefixer();
                    XmlDocument document = SerializeRequest(request);
                    prefixer.Process(document);
                    SaveFile(req, GisGkhFileType.request, document, "request.xml");
                    req.RequestState = GisGkhRequestState.Formed;
                    log += $"{DateTime.Now} Запрос сформирован\r\n";
                    req.Answer = $"*{house.Id}* Сформирован запрос по дому {house.FiasAddress.AddressName}";
                }
                else
                {
                    req.RequestState = GisGkhRequestState.Error;
                    req.Answer = $"*{house.Id}* У дома {house.FiasAddress.AddressName} отсутствует идентификатор ФИАС";
                    log += $"{DateTime.Now} У дома {house.FiasAddress.AddressName} отсутствует идентификатор ФИАС. Запрос не сформирован\r\n";
                }
                SaveLog(ref req, ref log);
                GisGkhRequestsDomain.Update(req);
                //return true;
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        public void CheckAnswer(GisGkhRequests req, string orgPPAGUID)
        {
            int first = req.Answer.IndexOf('*');
            int len = req.Answer.IndexOf('*', first + 1) - first;
            string houseId = req.Answer.Substring(first, len + 1);

            try
            {
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                log += $"{DateTime.Now} Запрос ответа из ГИС ЖКХ\r\n";
                var response = HouseManagementAsyncService.GetState(req.MessageGUID, orgPPAGUID);

                if (response.RequestState == 3)
                {
                    // Удаляем старые файлы ответов, если они, вдруг, имеются
                    GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .ToList().ForEach(x => GisGkhRequestsFileDomain.Delete(x.Id));
                    SaveFile(req, GisGkhFileType.answer, SerializeRequest(response), "response.xml");
                    req.Answer = houseId + " Ответ получен";
                    req.RequestState = GisGkhRequestState.ResponseReceived;
                    log += $"{DateTime.Now} Ответ из ГИС ЖКХ получен. Ставим задачу на обработку ответа\r\n";
                    GisGkhRequestsDomain.Update(req);
                    BaseParams baseParams = new BaseParams();
                    baseParams.Params.Add("reqId", req.Id.ToString());
                    try
                    {
                        var taskInfo = _taskManager.CreateTasks(new ProcessGisGkhAnswersTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                        if (taskInfo == null)
                        {
                            req.Answer = houseId + " Сбой создания задачи обработки ответа";
                            log += $"{DateTime.Now} Сбой создания задачи обработки ответа\r\n";
                            GisGkhRequestsDomain.Update(req);
                        }
                        else
                        {
                            req.Answer = houseId + $" Задача на обработку ответа exportHouseData поставлена в очередь с id {taskInfo.TaskId}";
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Ошибка: " + e.Message);
                    }
                }
                else if ((response.RequestState == 1) || (response.RequestState == 2))
                {
                    req.Answer = houseId + " Запрос ещё в очереди";
                    log += $"{DateTime.Now} Запрос ещё в очереди\r\n";
                    SaveLog(ref req, ref log);
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                req.RequestState = GisGkhRequestState.Error;
                req.Answer = houseId + $" {e.Message}";
                GisGkhRequestsDomain.Update(req);
            }
        }

        public void ProcessAnswer (GisGkhRequests req)
        {
            string log = string.Empty;
            if (req.LogFile != null)
            {
                StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                log = reader.ReadToEnd();
                log += "\r\n";
            }
            log += $"{DateTime.Now} Обработка ответа\r\n";
            int first = req.Answer.IndexOf('*');
            int len = req.Answer.IndexOf('*', first + 1) - first;
            string houseId = req.Answer.Substring(first, len + 1);
            // берём дом по ИД в поле Answer
            var House = RealityObjectRepo.Load(long.Parse(houseId.Trim('*')));

            if (req.RequestState == GisGkhRequestState.ResponseReceived)
            {
                try
                {
                    var fileInfo = GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .FirstOrDefault();
                    if (fileInfo != null)
                    {
                        var fileStream = _fileManager.GetFile(fileInfo.FileInfo);
                        var data = fileStream.ReadAllBytes();
                        //return Encoding.UTF8.GetString(data);
                        var response = DeserializeData<getStateResult>(Encoding.UTF8.GetString(data));
                        foreach (var responseItem in response.Items)
                        {
                            if (responseItem is ErrorMessageType)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                var error = (ErrorMessageType)responseItem;
                                req.Answer = houseId + $" {error.ErrorCode}:{error.Description} {House.FiasAddress.AddressName}";
                                log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {error.ErrorCode}:{error.Description}\r\n";
                                House.GisGkhMatchDate = DateTime.Now;
                                RealityObjectRepo.Update(House);
                            }
                            else if (responseItem is exportHouseResultType)
                            {
                                // если пришёл многоквартирный дом
                                if (((exportHouseResultType)responseItem).Item is exportHouseResultTypeApartmentHouse)
                                {
                                    log += $"{DateTime.Now} Многоквартирный дом из ГИС ЖКХ, GUID {((exportHouseResultTypeApartmentHouse)((exportHouseResultType)responseItem).Item).BasicCharacteristicts.FIASHouseGuid}\r\n";
                                    //// ищем дом по ГУИДу
                                    //var House = RealityObjectRepo.GetAll()
                                    //    .Where(x => x.FiasAddress.HouseGuid.ToString() ==
                                    //    ((exportHouseResultTypeApartmentHouse)((exportHouseResultType)responseItem).Item).BasicCharacteristicts.FIASHouseGuid)
                                    //    .FirstOrDefault();
                                    if (House != null)
                                    {
                                        log += $"{DateTime.Now} Дом в системе: {House.Address}\r\n";
                                        var houseDataItem = ((exportHouseResultType)responseItem).Item as exportHouseResultTypeApartmentHouse;
                                        //проставляем характеристики дома из ГИСа
                                        if (houseDataItem.BasicCharacteristicts != null)
                                        {
                                            if (houseDataItem.BasicCharacteristicts.ItemElementName == ItemChoiceType3.CadastralNumber)
                                            {
                                                House.CadastralHouseNumber = houseDataItem.BasicCharacteristicts.Item.ToString();
                                            }
                                            if (houseDataItem.BasicCharacteristicts.TotalSquareSpecified)
                                            {
                                                House.AreaMkd = houseDataItem.BasicCharacteristicts.TotalSquare;
                                            }
                                            if (!string.IsNullOrEmpty(houseDataItem.BasicCharacteristicts.FloorCount))
                                            {
                                                if (houseDataItem.BasicCharacteristicts.FloorCount.Contains("-"))
                                                {
                                                    var floors = new List<int>();
                                                    foreach (string floorStr in houseDataItem.BasicCharacteristicts.FloorCount.Split("-"))
                                                    {
                                                        floors.Add(Convert.ToInt16(floorStr));
                                                    }
                                                    
                                                    if (floors.Count > 0)
                                                    {
                                                        House.Floors = floors.Min();
                                                        House.MaximumFloors = floors.Max();
                                                    }
                                                    else
                                                    {
                                                        House.Floors = 1;
                                                        House.MaximumFloors = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    House.Floors = Convert.ToInt16(houseDataItem.BasicCharacteristicts.FloorCount);
                                                }
                                            }
                                            if (houseDataItem.BasicCharacteristicts.UsedYearSpecified)
                                            {
                                                House.BuildYear = Convert.ToInt16(houseDataItem.BasicCharacteristicts.UsedYear);
                                            }
                                        }
                                        if (houseDataItem.Entrance != null)
                                        {
                                            int entrCount = 0;
                                            foreach (var ent in houseDataItem.Entrance.ToList())
                                            {
                                                entrCount++;
                                            }
                                            House.NumberEntrances = entrCount;
                                        }                                        

                                        decimal lvnotlvarea = 0;
                                        decimal lvarea = 0;
                                        if (houseDataItem.NonResidentialPremises != null)
                                        foreach (var notLivPremice in houseDataItem.NonResidentialPremises.ToList())
                                        {
                                            lvnotlvarea += notLivPremice.TotalAreaSpecified ? notLivPremice.TotalArea : 0;
                                            var existRoom = RoomRepo.GetAll()
                                                 .Where(x => x.RealityObject == House && x.RoomNum == notLivPremice.PremisesNum).FirstOrDefault();
                                            if (existRoom != null)
                                            {
                                                if (!string.IsNullOrEmpty(existRoom.GisGkhPremisesGUID))
                                                {
                                                    existRoom.GisGkhPremisesGUID = notLivPremice.PremisesGUID;
                                                    if (notLivPremice.ItemElementName == ItemChoiceType3.CadastralNumber)
                                                    {
                                                        existRoom.CadastralNumber = notLivPremice.Item.ToString();
                                                    }
                                                    RoomRepo.Update(existRoom);
                                                }
                                            }
                                            else
                                            {
                                                Room newRoom = new Room
                                                {
                                                    Area = notLivPremice.TotalAreaSpecified ? notLivPremice.TotalArea : 0,
                                                    Type = Bars.Gkh.Enums.RoomType.NonLiving,
                                                    OwnershipType = Bars.Gkh.Enums.RoomOwnershipType.NotSet,
                                                    GisGkhPremisesGUID = notLivPremice.PremisesGUID,
                                                    RealityObject = House,
                                                    RoomNum = NormalizeString(notLivPremice.PremisesNum,100),
                                                    CadastralNumber = notLivPremice.ItemElementName == ItemChoiceType3.CadastralNumber ? notLivPremice.Item.ToString() : ""
                                                };
                                                RoomRepo.Save(newRoom);
                                            }
                                        }
                                        if(houseDataItem.ResidentialPremises != null)
                                        foreach (var livPremice in houseDataItem.ResidentialPremises.ToList())
                                        {
                                            lvnotlvarea += livPremice.TotalAreaSpecified ? livPremice.TotalArea : 0;
                                            lvarea += livPremice.TotalAreaSpecified ? livPremice.TotalArea : 0;
                                            var existRoom = RoomRepo.GetAll()
                                                 .Where(x => x.RealityObject == House && x.RoomNum == livPremice.PremisesNum).FirstOrDefault();
                                            if (existRoom != null)
                                            {
                                                if (!string.IsNullOrEmpty(existRoom.GisGkhPremisesGUID))
                                                {
                                                    existRoom.GisGkhPremisesGUID = livPremice.PremisesGUID;
                                                    if (livPremice.ItemElementName == ItemChoiceType3.CadastralNumber)
                                                    {
                                                       existRoom.CadastralNumber = livPremice.Item.ToString();
                                                    }
                                                    RoomRepo.Update(existRoom);
                                                }
                                            }
                                            else
                                            {
                                                Room newRoom = new Room
                                                {
                                                    Area = livPremice.TotalAreaSpecified ? livPremice.TotalArea : 0,
                                                    Type = Bars.Gkh.Enums.RoomType.Living,
                                                    OwnershipType = Bars.Gkh.Enums.RoomOwnershipType.NotSet,
                                                    GisGkhPremisesGUID = livPremice.PremisesGUID,
                                                    RealityObject = House,
                                                    RoomNum = NormalizeString(livPremice.PremisesNum,100),
                                                    CadastralNumber = livPremice.ItemElementName == ItemChoiceType3.CadastralNumber? livPremice.Item.ToString():""
                                                };
                                                RoomRepo.Save(newRoom);
                                            }
                                        }
                                        House.AreaLivingNotLivingMkd = lvnotlvarea;
                                        House.AreaLiving = lvarea;

                                        // Прописываем дому ГИС ЖКХ GUID. Да, он приходит под тегом FIASHouseGuid, но может отличаться от ФИАС
                                        House.GisGkhGUID = ((exportHouseResultTypeApartmentHouse)((exportHouseResultType)responseItem).Item).BasicCharacteristicts.FIASHouseGuid;
                                        if (!House.NumberGisGkhMatchedApartments.HasValue)
                                        {
                                            House.NumberGisGkhMatchedApartments = 0;
                                        }
                                        //var NumOfMatchedRes = House.NumberGisGkhMatchedApartments;
                                        //var NumOfMatchedNonRes = 0;
                                        // ищем помещения по дому
                                        var roomRepo = this.Container.ResolveRepository<Room>();
                                        var Rooms = roomRepo.GetAll()
                                            .Where(x => x.RealityObject == House);
                                        // собираем справочник жилых помещений из ГИС ЖКХ
                                        var ResPrems = ((exportHouseResultTypeApartmentHouse)((exportHouseResultType)responseItem).Item).ResidentialPremises;
                                        if (ResPrems != null)
                                        {
                                            var ResPremDict = new Dictionary<string, string>();
                                            // ищем номера без 0 спереди
                                            foreach (var resPrem in ResPrems)
                                            {
                                                AddPremises(House, resPrem);
                                                if (resPrem.PremisesNum.First() != '0')
                                                {
                                                    if (!ResPremDict.ContainsKey(resPrem.PremisesNum))
                                                    {
                                                        ResPremDict.Add(resPrem.PremisesNum, resPrem.PremisesGUID);
                                                    }
                                                    else
                                                    {
                                                        // тут можно проанализировать старое и новое помещение с одинаковым номером
                                                        // и выбрать, какое оставить в словаре
                                                    }
                                                }
                                            }
                                            // ещё раз пробегаем помещения и смотрим с нулём перед номером
                                            foreach (var resPrem in ResPrems)
                                            {
                                                if (resPrem.PremisesNum.First() == '0')
                                                {
                                                    // если нету такого же номера без учёта 0 - добавляем
                                                    if (!ResPremDict.ContainsKey(resPrem.PremisesNum.TrimStart('0')))
                                                    {
                                                        ResPremDict.Add(resPrem.PremisesNum.TrimStart('0'), resPrem.PremisesGUID);
                                                    }
                                                    else
                                                    {
                                                        // если уже есть - не добавляем
                                                        // либо как-то анализируем и выбираем
                                                    }
                                                }
                                            }
                                            // жилые помещения
                                            var ResRooms = Rooms.Where(x => x.Type == Bars.Gkh.Enums.RoomType.Living).ToList();
                                            // для каждого жилого помещения ищем соответствующее по номеру в ответе из ГИС ЖКХ
                                            foreach (var Room in ResRooms)
                                            {
                                                //// номер жилого помещения из системы - берём только первый блок цифр
                                                //var RoomNum = Regex.Match(Room.RoomNum, @"[0-9]+").Value;
                                                if (ResPremDict.ContainsKey(Room.RoomNum))
                                                {
                                                    if (string.IsNullOrEmpty(Room.GisGkhPremisesGUID))
                                                    {
                                                        House.NumberGisGkhMatchedApartments++;
                                                    }
                                                    Room.GisGkhPremisesGUID = ResPremDict[Room.RoomNum];
                                                    roomRepo.Update(Room);
                                                    //NumOfMatchedRes++;
                                                    ResPremDict.Remove(Room.RoomNum);
                                                    log += $"{DateTime.Now} Жилое помещение {Room.RoomNum} сопоставлено с ГИС ЖКХ\r\n";
                                                }
                                            }
                                            // тут в ResPremDict остаются жилые помещения из ГИС ЖКХ, которых нет в системе
                                        }

                                        // собираем нежилые помещения из ГИС ЖКХ
                                        var NonResPrems = ((exportHouseResultTypeApartmentHouse)((exportHouseResultType)responseItem).Item).NonResidentialPremises;
                                        if (NonResPrems != null)
                                        {
                                            foreach (var nonResPrem in NonResPrems)
                                            {
                                                AddPremises(House, nonResPrem);
                                            }
                                            // и из системы - пока не берём
                                            // var NonResRooms = Rooms.Where(x => x.Type == Bars.Gkh.Enums.RoomType.NonLiving).ToList();
                                            // todo как-то сопоставляем это дерьмо
                                            // NumOfMatchedNonRes++;
                                        }

                                        //House.NumberGisGkhMatchedApartments = NumOfMatchedRes;
                                        //House.NumberGisGkhMatchedNonResidental = NumOfMatchedNonRes;
                                        var state = StateDomain.GetAll().FirstOrDefault(x => x.Name == "Скорректирован" && x.TypeId == "gkh_real_obj");
                                        if (state != null)
                                        {
                                            House.State = state;
                                        }
                                        House.GisGkhMatchDate = DateTime.Now;
                                        RealityObjectRepo.Update(House);
                                        req.Answer = houseId + $" Данные из ГИС ЖКХ по дому {House.FiasAddress.AddressName} обработаны";
                                        req.RequestState = GisGkhRequestState.ResponseProcessed;
                                    }
                                }
                                // если пришёл индивидуальный дом
                                else if (((exportHouseResultType)responseItem).Item is exportHouseResultTypeLivingHouse)
                                {
                                    log += $"{DateTime.Now} Индивидуальный дом из ГИС ЖКХ, GUID {((exportHouseResultTypeLivingHouse)((exportHouseResultType)responseItem).Item).BasicCharacteristicts.FIASHouseGuid}\r\n";
                                    //// ищем дом по ГУИДу
                                    //var House = RealityObjectRepo.GetAll()
                                    //    .Where(x => x.FiasAddress.HouseGuid.ToString() ==
                                    //    ((exportHouseResultTypeApartmentHouse)((exportHouseResultType)responseItem).Item).BasicCharacteristicts.FIASHouseGuid)
                                    //    .FirstOrDefault();
                                    if (House != null)
                                    {
                                        log += $"{DateTime.Now} Дом в системе: {House.Address}\r\n";
                                        var houseDataItem = ((exportHouseResultType)responseItem).Item as exportHouseResultTypeLivingHouse;
                                        //проставляем характеристики дома из ГИСа
                                        if (houseDataItem.BasicCharacteristicts != null)
                                        {
                                            if (houseDataItem.BasicCharacteristicts.ItemElementName == ItemChoiceType3.CadastralNumber)
                                            {
                                                House.CadastralHouseNumber = houseDataItem.BasicCharacteristicts.Item.ToString();
                                            }
                                            if (houseDataItem.BasicCharacteristicts.TotalSquareSpecified)
                                            {
                                                House.AreaMkd = houseDataItem.BasicCharacteristicts.TotalSquare;
                                            }
                                            if (!string.IsNullOrEmpty(houseDataItem.BasicCharacteristicts.FloorCount))
                                            {
                                                if (houseDataItem.BasicCharacteristicts.FloorCount.Contains("-"))
                                                {
                                                    var floors = new List<int>();
                                                    foreach (string floorStr in houseDataItem.BasicCharacteristicts.FloorCount.Split("-"))
                                                    {
                                                        floors.Add(Convert.ToInt16(floorStr));
                                                    }

                                                    if (floors.Count > 0)
                                                    {
                                                        House.Floors = floors.Min();
                                                        House.MaximumFloors = floors.Max();
                                                    }
                                                    else
                                                    {
                                                        House.Floors = 1;
                                                        House.MaximumFloors = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    House.Floors = Convert.ToInt16(houseDataItem.BasicCharacteristicts.FloorCount);
                                                }
                                            }
                                            if (houseDataItem.BasicCharacteristicts.UsedYearSpecified)
                                            {
                                                House.BuildYear = Convert.ToInt16(houseDataItem.BasicCharacteristicts.UsedYear);
                                            }
                                        }                                     

                                        
                                        House.AreaLivingNotLivingMkd = House.AreaMkd;
                                        House.AreaLiving = House.AreaMkd;
                                        House.TypeHouse = Bars.Gkh.Enums.TypeHouse.Individual;

                                        // Прописываем дому ГИС ЖКХ GUID. Да, он приходит под тегом FIASHouseGuid, но может отличаться от ФИАС
                                        House.GisGkhGUID = ((exportHouseResultTypeLivingHouse)((exportHouseResultType)responseItem).Item).BasicCharacteristicts.FIASHouseGuid;
                                        if (!House.NumberGisGkhMatchedApartments.HasValue)
                                        {
                                            House.NumberGisGkhMatchedApartments = 0;
                                        }
                                        //House.NumberGisGkhMatchedNonResidental = NumOfMatchedNonRes;
                                        var state = StateDomain.GetAll().FirstOrDefault(x => x.Name == "Скорректирован" && x.TypeId == "gkh_real_obj");
                                        if (state != null)
                                        {
                                            House.State = state;
                                        }
                                        House.GisGkhMatchDate = DateTime.Now;
                                        RealityObjectRepo.Update(House);
                                        req.Answer = houseId + $" Данные из ГИС ЖКХ по дому {House.FiasAddress.AddressName} обработаны";
                                        req.RequestState = GisGkhRequestState.ResponseProcessed;
                                    }
                                }
                                else
                                {
                                    log += $"{DateTime.Now} Непонятный дом из ГИС ЖКХ";
                                    //// ищем дом по ГУИДу
                                    //var House = RealityObjectRepo.GetAll()
                                    //    .Where(x => x.FiasAddress.HouseGuid.ToString() ==
                                    //    ((exportHouseResultTypeApartmentHouse)((exportHouseResultType)responseItem).Item).BasicCharacteristicts.FIASHouseGuid)
                                    //    .FirstOrDefault();
                                    if (House != null)
                                    {
                                        log += $"{DateTime.Now} Дом в системе: {House.Address}\r\n";
                                        
                                        //House.NumberGisGkhMatchedNonResidental = NumOfMatchedNonRes;
                                        var state = StateDomain.GetAll().FirstOrDefault(x => x.Name == "Наличие не подтверждено" && x.TypeId == "gkh_real_obj");
                                        if (state != null)
                                        {
                                            House.State = state;
                                        }
                                        House.GisGkhMatchDate = DateTime.Now;
                                        RealityObjectRepo.Update(House);
                                        req.Answer = houseId + $" Данные из ГИС ЖКХ по дому {House.FiasAddress.AddressName} обработаны";
                                        req.RequestState = GisGkhRequestState.ResponseProcessed;
                                    }
                                }
                            }
                        }
                        log += $"{DateTime.Now} Обработка ответа завершена\r\n";
                        SaveLog(ref req, ref log);
                        GisGkhRequestsDomain.Update(req);
                    }
                    else
                    {
                        throw new Exception("Не найден файл с ответом из ГИС ЖКХ");
                    }
                }
                catch (Exception e)
                {
                    //req.RequestState = GisGkhRequestState.Error;
                    //GisGkhRequestsDomain.Update(req);
                    throw new Exception("Ошибка: " + e.Message);
                }
            }
        }

        #endregion

        #region Private methods

        private string NormalizeString(string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > maxLength)
            {
                return str.Substring(0, maxLength-1);
            }
            return str;
        }

        private void AddPremises(RealityObject House, exportHouseResultTypeApartmentHouseResidentialPremises prem)
        {
            var PremisesRepo = this.Container.ResolveRepository<GisGkhPremises>();
            var oldPremises = PremisesRepo.GetAll().Where(x => x.PremisesGUID == prem.PremisesGUID);
            foreach (var oldPremise in oldPremises)
            {
                PremisesRepo.Delete(oldPremise.Id);
            }
            var premise = new GisGkhPremises();
            
            switch (prem.ItemElementName)
            {
                case ItemChoiceType3.CadastralNumber:
                    premise.CadastralNumber = (string)prem.Item;
                    premise.No_RSO_GKN_EGRP_Data = null;
                    premise.No_RSO_GKN_EGRP_Registered = null;
                    break;
                case ItemChoiceType3.No_RSO_GKN_EGRP_Data:
                    premise.CadastralNumber = null;
                    premise.No_RSO_GKN_EGRP_Data = true;
                    premise.No_RSO_GKN_EGRP_Registered = null;
                    break;
                case ItemChoiceType3.No_RSO_GKN_EGRP_Registered:
                    premise.CadastralNumber = null;
                    premise.No_RSO_GKN_EGRP_Data = null;
                    premise.No_RSO_GKN_EGRP_Registered = true;
                    break;
            }
            if (prem.Item1 != null)
            {
                if (prem.Item1 is string)
                {
                    premise.EntranceNum = (string)prem.Item1;
                }
                else
                {
                    premise.EntranceNum = "";
                }
            }
            else
            {
                premise.EntranceNum = null;
            }
            premise.Floor = prem.Floor;
            if (prem.Item2 != null)
            {
                if (prem.Item2 is string)
                {
                    premise.GrossArea = (decimal)prem.Item2;
                }
                else
                {
                    premise.GrossArea = null;
                }
            }
            else
            {
                premise.GrossArea = null;
            }
            premise.IsCommonProperty = null;
            premise.ModificationDate = prem.ModificationDate;
            premise.PremisesGUID = prem.PremisesGUID;
            premise.PremisesNum = prem.PremisesNum;
            premise.PremisesUniqueNumber = prem.PremisesUniqueNumber;
            premise.RealityObject = House;
            premise.RoomType = Bars.Gkh.Enums.RoomType.Living;
            premise.TerminationDate = prem.TerminationDate;
            premise.TotalArea = prem.TotalArea;

            PremisesRepo.Save(premise);
        }

        private void AddPremises(RealityObject House, exportHouseResultTypeApartmentHouseNonResidentialPremises prem)
        {
            var PremisesRepo = this.Container.ResolveRepository<GisGkhPremises>();
            var oldPremises = PremisesRepo.GetAll().Where(x => x.PremisesGUID == prem.PremisesGUID);
            foreach (var oldPremise in oldPremises)
            {
                PremisesRepo.Delete(oldPremise.Id);
            }
            var premise = new GisGkhPremises();

            switch (prem.ItemElementName)
            {
                case ItemChoiceType3.CadastralNumber:
                    premise.CadastralNumber = (string)prem.Item;
                    premise.No_RSO_GKN_EGRP_Data = null;
                    premise.No_RSO_GKN_EGRP_Registered = null;
                    break;
                case ItemChoiceType3.No_RSO_GKN_EGRP_Data:
                    premise.CadastralNumber = null;
                    premise.No_RSO_GKN_EGRP_Data = true;
                    premise.No_RSO_GKN_EGRP_Registered = null;
                    break;
                case ItemChoiceType3.No_RSO_GKN_EGRP_Registered:
                    premise.CadastralNumber = null;
                    premise.No_RSO_GKN_EGRP_Data = null;
                    premise.No_RSO_GKN_EGRP_Registered = true;
                    break;
            }
            premise.EntranceNum = null;
            premise.Floor = prem.Floor;
            premise.GrossArea = null;
            premise.IsCommonProperty = prem.IsCommonProperty;
            premise.ModificationDate = prem.ModificationDate;
            premise.PremisesGUID = prem.PremisesGUID;
            premise.PremisesNum = prem.PremisesNum;
            premise.PremisesUniqueNumber = prem.PremisesUniqueNumber;
            premise.RealityObject = House;
            premise.RoomType = Bars.Gkh.Enums.RoomType.NonLiving;
            premise.TerminationDate = prem.TerminationDate;
            premise.TotalArea = prem.TotalArea;

            PremisesRepo.Save(premise);
        }

        /// <summary>
        /// Сериаилазация запроса
        /// </summary>
        /// <param name="data">Запрос</param>
        /// <returns>Xml-документ</returns>
        protected XmlDocument SerializeRequest(object data)
        {
            var type = data.GetType();
            XmlDocument result;

            var attr = (XmlTypeAttribute)type.GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(type, attr.Namespace);

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, data);

                    result = new XmlDocument();
                    result.LoadXml(stringWriter.ToString());
                }
            }

            //var prefixer = new XmlNsPrefixer();
            //prefixer.Process(result);

            return result;
        }

        protected TDataType DeserializeData<TDataType>(string data)
        {
            TDataType result;

            var attr = (XmlTypeAttribute)typeof(TDataType).GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(typeof(TDataType), attr.Namespace);

            using (var reader = XmlReader.Create(new StringReader(data)))
            {
                result = (TDataType)xmlSerializer.Deserialize(reader);
            }

            return result;
        }

        private Bars.B4.Modules.FileStorage.FileInfo SaveFile(byte[] data, string fileName)
        {
            if (data == null)
                return null;

            try
            {
                //сохраняем пакет
                return _fileManager.SaveFile(fileName, data);
            }
            catch (Exception eeeeeeee)
            {
                return null;
            }
        }

        private void SaveFile(GisGkhRequests req, GisGkhFileType fileType, XmlDocument data, string fileName)
        {
            if (data == null)
                throw new Exception("Пустой документ для сохранения");

            MemoryStream stream = new MemoryStream();
            data.PreserveWhitespace = true;
            data.Save(stream);
            try
            {
                //сохраняем
                GisGkhRequestsFileDomain.Save(new GisGkhRequestsFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    GisGkhRequests = req,
                    GisGkhFileType = fileType,
                    FileInfo = _fileManager.SaveFile(stream, fileName)
                });
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при сохранении файла: " + e.Message);
            }
        }

        private void SaveLog(ref GisGkhRequests req, ref string log)
        {
            if (req.LogFile != null)
            {
                var FileInfo = req.LogFile;
                req.LogFile = null;
                GisGkhRequestsDomain.Update(req);
                _fileManager.Delete(FileInfo);
                var fullPath = $"{((FileSystemFileManager)_fileManager).FilesDirectory.FullName}\\{FileInfo.ObjectCreateDate.Year}\\{FileInfo.ObjectCreateDate.Month}\\{FileInfo.Id}.{FileInfo.Extention}";
                //var fullPath = $"{FtpPath}\\{FileInfo.ObjectCreateDate.Year}\\{FileInfo.ObjectCreateDate.Month}\\{FileInfo.Id}.{FileInfo.Extention}";
                try
                {
                    System.IO.File.Delete(fullPath);
                }
                catch
                {
                    log += $"{DateTime.Now} Не удалось удалить старый лог-файл\r\n";
                }
            }
            req.LogFile = _fileManager.SaveFile("log.txt", Encoding.UTF8.GetBytes(log));
            //GisGkhRequestsDomain.Update(req);
        }

        #endregion

    }
}
