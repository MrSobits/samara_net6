namespace Bars.GkhGji.Regions.Habarovsk.Interceptors
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Entities;
    using Enums;
    using System;
    using System.Linq;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    class GisGmpInterceptor : EmptyDomainInterceptor<GisGmp>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<GisGmpFile> GisGmpFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }

        public IDomainService<Protocol197ArticleLaw> Protocol197ArticleLawDomain { get; set; }

        public IDomainService<ResolProsArticleLaw> ResolProsArticleLawDomain { get; set; }

        public IDomainService<Protocol197> Protocol197Domain { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        //public IDomainService<FLDocType> FLDocTypeDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<GisGmp> service, GisGmp entity)
        {
            try
            {
                //#if DEBUG
                //entity.Inspector = InspectorDomain.GetAll().First();
                //#else
                if (entity.Inspector == null)
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
                    if (thisOperator?.Inspector == null)
                        return Failure("Обмен информацией с ГИС ГМП доступен только сотрудникам ГЖИ");

                    entity.Inspector = thisOperator.Inspector;
                }
//#endif
                entity.CalcDate = DateTime.Now;
                entity.ObjectCreateDate = DateTime.Now;
                entity.ObjectEditDate = DateTime.Now;
                entity.ObjectVersion = 1;
                entity.RequestState = RequestState.NotFormed;
                entity.Status = entity.GISGMPPayerStatus.Code;
                entity.PaytReason = "0";
                entity.TaxPeriod = "0";
                if (entity.TypeLicenseRequest == TypeLicenseRequest.NotSet && entity.Protocol != null)
                {
                    entity.TaxDocNumber = entity.Protocol.DocumentNumber;
                    if (entity.Protocol.DocumentDate.HasValue)
                    {
                        if (entity.Protocol.DocumentDate.Value < new DateTime(2020, 1, 1))
                        {
                            entity.KBK = "07811610123010111140";
                        }                       
                    }
                }
                if (entity.TypeLicenseRequest == TypeLicenseRequest.First && entity.ManOrgLicenseRequest != null)
                {
                    entity.TaxDocNumber = entity.ManOrgLicenseRequest.RegisterNumber;
                    entity.KBK = "80110807400010000110";
                }
                if ((entity.TypeLicenseRequest == TypeLicenseRequest.Copy || entity.TypeLicenseRequest == TypeLicenseRequest.Reissuance) && entity.LicenseReissuance != null)
                {
                    entity.TaxDocNumber = entity.LicenseReissuance.RegisterNum.HasValue? entity.LicenseReissuance.RegisterNum.Value.ToString(): entity.LicenseReissuance.Id.ToString();
                }
                entity.TaxDocDate = "0";// (entity.Protocol?.DocumentDate?.ToString("dd.MM.yyyy") ?? entity.CalcDate.ToShortDateString());

                entity.PaymentType = "0";
                //
                entity.UIN = GetUIN(entity);
                entity.AltPayerIdentifier = CreateAltPayerIdentifier(entity);
                //
                entity.RequestState = RequestState.Formed;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<GisGmp>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<GisGmp> service, GisGmp entity)
        {
            try
            {
//#if DEBUG
//                entity.Inspector = InspectorDomain.GetAll().First();
//#else
                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator?.Inspector == null)
                {

                }
                else
                {
                 // entity.Inspector = thisOperator.Inspector;
                }

                    
//#endif
                entity.ObjectEditDate = DateTime.Now;
                if (entity.TypeLicenseRequest == TypeLicenseRequest.NotSet && entity.Protocol != null)
                {
                    entity.TaxDocNumber = entity.Protocol.DocumentNumber;
                }
                if (entity.TypeLicenseRequest == TypeLicenseRequest.First && entity.ManOrgLicenseRequest != null)
                {
                    entity.TaxDocNumber = entity.ManOrgLicenseRequest.RegisterNumber;
                }
                if ((entity.TypeLicenseRequest == TypeLicenseRequest.Copy || entity.TypeLicenseRequest == TypeLicenseRequest.Reissuance) && entity.LicenseReissuance != null)
                {
                    entity.TaxDocNumber = entity.LicenseReissuance.RegisterNum.HasValue ? entity.LicenseReissuance.RegisterNum.Value.ToString() : entity.LicenseReissuance.Id.ToString();
                }
                entity.TaxDocDate = "0"; //(entity.Protocol?.DocumentDate?.ToString("dd.MM.yyyy") ?? entity.CalcDate.ToShortDateString());
                entity.Status = entity.GISGMPPayerStatus.Code;
                entity.AltPayerIdentifier = CreateAltPayerIdentifier(entity);
                if (entity.GisGmpChargeType == GisGmpChargeType.First)
                {
                    entity.UIN = GetUIN(entity);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<GisGmp>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<GisGmp> service, GisGmp entity)
        {
            try
            {
                //чистка приаттаченных файлов
                GisGmpFileDomain.GetAll()
               .Where(x => x.GisGmp.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => GisGmpFileDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<GisGmp>: {e.ToString()}");
            }
        }

        string cachedId = null;

        private string GetOriginatorIdFromConfig()
        {
            if (cachedId != null)
                return cachedId;

            var configProvider = Container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("Bars.GkhGji.Regions.Voronezh");
            string id = config.GetAs("OriginatorId", (string)null, true);

            if (id == null)
                throw new Exception("Не определен OriginatorId в user.config в секции Bars.GkhGji.Regions.Voronezh");

            cachedId = id;
            return cachedId;
        }

        private string GetUIN(GisGmp entity)
        {
            if (entity.TypeLicenseRequest == TypeLicenseRequest.NotSet && entity.Protocol != null)
            {
                string s1 = Convert.ToInt32(GetOriginatorIdFromConfig(), 16).ToString().PadLeft(8, '0');
                string s2 = (entity.Protocol?.DocumentDate?.ToString("yyyyMMdd") ?? "00000000");
                string s3 = "";
                if (entity.Protocol.DocumentNumber.Contains("-"))
                {
                    if (entity.Protocol.DocumentNumber.Split('-').Count() > 2)
                    {
                        s3 = (entity.Protocol.DocumentNumber.Split('-')[1] + entity.Protocol.DocumentNumber.Split('-')[2]).PadRight(8, '0');
                    }
                    else if (entity.Protocol.DocumentNumber.Split('-').Count() == 2)
                    {
                        s3 = entity.Protocol.DocumentNumber.Split('-')[1].PadRight(8, '0');
                    }
                    else
                    {
                        s3 = entity.Protocol.DocumentNumber.Replace("-", "").PadRight(8, '0');
                    }
                }
                else
                {
                    s3 = entity.Protocol.DocumentNumber.PadRight(8, '0');
                }
                s3 = s3.Replace('/', '1');
                s3 = s3.Replace('\\', '0');
                s3 = s3.Replace('№', '4');
                char[] charsS3 = s3.ToCharArray();
                for (int i = 0; i < s3.Length; i++)
                {
                    if (!char.IsDigit(charsS3[i]))
                    {
                        s3 = s3.Replace(charsS3[i], '0');
                    }
                }
                string UIN = (s1 + s2 + s3).Substring(0, 24);
                return UIN + CheckSum(UIN);
            }
            else if (entity.TypeLicenseRequest == TypeLicenseRequest.First && entity.ManOrgLicenseRequest != null)
            {
                string s1 = Convert.ToInt32(GetOriginatorIdFromConfig(), 16).ToString().PadLeft(8, '0');
                string s2 = (entity.ManOrgLicenseRequest?.DateRequest?.ToString("yyyyMMdd") ?? "00000000");
                string s3 = "";
                if (entity.ManOrgLicenseRequest.RegisterNumber.Contains("-"))
                {
                    if (entity.ManOrgLicenseRequest.RegisterNumber.Split('-').Count() > 2)
                    {
                        s3 = (entity.ManOrgLicenseRequest.RegisterNumber.Split('-')[1] + entity.ManOrgLicenseRequest.RegisterNumber.Split('-')[2]).PadRight(8, '0');
                    }
                    else if (entity.ManOrgLicenseRequest.RegisterNumber.Split('-').Count() == 2)
                    {
                        s3 = entity.ManOrgLicenseRequest.RegisterNumber.Split('-')[1].PadRight(8, '0');
                    }
                    else
                    {
                        s3 = entity.ManOrgLicenseRequest.RegisterNumber.Replace("-", "").PadRight(8, '0');
                    }
                }
                else
                {
                    s3 = entity.ManOrgLicenseRequest.RegisterNumber.PadRight(8, '0');
                }
                s3 = s3.Replace('/', '1');
                s3 = s3.Replace('\\', '0');
                s3 = s3.Replace('№', '4');
                char[] charsS3 = s3.ToCharArray();
                for (int i = 0; i < s3.Length; i++)
                {
                    if (!char.IsDigit(charsS3[i]))
                    {
                        s3 = s3.Replace(charsS3[i], '0');
                    }
                }
                string UIN = (s1 + s2 + s3).Substring(0, 24);
                return UIN + CheckSum(UIN);
            }
            else if ((entity.TypeLicenseRequest == TypeLicenseRequest.Copy || entity.TypeLicenseRequest == TypeLicenseRequest.Reissuance) && entity.LicenseReissuance != null)
            {
                string s1 = Convert.ToInt32(GetOriginatorIdFromConfig(), 16).ToString().PadLeft(8, '0');
                string s2 = (entity.LicenseReissuance?.ReissuanceDate?.ToString("yyyyMMdd") ?? "00000000");
                string s3 = "";
                if (entity.LicenseReissuance.RegisterNumber.Contains("-"))
                {
                    if (entity.LicenseReissuance.RegisterNumber.Split('-').Count() > 2)
                    {
                        s3 = (entity.LicenseReissuance.RegisterNumber.Split('-')[1] + entity.LicenseReissuance.RegisterNumber.Split('-')[2]).PadRight(8, '0');
                    }
                    else if (entity.LicenseReissuance.RegisterNumber.Split('-').Count() == 2)
                    {
                        s3 = entity.LicenseReissuance.RegisterNumber.Split('-')[1].PadRight(8, '0');
                    }
                    else
                    {
                        s3 = entity.LicenseReissuance.RegisterNumber.Replace("-", "").PadRight(8, '0');
                    }
                }
                else
                {
                    s3 = entity.LicenseReissuance.RegisterNumber.PadRight(8, '0');
                }
                s3 = s3.Replace('/', '1');
                s3 = s3.Replace('\\', '0');
                s3 = s3.Replace('№', '4');
                char[] charsS3 = s3.ToCharArray();
                for (int i = 0; i < s3.Length; i++)
                {
                    if (!char.IsDigit(charsS3[i]))
                    {
                        s3 = s3.Replace(charsS3[i], '0');
                    }
                }
                string UIN = (s1 + s2 + s3).Substring(0, 24);
                return UIN + CheckSum(UIN);
            }
            else
            {
                return "";
            }
        }

        private String CreateAltPayerIdentifier(GisGmp entity)
        {
            switch (entity.PayerType)
            {
                case PayerType.IP:
                    {
                        //При формировании идентификатора плательщика для ИП:
                        //4 – 10 разряды символ «0» (ноль);
                        //11 — 22 разряды — ИНН ИП(12 символов).
                        if (entity.INN.Length != 12)
                            throw new ApplicationException($"Длина ИНН ИП должна быть 12, а пришла {entity.INN.Length}");

                        return "400" + entity.INN.PadLeft(19, '0');
                    }
                case PayerType.Juridical:
                    {
                        if (entity.IsRF)
                        {
                            //При формировании идентификатора плательщика для ЮЛ резидентов РФ:
                            //­4 — 13 разряды — ИНН ЮЛ (10 цифр);
                            //14 — 22 разряды — КПП ЮЛ(9 символов)
                            if (entity.INN.Length != 10)
                                throw new ApplicationException($"Длина ИНН юр.лица должна быть 10, а пришла {entity.INN.Length}");

                            if (entity.KPP.Length != 9)
                                throw new ApplicationException($"Длина КПП юр.лица должна быть 9, а пришла {entity.KPP.Length}");

                            return "200" + entity.INN + entity.KPP;
                        }
                        else if (entity.IdentifierType == IdentifierType.INN)
                        {
                            //При формировании идентификатора плательщика для ЮЛ нерезидентов РФ(при наличии ИНН) следующие:
                            //4 — 13 разряды — ИНН ЮЛ(10 цифр); 
                            //14 — 22 разряды — КПП ЮЛ(9 символов);
                            if (entity.INN.Length != 10)
                                throw new ApplicationException($"Длина ИНН юр.лица должна быть 10, а пришла {entity.INN.Length}");

                            if (entity.KPP.Length != 9)
                                throw new ApplicationException($"Длина КПП юр.лица должна быть 9, а пришла {entity.KPP.Length}");

                            return "300" + entity.INN + entity.KPP;
                        }
                        else
                        {
                            //При формировании идентификатора плательщика для ЮЛ нерезидентов РФ(при наличии КИО) следующие:
                            //4 – 8 разряды – символ «0» (ноль);
                            //9 — 13 разряды — КИО ЮЛ(5 цифр); 
                            //14 — 22 разряды — КПП ЮЛ(9 символов).
                            if (entity.KIO.Length != 5)
                                throw new ApplicationException($"Длина КИО юр.лица должна быть 5, а пришла {entity.KIO.Length}");

                            if (entity.KPP.Length != 9)
                                throw new ApplicationException($"Длина КПП юр.лица должна быть 9, а пришла {entity.KPP.Length}");

                            return "30000000" + entity.KIO + entity.KPP;
                        }
                    }

                case PayerType.Physical:
                    {
                        //При формировании идентификатора плательщика для ФЛ:
                        //С 4 - го по 22 - й символы — серия и номер документа, код которого указан со 2 - го по 3 - й разряд.
                        //Серия и номер документа указываются в одну строку, без разделителей; знаки «N» и «-» не указываются; при наличии букв, они должны указываться как заглавные.
                        //Если номер документа содержит менее 19 символов, он дополняется слева нулями до 19 символов.

                        string documentCode = entity.PhysicalPersonDocType.Code.PadLeft(2, '0');
                        string documentNumber = entity.DocumentNumber.ToUpper().Replace(" ", "").Replace("-", "");
                        string documentSerial = entity.DocumentSerial.ToUpper().Replace(" ", "").Replace("-", "");
                        string document = (documentSerial + documentNumber).PadLeft(19, '0');

                        return "1" + documentCode + document;
                    }
                default:
                    throw new ApplicationException($"Для типа {entity.PayerType} не определен алгоритм формирования идентификатора плательщика");
            }
        }

        private Int32 CheckSum(String number)
        {
            Int32 result = CheckSum(number, 1);

            return result != 10 ? result : CheckSum(number, 3) % 10;
        }

        private Int32 CheckSum(String number, Int32 ves)
        {
            int sum = 0;
            for (int i = 0; i < number.Length; i++)
            {
                int t = (int)Char.GetNumericValue(number[i]);
                int rrr = ((ves % 10) == 0 ? 10 : ves % 10);

                sum += t * rrr;
                ves++;
            }

            return sum % 11;
        }
    }
}
