namespace Bars.Gkh.Regions.Voronezh.Imports
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4;
    using Entities;
    using Ionic.Zip;
    using Castle.Windsor;
    using B4.DataAccess;
    using Import.Impl;
    using Import;
    using Gkh.Enums.Import;
    using System.Xml.Serialization;
    using Schema;
    using Dapper;

    public class RosRegExtractImport : GkhImportBase
    {
        #region Base
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "ImportRosRegExtract"; }
        }

        public override string Name
        {
            get { return "Импорт выписок из Росреестра"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "zip"; }
        }

        public override string PermissionName
        {
            get { return "Import.RosRegExtract"; }
        }
        private void InitLog(string fileName)
        {
            LogManager = Container.Resolve<ILogImportManager>();
            if (LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }
            LogImport.ImportKey = this.Name;
            LogManager.FileNameWithoutExtention = fileName;
            LogManager.UploadDate = DateTime.Now;
        }
        #endregion
        #region Services
        public IDomainService<RosRegExtractGov> rreGovService { get; set; }
        public IDomainService<RosRegExtractPers> rrePersService { get; set; }
        public IDomainService<RosRegExtractOrg> rreOrgService { get; set; }
        public IDomainService<RosRegExtract> rreService { get; set; }
        public IDomainService<RosRegExtractRight> rreRightService { get; set; }
        public IDomainService<RosRegExtractDesc> rreDescService { get; set; }
        public IDomainService<RosRegExtractOwner> rreOwnerService { get; set; }
        public IDomainService<RosRegExtractReg> rreRegService { get; set; }
        public ISessionProvider SessionProvider { get; private set; }
        #endregion
        //Таймер для замера локальных операций, чтобы не создавать объект каждый раз
        private Stopwatch local_stopwatch;
        //Таймер выполнения импорта
        private Stopwatch global_stopwatch;
        public override ImportResult Import(BaseParams baseParams)
        {
            local_stopwatch = new Stopwatch();
            global_stopwatch = new Stopwatch();
            global_stopwatch.Start();
            var file = baseParams.Files["FileImport"];
            InitLog(file.FileName);

            using (var zipfileMemoryStream = new MemoryStream(file.Data))
            {
                using (var zipFile = ZipFile.Read(zipfileMemoryStream))
                {
                    var zipEntries = zipFile.Where(x => x.FileName.EndsWith(".xml")).ToArray();

                    if (zipEntries.Length < 1)
                    {
                        LogImport.Error("Ошибка", "Отсутствуют файлы для импорта");
                        return new ImportResult(StatusImport.CompletedWithError, "Отсутствуют файлы для импорта");
                    }

                    this.ImportInternal(zipEntries, zipFile.Name);
                }
            }
            LogImport.SetFileName(file.FileName);
            LogImport.ImportKey = this.CodeImport;

            LogImportManager.FileNameWithoutExtention = file.FileName;
            LogImportManager.Add(file, LogImport);
            LogImportManager.Save();

            var statusImport = LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : LogImport.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError;

            global_stopwatch.Stop();

            return new ImportResult(statusImport, string.Format("time elapsed: {0} ms;", global_stopwatch.ElapsedMilliseconds));
        }  
        private void ImportInternal(ZipEntry[] zipEntries, string archiveName)
        {
            var internal_stopwatch = new Stopwatch(); //Таймер цикла
            int indicator = 0;
            using (var zipfileMemoryStream = new MemoryStream())
            {
                var readOptions = new ReadOptions();
                readOptions.Encoding = Encoding.GetEncoding(866);
                foreach (ZipEntry zipentry in zipEntries)
                {
                    indicator++;
                    Indicate(100*indicator/zipEntries.Length,$"{indicator} выписок из {zipEntries.Length} загружено");
                    //Исправление кодировки для кириллических имен файлов
                    string filename = Encoding.GetEncoding(866).GetString(Encoding.GetEncoding(437).GetBytes(zipentry.FileName));
                    try
                    {
                        Debug.WriteLine($@"IMPORT BEGIN: {filename}");
                        internal_stopwatch.Restart();

                        //Десериализация в классы
                        Debug.WriteLine("IMPORT READING ZIPFILESTREAM");
                        using (Stream zipFileStream = new MemoryStream())
                        {
                            zipentry.Extract(zipFileStream);
                            var ser = new XmlSerializer(typeof(Extract));
                            Extract xDoc = null;
                            Debug.WriteLine("IMPORT DESERIALIZATION START");
                            try
                            {
                                zipFileStream.Seek(0, SeekOrigin.Begin);
                                xDoc = (Extract)ser.Deserialize(zipFileStream);
                            }
                            catch (Exception e)
                            {
                                //No Action
                                LogImport.Error(e.Message, e.StackTrace, "");
                            }

                            Debug.WriteLine($@"IMPORT DESERIALIZED");
                            //Создание записи описания
                            var rreDesc = getDesc(xDoc);
                            //rreDescService.Save(rreDesc);

                            //Прикрепление XML к описанию
                            string xmlstring = extractXML(zipentry);
                            attachXMLtoDesc(xmlstring, rreDesc.Id);

                            //Обработка прав на собственность
                            var objRight = xDoc.ReestrExtract.ExtractObjectRight.ExtractObject.ObjectRight.Right;
                            foreach (var right in objRight)
                            {
                                //Данные о регистрации права
                                var rreReg = getReg(right);
                                //rreRegService.Save(rreReg);
                                var objOwner = right.Owner;
                                if (objOwner != null)
                                {
                                    foreach (var owner in objOwner)
                                    {
                                        //Собственник
                                        var rreOwner = new RosRegExtractOwner();
                                        RosRegExtractGov rreGov = getGov(owner);
                                        rreOwner.gov_id = rreGov;
                                        RosRegExtractOrg rreOrg = getOrg(owner);
                                        rreOwner.org_id = rreOrg;
                                        RosRegExtractPers rrePers = getPers(owner);
                                        rreOwner.pers_id = rrePers;
                                        rreOwnerService.Save(rreOwner);

                                        //Право
                                        var rreRight = new RosRegExtractRight();
                                        rreRight.reg_id = rreReg;
                                        rreRight.RightNumber = right?.RightNumber.ToString();
                                        rreRight.owner_id = rreOwner;
                                        rreRightService.Save(rreRight);

                                        //Связь описания и права
                                        RosRegExtract rre = new RosRegExtract();
                                        rre.desc_id = rreDesc;
                                        rre.right_id = rreRight;
                                        rreService.Save(rre);
                                    }
                                }
                                internal_stopwatch.Stop();

                                Debug.WriteLine($@"IMPORT END: {filename} - {internal_stopwatch.ElapsedMilliseconds} ms");
                                Debug.WriteLine("************************************************************");
                            }
                        }
                        LogImport.Info(filename + " - " + internal_stopwatch.ElapsedMilliseconds + "мс", "Импортировано успешно", "");
                        
                    }
                    catch (Exception e)
                    {
                        LogImport.Error(filename + " - " + internal_stopwatch.ElapsedMilliseconds + "мс", e.Message,"");
                        Debug.WriteLine($"IMPORT ERROR: {indicator}/{zipEntries.Length} - {filename}");
                        Debug.WriteLine($"IMPORT ERROR: {e.Message}");
                        Debug.WriteLine($"IMPORT ERROR: {e.InnerException.Message}");
                    }
                }
                Debug.WriteLine($@"IMPORT END: {global_stopwatch.ElapsedMilliseconds} ms");
            }
        }

        //Методы получения сущностей для базы из десериализованного XML-файла
        #region getMethods
        private RosRegExtractDesc getDesc(Extract xDoc)
        {
            Debug.WriteLine("IMPORT Desc parsing begin");
            var objDesc = xDoc.ReestrExtract.ExtractObjectRight.ExtractObject.ObjectRight.ObjectDesc;
            local_stopwatch.Restart();
            RosRegExtractDesc rreDesc = new RosRegExtractDesc();
            #region assignments
            rreDesc.Desc_ID_Object = objDesc?.ID_Object.ToString();
            rreDesc.Desc_CadastralNumber = objDesc?.CadastralNumber;
            rreDesc.Desc_ObjectType = objDesc?.ObjectType.ToString();
            rreDesc.Desc_ObjectTypeText = objDesc?.ObjectTypeText;
            rreDesc.Desc_ObjectTypeName = objDesc?.Name;
            rreDesc.Desc_AssignationCode = objDesc?.Assignation_Code.ToString();
            rreDesc.Desc_AssignationCodeText = objDesc?.Assignation_Code_Text;
            rreDesc.Desc_Area = objDesc?.Area?.Area.ToString();
            rreDesc.Desc_AreaText = objDesc?.Area?.AreaText;
            rreDesc.Desc_AreaUnit = objDesc?.Area?.Unit.ToString();
            rreDesc.Desc_Floor = objDesc?.Floor;
            rreDesc.Desc_ID_Address = objDesc?.Address.ID_Address.ToString();
            rreDesc.Desc_AddressContent = objDesc?.Address?.Content;
            rreDesc.Desc_RegionCode = objDesc?.Address?.Region?.Code.ToString();
            rreDesc.Desc_RegionName = objDesc?.Address?.Region.Name;
            rreDesc.Desc_OKATO = objDesc?.Address?.Code_OKATO.ToString();
            rreDesc.Desc_KLADR = objDesc?.Address?.Code_KLADR.ToString();
            rreDesc.Desc_CityName = objDesc?.Address?.City?.Name;
            rreDesc.Desc_Urban_District = objDesc?.Address?.Urban_District?.Name;
            rreDesc.Desc_Locality = objDesc?.Address?.Locality?.Name;
            rreDesc.Desc_StreetName = objDesc?.Address?.Street?.Name;
            rreDesc.Desc_Level1Name = objDesc?.Address?.Level1?.Name;
            rreDesc.Desc_Level2Name = objDesc?.Address?.Level2?.Name;
            rreDesc.Desc_ApartmentName = objDesc?.Address?.Apartment?.Name;

            //Header fields
            rreDesc.ExtractDate = xDoc?.ReestrExtract?.DeclarAttribute?.ExtractDate?.ToString(); //дата
            rreDesc.ExtractNumber = xDoc?.ReestrExtract?.DeclarAttribute?.ExtractNumber?.ToString(); //Номер выписки
            rreDesc.HeadContent = xDoc?.ReestrExtract?.ExtractObjectRight?.HeadContent?.Content?.ToString(); //шапка выписки

            rreDesc.Registrator = xDoc?.ReestrExtract?.DeclarAttribute?.Registrator?.ToString(); //фио для футера
            rreDesc.Appointment = xDoc?.eDocument?.Sender?.Appointment?.ToString(); //Должность
            //Поля 5-9 выписки
            var objExObj = xDoc?.ReestrExtract?.ExtractObjectRight?.ExtractObject;
            rreDesc.NoShareHolding = objExObj?.ObjectRight?.NoShareHolding?.ToString();
            rreDesc.RightAgainst = objExObj?.RightAgainst?.ToString(); //6?
            rreDesc.RightAssert = objExObj?.RightAssert?.ToString(); //7?
            rreDesc.RightClaim = objExObj?.RightClaim?.ToString(); //8?
            rreDesc.RightSteal = objExObj?.RightSteal?.ToString(); //9
            #endregion
            local_stopwatch.Stop();
            Debug.WriteLine("IMPORT Desc parsing end: " + local_stopwatch.ElapsedMilliseconds);

            Debug.WriteLine("IMPORT Desc saving begin: " + local_stopwatch.ElapsedMilliseconds);
            rreDescService.Save(rreDesc);
            
            Debug.WriteLine("IMPORT Desc saving end: " + local_stopwatch.ElapsedMilliseconds);
            return rreDesc;
        }
        private RosRegExtractGov getGov(ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwner owner)
        {
            local_stopwatch.Restart();
            var objGov = owner.Governance != null ? owner.Governance : null;
            if (objGov != null)
            {
                Debug.WriteLine("IMPORT Gov parsing begin");
                local_stopwatch.Restart();
                var rreGov = new RosRegExtractGov();
                rreGov.Gov_Code_SP = owner?.Governance?.Code_SP.ToString();
                rreGov.Gov_Content = owner?.Governance?.Content;
                rreGov.Gov_Name = owner?.Governance?.Name;
                rreGov.Gov_OKATO_Code = owner?.Governance?.OKATO_Code.ToString();
                rreGov.Gov_Country = owner?.Governance?.Country;
                rreGov.Gov_Address = owner?.Governance?.Address;
                rreGovService.Save(rreGov);
                local_stopwatch.Stop();
                Debug.WriteLine("IMPORT Gov parsing end: " + local_stopwatch.ElapsedMilliseconds);
                return rreGov;
            }
            return null;
        }
        private RosRegExtractPers getPers(ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwner owner)
        {
            local_stopwatch.Restart();
            var objPers = owner.Person != null ? owner.Person : null;

            if (objPers != null)
            {
                Debug.WriteLine("IMPORT Person parsing begin");
                var rrePers = new RosRegExtractPers();
                #region assignments
                rrePers.Pers_Code_SP = owner.Person?.Code_SP.ToString();
                rrePers.Pers_Content = owner.Person?.Content;
                rrePers.Pers_FIO_Surname = owner.Person?.FIO?.Surname;
                rrePers.Pers_FIO_First = owner.Person?.FIO?.First;
                rrePers.Pers_FIO_Patronymic = owner.Person?.FIO?.Patronymic;
                rrePers.Pers_DateBirth = owner.Person?.DateBirth;
                rrePers.Pers_Place_Birth = owner.Person?.Place_Birth;
                rrePers.Pers_Citizen = owner.Person?.Citizen;
                rrePers.Pers_Sex = owner.Person?.Sex;
                rrePers.Pers_DocContent = owner.Person?.Document?.Content;
                rrePers.Pers_DocType_Document = owner.Person?.Document?.Type_Document.ToString();
                rrePers.Pers_DocName = owner.Person?.Document?.Name;
                rrePers.Pers_DocSeries = owner.Person?.Document?.Series;
                rrePers.Pers_DocNumber = owner.Person?.Document?.Number;
                rrePers.Pers_DocDate = owner.Person?.Document?.Date;
                rrePers.Pers_SNILS = owner.Person?.SNILS;
                //Person->Location
                rrePers.Pers_Loc_ID_Address = owner.Person?.Location?.ID_Address.ToString();
                rrePers.Pers_Loc_Content = owner.Person?.Location?.Content;
                rrePers.Pers_Loc_CountryCode = owner.Person?.Location?.Country?.Code.ToString();
                rrePers.Pers_Loc_CountryName = owner.Person?.Location?.Country?.Name;
                rrePers.Pers_Loc_RegionCode = owner.Person?.Location?.Region?.Code.ToString();
                rrePers.Pers_Loc_RegionName = owner.Person?.Location?.Region?.Name;
                rrePers.Pers_Loc_Code_OKATO = owner.Person?.Location?.Code_OKATO.ToString();
                rrePers.Pers_Loc_Code_KLADR = owner.Person?.Location?.Code_KLADR.ToString();
                rrePers.Pers_Loc_DistrictName = owner.Person?.Location?.District?.Name;
                rrePers.Pers_Loc_Urban_DistrictName = owner.Person?.Location?.Urban_District?.Name;
                rrePers.Pers_Loc_LocalityName = owner.Person?.Location?.Locality?.Name;
                rrePers.Pers_Loc_StreetName = owner.Person?.Location?.Street?.Name;
                rrePers.Pers_Loc_Level1Name = owner.Person?.Location?.Level1?.Name;
                rrePers.Pers_Loc_Level2Name = owner.Person?.Location?.Level2?.Name;
                rrePers.Pers_Loc_Level3Name = owner.Person?.Location?.Level3?.Name;
                rrePers.Pers_Loc_ApartmentName = owner.Person?.Location?.Apartment?.Name;
                rrePers.Pers_Loc_Other = owner.Person?.Location?.Other;
                //Person->FactLocation
                //Polymorphic array
                var loc_items = owner?.Person?.FactLocation?.Items;
                var loc_ident = owner?.Person?.FactLocation?.ItemsElementName;

                ulong? ID_Address = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.ID_Address) as ulong?;
                string Content = (string)XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Content) as string;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationCountry Country = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Country) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationCountry;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationRegion Region = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Region) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationRegion;
                ulong? Code_OKATO = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Code_OKATO) as ulong?;
                ulong? Code_KLADR = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Code_KLADR) as ulong?;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationDistrict District = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.District) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationDistrict;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationUrban_District Urban_District = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Urban_District) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationUrban_District;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationLocality Flocality = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Locality) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationLocality;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationStreet Street = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Street) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationStreet;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationLevel1 Level1 = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Level1) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationLevel1;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationLevel2 Level2 = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Level2) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationLevel2;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationLevel3 Level3 = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Level3) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationLevel3;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationApartment Apartment = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Apartment) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerPersonFactLocationApartment;
                string Other = (string)XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType1.Other);

                rrePers.Pers_Floc_ID_Address = ID_Address.ToString();
                rrePers.Pers_Floc_Content = Content;
                rrePers.Pers_Floc_CountryCode = Country?.Code.ToString();
                rrePers.Pers_Floc_CountryName = Country?.Name;
                rrePers.Pers_Floc_RegionCode = Region?.Code.ToString();
                rrePers.Pers_Floc_RegionName = Region?.Name;
                rrePers.Pers_Floc_Code_OKATO = Code_OKATO.ToString();
                rrePers.Pers_Floc_Code_KLADR = Code_KLADR.ToString();
                rrePers.Pers_Floc_DistrictName = District?.Name;
                rrePers.Pers_Floc_Urban_DistrictName = Urban_District?.Name;
                rrePers.Pers_Floc_FlocalityName = Flocality?.Name;
                rrePers.Pers_Floc_StreetName = Street?.Name;
                rrePers.Pers_Floc_Level1Name = Level1?.Name;
                rrePers.Pers_Floc_Level2Name = Level2?.Name;
                rrePers.Pers_Floc_Level3Name = Level3?.Name;
                rrePers.Pers_Floc_ApartmentName = Apartment?.Name;
                rrePers.Pers_Floc_Other = Other;
                #endregion
                rrePersService.Save(rrePers);
                local_stopwatch.Stop();
                Debug.WriteLine("IMPORT Person parsing end: " + local_stopwatch.ElapsedMilliseconds);
                return rrePers;
            }
            return null;
        }
        private RosRegExtractOrg getOrg(ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwner owner)
        {
            local_stopwatch.Restart();
            var objOrg = owner.Organization != null ? owner.Organization : null;

            if (objOrg != null)
            {
                var sw = new Stopwatch();
                Debug.WriteLine("IMPORT Org parsing begin");
                var rreOrg = new RosRegExtractOrg();
                #region assignments
                rreOrg.Org_Code_SP = owner?.Organization.Code_SP.ToString();
                rreOrg.Org_Content = owner?.Organization?.Content;
                rreOrg.Org_Code_OPF = owner?.Organization?.Code_OPF.ToString();
                rreOrg.Org_Name = owner?.Organization?.Name;
                rreOrg.Org_Inn = owner?.Organization?.INN.ToString();
                rreOrg.Org_Code_OGRN = owner?.Organization?.Code_OGRN.ToString();
                rreOrg.Org_RegDate = owner?.Organization?.RegDate;
                rreOrg.Org_AgencyRegistration = owner?.Organization?.AgencyRegistration;
                rreOrg.Org_Code_CPP = owner?.Organization?.Code_CPP.ToString();
                //Organization->Location
                //Polymorphic array
                var loc_items = owner?.Organization?.Location?.Items;
                var loc_ident = owner?.Organization?.Location?.ItemsElementName;

                ulong? ID_Address = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType.ID_Address) as ulong?;
                string Content = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType.Content) as string;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerOrganizationLocationRegion RegionCode = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType.Region) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerOrganizationLocationRegion;
                ulong? Code_OKATO = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType.Code_OKATO) as ulong?;
                ulong? Code_KLADR = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType.Code_KLADR) as ulong?;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerOrganizationLocationCity City = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType.City) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerOrganizationLocationCity;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerOrganizationLocationStreet Street = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType.Street) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerOrganizationLocationStreet;
                ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerOrganizationLocationLevel1 Level1 = XMLPolyArrHelper.GetItem(loc_items, loc_ident, ItemsChoiceType.Level1) as ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRightOwnerOrganizationLocationLevel1;

                rreOrg.Org_Loc_ID_Address = ID_Address.ToString();
                rreOrg.Org_Loc_Content = Content?.ToString();
                rreOrg.Org_Loc_RegionCode = RegionCode?.Code.ToString();
                rreOrg.Org_Loc_RegionName = RegionCode?.Name.ToString();
                rreOrg.Org_Loc_Code_OKATO = Code_OKATO.ToString();
                rreOrg.Org_Loc_Code_KLADR = Code_KLADR.ToString();
                rreOrg.Org_Loc_CityName = City?.Name;
                rreOrg.Org_Loc_StreetName = Street?.Name;
                rreOrg.Org_Loc_Level1Name = Level1?.Name;

                //Organization->FactLocation
                rreOrg.Org_FLoc_ID_Address = owner?.Organization?.FactLocation?.ID_Address.ToString();
                rreOrg.Org_FLoc_Content = owner?.Organization?.FactLocation?.Content;
                rreOrg.Org_FLoc_RegionCode = owner?.Organization?.FactLocation?.Region.Code.ToString();
                rreOrg.Org_FLoc_RegionName = owner?.Organization?.FactLocation?.Region.Name;
                rreOrg.Org_FLoc_Code_OKATO = owner?.Organization?.FactLocation?.Code_OKATO.ToString();
                rreOrg.Org_FLoc_Code_KLADR = owner?.Organization?.FactLocation?.Code_KLADR.ToString();
                rreOrg.Org_FLoc_CityName = owner?.Organization?.FactLocation?.City?.Name;
                rreOrg.Org_FLoc_StreetName = owner?.Organization?.FactLocation?.Street?.Name;
                rreOrg.Org_FLoc_Level1Name = owner?.Organization?.FactLocation?.Level1?.Name;
                #endregion
                rreOrgService.Save(rreOrg);
                sw.Stop();
                Debug.WriteLine("IMPORT Org parsing end: " + sw.ElapsedMilliseconds);
                return rreOrg;
            }
            return null;
        }
        private RosRegExtractReg getReg(ExtractReestrExtractExtractObjectRightExtractObjectObjectRightRight right)
        {
            local_stopwatch.Restart();
            Debug.WriteLine("IMPORT Reg parsing begin");
            var rreReg = new RosRegExtractReg();
            rreReg.Reg_ID_Record = right?.Registration?.ID_Record.ToString();
            rreReg.Reg_RegNumber = right?.Registration?.RegNumber;
            rreReg.Reg_Type = right?.Registration?.Type.ToString();
            rreReg.Reg_Name = right?.Registration?.Name;
            rreReg.Reg_RegDate = right?.Registration?.RegDate;
            rreReg.Reg_ShareText = right?.Registration?.ShareText;
            Debug.WriteLine("IMPORT Reg parsing end");
            Debug.WriteLine("IMPORT Reg saving begin");
            rreRegService.Save(rreReg);
            local_stopwatch.Stop();
            Debug.WriteLine("IMPORT Reg saving end: " + local_stopwatch.ElapsedMilliseconds);
            //return rreRegService.GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
            return rreReg;
        }
        #endregion
        /// <summary>
        /// Прикрепление XML-файла к записи описания
        /// </summary>
        /// <param name="xmlstring">XML</param>
        /// <param name="desc_id">Id записи</param>
        private void attachXMLtoDesc(string xmlstring, long desc_id)
        {
            local_stopwatch.Restart();
            Debug.WriteLine($@"IMPORT SQL INSERT XML BEGIN");
            //Вставка прямым запросом, т.к. нхибернейт не желает работать с типом данных xml
            var StatelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

            var connection = StatelessSession.Connection;

            var sql = $@"UPDATE import.rosregextractdesc
                                    SET XML='{xmlstring}'
                                    WHERE ID={desc_id}";
            connection.Execute(sql);
            connection.Close();
            connection.Dispose();
            Container.Release(StatelessSession);
            local_stopwatch.Stop();
            Debug.WriteLine($@"IMPORT SQL INSERT: {local_stopwatch.ElapsedMilliseconds} ms");
        }
        /// <summary>
        /// Получение XML-файла из записи архива
        /// </summary>
        /// <param name="zipentry">Архивная запись</param>
        /// <returns></returns>
        private string extractXML(ZipEntry zipentry)
        {
            local_stopwatch.Restart();
            Debug.WriteLine($@"IMPORT XML EXTRACT BEGIN");
            var ms = new MemoryStream();
            zipentry.Extract(ms);
            ms.Position = 0;
            var sr = new StreamReader(ms);
            var xml = sr.ReadToEnd();
            sr.Close();
            ms.Close();
            
            local_stopwatch.Stop();
            Debug.WriteLine($@"IMPORT XML EXTRACT: {local_stopwatch.ElapsedMilliseconds} ms");

            return xml;
        }        
    }
    public static class XMLPolyArrHelper
    {
        public static TResult GetItem<TIDentifier, TResult>(TResult[] items, TIDentifier[] itemIdentifiers, TIDentifier itemIdentifier)
        {
            if (itemIdentifiers == null)
            {
                Debug.Assert(items == null);
                return default(TResult);
            }
            Debug.Assert(items.Length == itemIdentifiers.Length);
            var i = Array.IndexOf(itemIdentifiers, itemIdentifier);
            if (i < 0)
                return default(TResult);
            return items[i];
        }
    }
}
