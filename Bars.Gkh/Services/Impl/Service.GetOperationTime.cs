namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.GetOperationTime;

    public partial class Service
    {
        public GetOperationTimeResponse GetOperationTime(string moId)
        {
            int id;
            OperationTime operationTime = null;
            if (int.TryParse(moId, out id))
            {
                var operationTimes =
                    Container.Resolve<IDomainService<ManagingOrgWorkMode>>()
                             .GetAll()
                             .Where(x => x.ManagingOrganization.Id == id)
                             .ToList();
                if (operationTimes.Count > 0)
                {
                    operationTime = new OperationTime
                                        {
                                            ModeOfOrganizations =
                                                this.GetWorkingHours(operationTimes, TypeMode.WorkMode),
                                            CitizenRecHours =
                                                this.GetWorkingHours(
                                                    operationTimes, TypeMode.ReceptionCitizens),
                                            DispatcherTimes =
                                                this.GetWorkingHours(
                                                    operationTimes, TypeMode.DispatcherWork)
                                        };
                }
            }

            var result = operationTime == null ? Result.DataNotFound : Result.NoErrors;
            return new GetOperationTimeResponse { OperationTime = operationTime, Result = result };
        }

        private WorkingHours[] GetWorkingHours(IEnumerable<ManagingOrgWorkMode> workMode, TypeMode type)
        {
            var res =
                workMode.Where(x => x.TypeMode == type)
                        .OrderBy(x => x.TypeDayOfWeek)
                        .Select(
                            x =>
                            new WorkingHours
                                {
                                    Break = string.IsNullOrEmpty(x.Pause) ? null : x.Pause,
                                    OfficeHours = GetHours(x.StartDate, x.EndDate, x.AroundClock),
                                    Day = GetDayOfWeek(x.TypeDayOfWeek)
                                })
                        .ToArray();
            return res.Length > 0 ? res : null;
        }

        private string GetHours(DateTime? start, DateTime? end, bool aroundClock)
        {
            if (aroundClock)
            {
                return "0:00-24:00";
            }

            if (start.HasValue && end.HasValue)
            {
                return start.Value.ToString("HH:mm") + "-" + end.Value.ToString("HH:mm");
            }

            return null;
        }

        private string GetDayOfWeek(TypeDayOfWeek day)
        {
            switch (day)
            {
                case TypeDayOfWeek.Monday:
                    return "Понедельник";
                case TypeDayOfWeek.Tuesday:
                    return "Вторник";
                case TypeDayOfWeek.Wednesday:
                    return "Среда";
                case TypeDayOfWeek.Thursday:
                    return "Четверг";
                case TypeDayOfWeek.Friday:
                    return "Пятница";
                case TypeDayOfWeek.Saturday:
                    return "Суббота";
                case TypeDayOfWeek.Sunday:
                    return "Воскресенье";
            }

            return null;
        }
    }
}