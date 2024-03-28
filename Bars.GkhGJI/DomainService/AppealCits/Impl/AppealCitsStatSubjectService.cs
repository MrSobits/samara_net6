namespace Bars.GkhGji.DomainService
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис взаимодействия с тематиками обращений
    /// </summary>
    public class AppealCitsStatSubjectService : IAppealCitsStatSubjectService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc/>
        public IDataResult AddStatementSubject(BaseParams baseParams)
        {
            try
            {
                var appealCitizensId = baseParams.Params.ContainsKey("appealCitizensId") ? baseParams.Params["appealCitizensId"].ToLong() : 0;

                //в objectIds приходит строка вида: "1/2/3,2/3,4/5"
                //идентификаторы, разделенные слешами это: первый - тематика, второй - подтематика, третий характеристика
                var subjIds = baseParams.Params.ContainsKey("objectIds") ? baseParams.Params["objectIds"].ToString() : string.Empty;

                if (appealCitizensId == 0 || string.IsNullOrEmpty(subjIds))
                {
                    return new BaseDataResult{ Success = false, Message = "Не удалось получить обращение и/или тематики обращений" };
                }

                var service = Container.Resolve<IDomainService<AppealCitsStatSubject>>();
                var serviceAppCit = Container.Resolve<IDomainService<AppealCits>>();
                var accCit = serviceAppCit.Get(appealCitizensId);

                var data = 
                    service.GetAll()
                        .Where(x => x.AppealCits.Id == appealCitizensId)
                        .Select(x => new
                        {
                            SubjectId = x.Subject.Id,
                            SubsubjectId = (long?)x.Subsubject.Id,
                            FeatureId = (long?)x.Feature.Id
                        })
                        .ToList();

                var existsRecords = data
                    .Select(x => new ProxyClass
                    {
                        SubjectId = x.SubjectId,
                        SubsubjectId = x.SubsubjectId,
                        FeatureId = x.FeatureId
                    })
                    .ToList();


                foreach (var subjId in subjIds.Split(','))
                {
                    var ids = subjId.Split('/').Select(x => x.ToLong()).ToArray();

                    AppealCitsStatSubject newRec = null;

                    switch (ids.Count())
                    {
                        //только тематика
                        case 1:
                            if (!existsRecords.Any(x => x.SubjectId == ids[0] && !x.SubsubjectId.HasValue && !x.FeatureId.HasValue))
                                newRec = new AppealCitsStatSubject
                                    {
                                        AppealCits = new AppealCits { Id = appealCitizensId },
                                        Subject = new StatSubjectGji { Id = ids[0] }
                                    };
                            break;
                        //тематика и подтематика
                        case 2:
                            if (!existsRecords.Any(x => x.SubjectId == ids[0] && x.SubsubjectId == ids[1] && !x.FeatureId.HasValue))
                                newRec = new AppealCitsStatSubject
                                    {
                                        AppealCits = new AppealCits { Id = appealCitizensId },
                                        Subject = new StatSubjectGji { Id = ids[0] },
                                        Subsubject = new StatSubsubjectGji { Id = ids[1] }
                                    };
                            break;
                        //тематика, подтематика, характеристика
                        case 3:
                            if (!existsRecords.Any(x => x.SubjectId == ids[0] && x.SubsubjectId == ids[1] && x.FeatureId == ids[2]))
                                newRec = new AppealCitsStatSubject
                                    {
                                        AppealCits = new AppealCits { Id = appealCitizensId },
                                        Subject = new StatSubjectGji { Id = ids[0] },
                                        Subsubject = new StatSubsubjectGji { Id = ids[1] },
                                        Feature = new  FeatureViolGji { Id = ids[2] }
                                    };
                            break;
                    }

                    if (newRec != null)
                    {
                        service.Save(newRec);
                        if (string.IsNullOrEmpty(accCit.StatementSubjects))
                        {
                            accCit.StatementSubjects = newRec.Subject.Name;
                        }
                        else
                        {
                            accCit.StatementSubjects = accCit.StatementSubjects + ", " + newRec.Subject.Name; 
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult{Success = false, Message = e.Message};
            }
        }

        private class ProxyClass
        {
            public long? SubjectId;
            public long? SubsubjectId;
            public long? FeatureId;
        }
    }
}