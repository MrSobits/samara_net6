namespace Bars.GisIntegration.Base.Tasks.PrepareData.Inspection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.InspectionAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Castle.Core.Internal;

    using Castle.Core.Internal;

    /// <summary>
    /// Класс задачи подготовки сведений о выполняющихся и проведенных проверках
    /// </summary>
    public class ExaminationPrepareDataTask : BasePrepareDataTask<importExaminationsRequest>
    {
        /// <summary>
        /// Размер порции
        /// </summary>
        private const int Portion = 1000;

        private List<Examination> examinations;
        private List<ExaminationPlace> examinationPlaces;
        private List<ExaminationOtherDocument> examinationOtherDocuments;
        private List<Precept> precepts;
        private List<PreceptAttachment> preceptAttachments;
        private List<CancelPreceptAttachment> cancelPreceptAttachments;
        private List<Offence> offences;
        private List<OffenceAttachment> offenceAttachments;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var examinationsExtractor = this.Container.Resolve<IDataExtractor<Examination>>("ExaminationExtractor");
            var examinationPlaceExtractor = this.Container.Resolve<IDataExtractor<ExaminationPlace>>("ExaminationPlaceExtractor");
            var otherActDocumentExtractor = this.Container.Resolve<IDataExtractor<ExaminationOtherDocument>>("OtherActDocumentExtractor");
            var preceptExtractor = this.Container.Resolve<IDataExtractor<Precept>>("PreceptExtractor");
            var preceptAttachmentExtractor = this.Container.Resolve<IDataExtractor<PreceptAttachment>>("PreceptAttachmentExtractor");
            var cancelPreceptAttachmentExtractor = this.Container.Resolve<IDataExtractor<CancelPreceptAttachment>>("CancelPreceptAttachmentExtractor");
            var offenceExtractor = this.Container.Resolve<IDataExtractor<Offence>>("OffenceExtractor");
            var offenceAttachmentExtractor = this.Container.Resolve<IDataExtractor<OffenceAttachment>>("OffenceAttachmentExtractor");

            try
            {
                this.examinations = this.RunExtractor(examinationsExtractor, parameters);
                parameters.Add("Examinations", this.examinations);

                this.examinationPlaces = this.RunExtractor(examinationPlaceExtractor, parameters);

                this.examinationOtherDocuments = this.RunExtractor(otherActDocumentExtractor, parameters);

                this.precepts = this.RunExtractor(preceptExtractor, parameters);
                parameters.Add("Precepts", this.precepts);

                this.preceptAttachments = this.RunExtractor(preceptAttachmentExtractor, parameters);
                this.cancelPreceptAttachments = this.RunExtractor(cancelPreceptAttachmentExtractor, parameters);

                this.offences = this.RunExtractor(offenceExtractor, parameters);
                parameters.Add("Offences", this.offences);

                this.offenceAttachments = this.RunExtractor(offenceAttachmentExtractor, parameters);
            }
            finally
            {
                this.Container.Release(examinationsExtractor);
                this.Container.Release(examinationPlaceExtractor);
                this.Container.Release(otherActDocumentExtractor);
                this.Container.Release(preceptExtractor);
                this.Container.Release(preceptAttachmentExtractor);
                this.Container.Release(cancelPreceptAttachmentExtractor);
                this.Container.Release(offenceExtractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(this.ValidateObjectList(this.examinations, this.ValidateExamination));
            result.AddRange(this.ValidateObjectList(this.examinationPlaces, this.ValidateExaminationPlace));
            result.AddRange(this.ValidateObjectList(this.examinationOtherDocuments, this.ValidateExaminationOtherDocument));
            result.AddRange(this.ValidateObjectList(this.precepts, this.ValidatePrecept));
            result.AddRange(this.ValidateObjectList(this.cancelPreceptAttachments, this.ValidateCancelPreceptAttachment));
            result.AddRange(this.ValidateObjectList(this.preceptAttachments, this.ValidatePreceptAttachment));
            result.AddRange(this.ValidateObjectList(this.offences, this.ValidateOffence));
            result.AddRange(this.ValidateObjectList(this.offenceAttachments, this.ValidateOffenceAttachment));


            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь транспортных идентификаторов</returns>
        protected override Dictionary<importExaminationsRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importExaminationsRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();

                var request = this.GetRequestObject(iterationList, transportGuidDictionary);
                request.Id = Guid.NewGuid().ToString();

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<Examination>> GetPortions()
        {
            var result = new List<IEnumerable<Examination>>();

            if (this.examinations.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.examinations.Skip(startIndex).Take(ExaminationPrepareDataTask.Portion));
                    startIndex += ExaminationPrepareDataTask.Portion;
                }
                while (startIndex < this.examinations.Count);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект запроса</returns>
        private importExaminationsRequest GetRequestObject(IEnumerable<Examination> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var examinationList = new List<importExaminationsRequestImportExamination>();
            var examinationGuidDictionary = new Dictionary<string, long>();
            var preceptGuidDictionary = new Dictionary<string, long>();
            var offenceGuidDictionary = new Dictionary<string, long>();

            foreach (var examination in listForImport)
            {
                var examinationItem = this.GetExaminationItem(examination, preceptGuidDictionary, offenceGuidDictionary);


                examinationList.Add(examinationItem);
                examinationGuidDictionary.Add(examinationItem.TransportGUID, examination.Id);
            }

            transportGuidDictionary.Add(typeof(Examination), examinationGuidDictionary);
            transportGuidDictionary.Add(typeof(Precept), preceptGuidDictionary);
            transportGuidDictionary.Add(typeof(Offence), offenceGuidDictionary);

            return new importExaminationsRequest { ImportExamination = examinationList.ToArray() };
        }

        /// <summary>
        /// Получить элемент ImportExamination
        /// </summary>
        /// <param name="examination">Проверка</param>
        /// <param name="preceptGuidDictionary">Словарь транспортных идентификаторов предписаний</param>
        /// <param name="offenceGuidDictionary">Словарь транспортных идентификаторов протоколов</param>
        /// <returns></returns>
        private importExaminationsRequestImportExamination GetExaminationItem(Examination examination, Dictionary<string, long> preceptGuidDictionary, Dictionary<string, long> offenceGuidDictionary)
        {
            var resultsInfo = this.GetResultsInfo(examination);

            var result = new importExaminationsRequestImportExamination
            {
                TransportGUID = Guid.NewGuid().ToString(),
                Examination = new ExaminationType
                {
                    ExaminationOverview = new ExaminationTypeExaminationOverview
                    {
                        ExaminationTypeType = new ExaminationTypeExaminationOverviewExaminationTypeType
                        {
                            Item = this.GetExaminationTypeTypeItem(examination)
                        },
                        OversightActivitiesRef = new nsiRef
                        {
                            Code = examination.OversightActivitiesCode,
                            GUID = examination.OversightActivitiesGuid
                        },
                        ExaminationForm = new nsiRef
                        {
                            Code = examination.ExaminationFormCode,
                            GUID = examination.ExaminationFormGuid
                        },
                        OrderNumber = examination.OrderNumber,
                        OrderDate = examination.OrderDate.GetValueOrDefault(),
                        OrderDateSpecified = examination.OrderDate.HasValue
                    },
                    RegulatoryAuthorityInformation = new ExaminationTypeRegulatoryAuthorityInformation
                    {
                        FunctionRegistryNumber = examination.FunctionRegistryNumber,
                        AuthorizedPersons = examination.AuthorizedPersons,
                        InvolvedExperts = examination.InvolvedExperts
                    },
                    NotificationInfo = new ExaminationTypeNotificationInfo
                    {
                        Items = new object[] { true },
                        ItemsElementName = new[] { ItemsChoiceType4.RequiredAndNotSent }
                    },
                    ExaminationInfo = new ExaminationTypeExaminationInfo
                    {
                        Base = examination.BaseCode.IsNotEmpty() && examination.BaseGuid.IsNotEmpty() ? new nsiRef
                        {
                            Code = examination.BaseCode,
                            GUID = examination.BaseGuid
                        } : null,
                        BasedOnPrecept = !examination.PreceptGuid.IsEmpty() ? new ExaminationTypeExaminationInfoBasedOnPrecept
                        {
                            PreceptGuid = examination.PreceptGuid
                        } : null,
                        Objective = examination.Objective,
                        Tasks = examination.Tasks,
                        Object = new[]
                        {
                            new nsiRef
                            {
                                Code = examination.ObjectCode,
                                GUID = examination.ObjectGuid
                            }
                        },
                        From = examination.From.GetValueOrDefault(),
                        To = examination.To.GetValueOrDefault(),
                        Duration = new ExaminationTypeExaminationInfoDuration
                        {
                            Item = examination.Duration.GetValueOrDefault(),
                            ItemElementName = ItemChoiceType2.WorkDays
                        },
                        ProsecutorAgreementInformation = examination.ProsecutorAgreementInformation
                    },
                    ExecutingInfo = new ExaminationTypeExecutingInfo
                    {
                        Event = new[]
                        {
                            new ExaminationEventType
                            {
                                Number = "1",
                                Description = examination.EventDescription
                            }
                        },
                        Place = this.GetExaminationPlaces(examination)
                    },
                    ResultsInfo = resultsInfo
                },
                AnnulExaminationSpecified = false,
                ImportOffence = this.GetImportOffence(examination, offenceGuidDictionary),
                ImportPrecept = this.GetImporPrecept(examination, preceptGuidDictionary)
            };

            if (examination.Operation == RisEntityOperation.Update)
            {
                result.ExaminationGuid = examination.Guid;
            }

            return result;
        }

        /// <summary>
        /// Получить блок ImportPrecept
        /// </summary>
        /// <param name="examination">Проверка</param>
        /// <param name="preceptGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Блок ImportPrecept</returns>
        private importExaminationsRequestImportExaminationImportPrecept[] GetImporPrecept(Examination examination, Dictionary<string, long> preceptGuidDictionary)
        {
            var result = new List<importExaminationsRequestImportExaminationImportPrecept>();

            foreach (var precept in this.precepts.Where(x => x.Examination == examination))
            {
                object item = true;
                ItemChoiceType3 itemName = ItemChoiceType3.DeletePrecept;

                if (precept.Operation == RisEntityOperation.Update && precept.IsCancelledAndIsFulfiled)
                {
                    itemName = ItemChoiceType3.IsFulfiledPrecept;
                }
                else if (precept.Operation == RisEntityOperation.Update && precept.IsCancelled)
                {
                    itemName = ItemChoiceType3.CancelPrecept;
                    item = new CancelledInfoWithAttachmentsType
                    {
                        Reason = new nsiRef
                        {
                            Code = precept.CancelReason,
                            GUID = precept.CancelReasonGuid
                        },
                        Date = precept.CancelDate.GetValueOrDefault(),
                        Organisation = new RegOrgType
                        {
                            orgRootEntityGUID = precept.OrgRootEntityGuid
                        },
                        Attachments = this.GetAttachments(this.cancelPreceptAttachments.Where(x => x.Precept == precept).Select(x => x.Attachment))
                    };
                }
                else if (precept.Operation != RisEntityOperation.Delete) // прочее редактирование и создание
                {
                    itemName = ItemChoiceType3.Precept;
                    item = new PreceptType
                    {
                        Number = precept.Number,
                        Date = precept.Date.GetValueOrDefault(),
                        Deadline = precept.Deadline.GetValueOrDefault(),
                        FIASHouseGUID = new[] { precept.FiasHouseGuid },
                        IsFulfiledPrecept = precept.IsFulfiledPrecept.GetValueOrDefault(),
                        IsFulfiledPreceptSpecified = precept.IsFulfiledPrecept.HasValue,
                        Attachment = this.GetAttachments(this.preceptAttachments.Where(x => x.Precept == precept).Select(x => x.Attachment))
                    };
                }

                var preceptItem = new importExaminationsRequestImportExaminationImportPrecept
                {
                    TransportGUID = Guid.NewGuid().ToString(),
                    Item = item,
                    ItemElementName = itemName
                };

                if (precept.Operation == RisEntityOperation.Update)
                {
                    preceptItem.PreceptGuid = precept.Guid;
                }

                preceptGuidDictionary.Add(preceptItem.TransportGUID, precept.Id);
                result.Add(preceptItem);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить вложения для запроса
        /// </summary>
        /// <param name="attachments">Список вложений</param>
        /// <returns>Вложения для запроса</returns>
        private AttachmentType[] GetAttachments(IEnumerable<Base.Entities.Attachment> attachments)
        {
            var result = new List<AttachmentType>();

            foreach (var attachment in attachments)
            {
                result.Add(new AttachmentType
                {
                    Name = attachment?.Name,
                    Description = attachment?.Description,
                    AttachmentHASH = attachment?.Hash,
                    Attachment = new Attachment
                    {
                        AttachmentGUID = attachment?.Guid
                    }
                });
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить блок ImportOffence
        /// </summary>
        /// <param name="examination">Проверка</param>
        /// <param name="offenceGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Блок ImportOffence</returns>
        private importExaminationsRequestImportExaminationImportOffence[] GetImportOffence(Examination examination, Dictionary<string, long> offenceGuidDictionary)
        {
            var result = new List<importExaminationsRequestImportExaminationImportOffence>();

            foreach (var offence in this.offences.Where(x => x.Examination == examination))
            {
                object item = true;

                if (offence.Operation != RisEntityOperation.Delete)
                {
                    item = new OffenceType
                    {
                        Number = offence.Number,
                        Date = offence.Date.GetValueOrDefault(),
                        IsFulfiledOffence = offence.IsFulfiledOffence.GetValueOrDefault(),
                        IsFulfiledOffenceSpecified = offence.IsFulfiledOffence.HasValue,
                        Attachment = this.GetAttachments(this.offenceAttachments.Where(x => x.Offence == offence).Select(x => x.Attachment))
                    };
                }

                var offenceItem = new importExaminationsRequestImportExaminationImportOffence
                {
                    TransportGUID = Guid.NewGuid().ToString(),
                    ItemElementName = offence.Operation == RisEntityOperation.Delete ? ItemChoiceType4.DeleteOffence : ItemChoiceType4.Offence,
                    Item = item
                };

                if (offence.Operation == RisEntityOperation.Update)
                {
                    offenceItem.OffenceGuid = offence.Guid;
                }

                offenceGuidDictionary.Add(offenceItem.TransportGUID, offence.Id);
                result.Add(offenceItem);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить раздел ResultsInfo
        /// </summary>
        /// <param name="examination">Проверка</param>
        /// <returns>Раздел ResultsInfo</returns>
        private ResultsInfoType GetResultsInfo(Examination examination)
        {
            var items = new List<object> { true };

            if (examination.HasOffence)
            {
                items.Add(new ExaminationResultTypeIdentifiedOffencesInfo
                {
                    IdentifiedOffences = examination.IdentifiedOffences
                });
            }

            return new ResultsInfoType
            {
                FinishedInfo = new ResultsInfoTypeFinishedInfo
                {
                    Result = new ExaminationResultType
                    {
                        DocumentType = new nsiRef
                        {
                            Code = examination.ResultDocumentTypeCode,
                            GUID = examination.ResultDocumentTypeGuid
                        },
                        Number = examination.ResultDocumentNumber,
                        Date = examination.ResultDocumentDateTime.GetValueOrDefault(),
                        Items = items.ToArray(),
                        ItemsElementName = examination.HasOffence ? new[] { ItemsChoiceType5.HasOffence, ItemsChoiceType5.IdentifiedOffencesInfo, } : new[] { ItemsChoiceType5.HasNoOffence },
                        From = examination.ResultFrom.GetValueOrDefault(),
                        To = examination.ResultTo.GetValueOrDefault(),
                        Place = examination.ResultPlace,
                        Duration = examination.Duration.ToString(),
                        RepresentativesRegionPersons = examination?.FamiliarizedPerson,
                        InspectionPersons = examination.InvolvedExperts,
                        SettlingDocumentPlace = examination.ResultPlace
                    },
                    OtherDocument = this.GetAttachments(this.examinationOtherDocuments.Where(x => x.Examination == examination).Select(x => x.Attachment))
                },
                FamiliarizationInfo = new ResultsInfoTypeFamiliarizationInfo
                {
                    ItemsElementName = new[] { ItemsChoiceType6.FamiliarizationDate, ItemsChoiceType6.IsSigned, ItemsChoiceType6.FamiliarizedPerson },
                    Items = new object[] { examination.FamiliarizationDate, examination.IsSigned, examination.FamiliarizedPerson }
                }
            };
        }

        /// <summary>
        /// Получить раздел Place
        /// </summary>
        /// <param name="examination">Проверка</param>
        /// <returns>Раздел Place</returns>
        private ExaminationPlaceType[] GetExaminationPlaces(Examination examination)
        {
            var result = new List<ExaminationPlaceType>();

            foreach (var place in this.examinationPlaces.Where(x => x.Examination == examination))
            {
                result.Add(new ExaminationPlaceType
                {
                    OrderNumber = place.OrderNumber,
                    FIASHouseGuid = place.FiasHouseGuid
                });
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить Item раздела ExaminationTypeType
        /// </summary>
        /// <param name="examination">Проверка</param>
        /// <returns>Item раздела ExaminationTypeType</returns>
        private object GetExaminationTypeTypeItem(Examination examination)
        {
            object result;

            if (examination.IsScheduled)
            {
                RegOrgType item = null;

                if (examination.SubjectType == ExaminationSubjectType.Individual)
                {
                    item = new ScheduledExaminationSubjectInfoTypeIndividual
                    {
                        orgRootEntityGUID = examination.GisContragent?.OrgRootEntityGuid,
                        ActualActivityPlace = examination.GisContragent?.FactAddress
                    };
                }
                else if (examination.SubjectType == ExaminationSubjectType.Organization)
                {
                    item = new ScheduledExaminationSubjectInfoTypeOrganization
                    {
                        orgRootEntityGUID = examination.GisContragent?.OrgRootEntityGuid,
                        ActualActivityPlace = examination.GisContragent?.FactAddress
                    };
                }

                result = new ExaminationTypeExaminationOverviewExaminationTypeTypeScheduled
                {
                    Subject = new ScheduledExaminationSubjectInfoType
                    {
                        Item = item
                    }
                };
            }
            else
            {
                object item;

                if (examination.SubjectType == ExaminationSubjectType.Individual)
                {
                    item = new UnscheduledExaminationSubjectInfoTypeIndividual
                    {
                        orgRootEntityGUID = examination.GisContragent?.OrgRootEntityGuid,
                        ActualActivityPlace = examination.GisContragent?.FactAddress
                    };
                }
                else if (examination.SubjectType == ExaminationSubjectType.Organization)
                {
                    item = new UnscheduledExaminationSubjectInfoTypeOrganization
                    {
                        orgRootEntityGUID = examination.GisContragent?.OrgRootEntityGuid,
                        ActualActivityPlace = examination.GisContragent?.FactAddress
                    };
                }
                else // ExaminationSubjectType.Citizen
                {
                    item = new CitizenInfoType
                    {
                        LastName = examination.LastName,
                        FirstName = examination.FirstName
                    };
                }

                result = new ExaminationTypeExaminationOverviewExaminationTypeTypeUnscheduled
                {
                    Subject = new UnscheduledExaminationSubjectInfoType
                    {
                        Item = item
                    }
                };
            }

            return result;
        }

        /// <summary>
        /// Проверить объект проверки
        /// </summary>
        /// <param name="examination">Объект проверки</param>
        /// <returns>Результат проверки</returns>
        private ValidateObjectResult ValidateExamination(Examination examination)
        {
            StringBuilder messages = new StringBuilder();

            if (examination.SubjectType != ExaminationSubjectType.Citizen &&
                examination.GisContragent == null)
            {
                messages.Append("orgRootEntityGUID ActualActivityPlace ");
            }
            else if (examination.SubjectType == ExaminationSubjectType.Citizen)
            {
                if (examination.FirstName.IsEmpty())
                {
                    messages.Append("FirstName ");
                }

                if (examination.LastName.IsEmpty())
                {
                    messages.Append("LastName ");
                }
            }

            if (examination.OversightActivitiesCode.IsEmpty() ||
                examination.OversightActivitiesGuid.IsEmpty())
            {
                messages.Append("OversightActivitiesRef ");
            }

            if (examination.ExaminationFormCode.IsEmpty() ||
                examination.ExaminationFormGuid.IsEmpty())
            {
                messages.Append("ExaminationForm ");
            }

            if (examination.Objective.IsEmpty())
            {
                messages.Append("Цель проведения проверки с реквизитами документов основания ");
            }

            if (examination.Tasks.IsEmpty())
            {
                messages.Append("Tasks ");
            }

            if (examination.ObjectCode.IsEmpty() ||
                examination.ObjectGuid.IsEmpty())
            {
                messages.Append("НСИ 'Предмет проверки' (реестровый номер 69) ");
            }

            if (!examination.From.HasValue)
            {
                messages.Append("From ");
            }

            if (!examination.To.HasValue)
            {
                messages.Append("To ");
            }

            if (!examination.Duration.HasValue || examination.Duration <= 0d)
            {
                messages.Append("WorkDays ");
            }

            if (examination.ResultDocumentTypeCode.IsEmpty() ||
                examination.ResultDocumentTypeCode.IsEmpty())
            {
                messages.Append("Result/DocumentType ");
            }

            if (examination.ResultDocumentNumber.IsEmpty())
            {
                messages.Append("Result/Number ");
            }

            if (!examination.ResultDocumentDateTime.HasValue)
            {
                messages.Append("Result/Date ");
            }

            if (this.examinationOtherDocuments.All(x => x.Examination != examination))
            {
                messages.Append("OtherDocument ");
            }

            if (!examination.FamiliarizationDate.HasValue)
            {
                messages.Append("FamiliarizationInfo/FamiliarizationDate ");
            }

            if (examination.FamiliarizedPerson.IsEmpty())
            {
                messages.Append("FamiliarizationInfo/FamiliarizedPerson ");
            }

            if (examination.FunctionRegistryNumber.IsNullOrEmpty())
            {
                messages.Append("FunctionRegistryNumber ");
            }

            if (examination.ResultPlace.IsNullOrEmpty())
            {
                messages.Append("ResultPlace ");
            }

            return new ValidateObjectResult
            {
                Id = examination.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Проверка"
            };
        }

        /// <summary>
        /// Проверить место проведения проверки
        /// </summary>
        /// <param name="place">Место проведения проверки</param>
        /// <returns>Результат проверки</returns>
        private ValidateObjectResult ValidateExaminationPlace(ExaminationPlace place)
        {
            StringBuilder messages = new StringBuilder();

            if (place.FiasHouseGuid.IsEmpty())
            {
                messages.Append("FiasHouseGuid");
            }

            return new ValidateObjectResult
            {
                Id = place.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Место проведения проверки"
            };
        }

        /// <summary>
        /// Проверить другой документ проверки
        /// </summary>
        /// <param name="document">Другой документ проверки</param>
        /// <returns>Результат проверки</returns>
        private ValidateObjectResult ValidateExaminationOtherDocument(ExaminationOtherDocument document)
        {
            StringBuilder messages = new StringBuilder();

            if (document.Attachment == null)
            {
                messages.Append("Attachment ");
            }
            else
            {
                if (document.Attachment.Name.IsEmpty())
                {
                    messages.Append("Name ");
                }

                if (document.Attachment.Description.IsEmpty())
                {
                    messages.Append("Description ");
                }
            }

            return new ValidateObjectResult
            {
                Id = document.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Другой документ проверки"
            };
        }

        /// <summary>
        /// Проверить предписание
        /// </summary>
        /// <param name="precept">Предписание</param>
        /// <returns>Результат проверки</returns>
        private ValidateObjectResult ValidatePrecept(Precept precept)
        {
            StringBuilder messages = new StringBuilder();

            if (precept.Operation == RisEntityOperation.Delete || precept.IsCancelledAndIsFulfiled)
            {
                // пока нечего проверять
            }
            else if (precept.IsCancelled)
            {
                if (precept.CancelReason.IsEmpty() ||
                    precept.CancelReasonGuid.IsEmpty())
                {
                    messages.Append("CancelReason ");
                }

                if (!precept.CancelDate.HasValue)
                {
                    messages.Append("CancelDate ");
                }

                if (precept.OrgRootEntityGuid.IsEmpty())
                {
                    messages.Append("OrgRootEntityGuid ");
                }
            }
            else
            {
                if (precept.Number.IsEmpty())
                {
                    messages.Append("Number ");
                }

                if (!precept.Date.HasValue)
                {
                    messages.Append("Date ");
                }

                if (!precept.Deadline.HasValue)
                {
                    messages.Append("Deadline ");
                }

                if (this.preceptAttachments.All(x => x.Precept != precept))
                {
                    messages.Append("Attachment ");
                }
            }

            return new ValidateObjectResult
            {
                Id = precept.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Предписание"
            };
        }

        /// <summary>
        /// Проверить вложение отмены предписания
        /// </summary>
        /// <param name="cancelPreceptAttachment">Вложение отмены предписания</param>
        /// <returns>Результат проверки</returns>
        private ValidateObjectResult ValidateCancelPreceptAttachment(CancelPreceptAttachment cancelPreceptAttachment)
        {
            StringBuilder messages = new StringBuilder();

            if (cancelPreceptAttachment.Attachment == null)
            {
                messages.Append("Attachment ");
            }
            else
            {
                if (cancelPreceptAttachment.Attachment.Name.IsEmpty())
                {
                    messages.Append("Name ");
                }

                if (cancelPreceptAttachment.Attachment.Description.IsEmpty())
                {
                    messages.Append("Description ");
                }
            }

            return new ValidateObjectResult
            {
                Id = cancelPreceptAttachment.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Вложение отмены предписания"
            };
        }

        /// <summary>
        /// Проверить вложение предписания
        /// </summary>
        /// <param name="preceptAttachment">Вложение предписания</param>
        /// <returns>Результат проверки</returns>
        private ValidateObjectResult ValidatePreceptAttachment(PreceptAttachment preceptAttachment)
        {
            StringBuilder messages = new StringBuilder();

            if (preceptAttachment.Attachment == null)
            {
                messages.Append("Attachment ");
            }
            else
            {
                if (preceptAttachment.Attachment.Name.IsEmpty())
                {
                    messages.Append("Name ");
                }

                if (preceptAttachment.Attachment.Description.IsEmpty())
                {
                    messages.Append("Description ");
                }
            }

            return new ValidateObjectResult
            {
                Id = preceptAttachment.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Вложение предписания"
            };
        }

        /// <summary>
        /// Проверить протокол
        /// </summary>
        /// <param name="offence">Протокол</param>
        /// <returns>Результат проверки</returns>
        private ValidateObjectResult ValidateOffence(Offence offence)
        {
            StringBuilder messages = new StringBuilder();

            if (offence.Operation != RisEntityOperation.Delete)
            {
                if (offence.Number.IsEmpty())
                {
                    messages.Append("Number ");
                }

                if (!offence.Date.HasValue)
                {
                    messages.Append("Date ");
                }

                if (this.offenceAttachments.All(x => x.Offence != offence))
                {
                    messages.Append("Attachment ");
                }
            }

            return new ValidateObjectResult
            {
                Id = offence.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Протокол"
            };
        }

        /// <summary>
        /// Проверить вложение протокола
        /// </summary>
        /// <param name="offenceAttachment">Вложение протокола</param>
        /// <returns>Результат проверки</returns>
        private ValidateObjectResult ValidateOffenceAttachment(OffenceAttachment offenceAttachment)
        {
            StringBuilder messages = new StringBuilder();

            if (offenceAttachment.Attachment == null)
            {
                messages.Append("Attachment ");
            }
            else
            {
                if (offenceAttachment.Attachment.Name.IsEmpty())
                {
                    messages.Append("Name ");
                }

                if (offenceAttachment.Attachment.Description.IsEmpty())
                {
                    messages.Append("Description ");
                }
            }

            return new ValidateObjectResult
            {
                Id = offenceAttachment.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Вложение протокола"
            };
        }
    }
}