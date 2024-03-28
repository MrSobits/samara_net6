namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public abstract class ActBaseStimulReport : GjiBaseStimulReport
    {
        protected ActBaseStimulReport(IReportTemplate reportTemplate)
            : base(reportTemplate)
        {
        }

        protected void RegWitnessesDataSource(IEnumerable<ActCheckWitness> witnesses,
            string dataSourceName = "ЛицаПрисутствовавшиеПриПроверке")
        {
            this.DataSources.Add(new MetaData
            {
                SourceName = dataSourceName,
                MetaType = nameof(ЛицоПриПроверке),
                Data = witnesses.ToList().Select(x => new ЛицоПриПроверке(x))
            });
        }

        protected void RegAttachmentsDataSource(IEnumerable<ActCheckAnnex> attachments,
            string dataSourceName = "ПрилагаемыеДокументы")
        {
            this.DataSources.Add(new MetaData
            {
                SourceName = dataSourceName,
                MetaType = nameof(ПрилагаемыйДокумент),
                Data = attachments.ToList().Select(x => new ПрилагаемыйДокумент(x))
            });
        }

        protected void RegPeriodDataSource(IEnumerable<ActCheckPeriod> periods,
            string dataSourceName = "ПериодыПроверки")
        {
            this.DataSources.Add(new MetaData
            {
                SourceName = dataSourceName,
                MetaType = nameof(ПериодПроверки),
                Data = periods.ToList().Select(x => new ПериодПроверки(x))
            });
        }

        protected void RegDurationDataSource(TimeSpan duration, string dataSourceName = "ОбщаяПродолжительность")
        {
            this.DataSources.Add(new MetaData
            {
                SourceName = dataSourceName,
                MetaType = nameof(ОбщаяПродолжительность),
                Data = new ОбщаяПродолжительность(duration)
            });
        }
        protected void RegDurationDataSource(IEnumerable<Period> periods, string dataSourceName = "ОбщаяПродолжительность")
        {
            this.DataSources.Add(new MetaData
            {
                SourceName = dataSourceName,
                MetaType = nameof(ОбщаяПродолжительность),
                Data = new ОбщаяПродолжительность(periods)
            });
        }

        protected class Period
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
        }

        protected class ПериодПроверки
        {
            public string ДатаПроверки { get; set; }
            public string ДатаНачало { get; set; }
            public int ЧасНачало { get; set; }
            public int МинНачало { get; set; }
            public string ДатаКонец { get; set; }
            public int ЧасКонец { get; set; }
            public int МинКонец { get; set; }
            public int ЧасПродолжительность { get; set; }
            public int МинПродолжительность { get; set; }

            public ПериодПроверки(ActCheckPeriod actCheckPeriod)
            {
                if (actCheckPeriod.DateCheck.HasValue)
                {
                    ДатаПроверки = actCheckPeriod.DateCheck.Value.ToString("dd.MM.yyyy");
                }

                if (actCheckPeriod.DateStart.HasValue)
                {
                    ДатаПроверки = actCheckPeriod.DateStart.Value.ToString("dd.MM.yyyy");
                    ЧасНачало = actCheckPeriod.DateStart.Value.Hour;
                    МинНачало = actCheckPeriod.DateStart.Value.Minute;
                }

                if (actCheckPeriod.DateEnd.HasValue)
                {
                    ДатаПроверки = actCheckPeriod.DateEnd.Value.ToString("dd.MM.yyyy");
                    ЧасКонец = actCheckPeriod.DateEnd.Value.Hour;
                    МинКонец = actCheckPeriod.DateEnd.Value.Minute;
                }

                if (actCheckPeriod.DateStart.HasValue && actCheckPeriod.DateEnd.HasValue)
                {
                    var duration = actCheckPeriod.DateEnd.Value.TimeOfDay - actCheckPeriod.DateStart.Value.TimeOfDay;
                    ЧасПродолжительность = duration.Hours;
                    МинПродолжительность = duration.Minutes;
                }
            }
        }

        protected class ЛицоПриПроверке
        {
            public string Должность { get; set; }
            public bool Ознакомлен { get; set; }
            public string ФИО { get; set; }

            public ЛицоПриПроверке(ActCheckWitness actCheckWitness)
            {
                Должность = actCheckWitness.Position;
                Ознакомлен = actCheckWitness.IsFamiliar;
                ФИО = actCheckWitness.Fio;
            }
        }

        protected class ПрилагаемыйДокумент
        {
            public string ДатаДокумента { get; set; }

            public string Наименование { get; set; }

            public string Описание { get; set; }

            public ПрилагаемыйДокумент(ActCheckAnnex attachment)
            {
                ДатаДокумента = attachment.DocumentDate.HasValue
                    ? attachment.DocumentDate.Value.ToString("dd.MM.yyyy")
                    : string.Empty;
                Наименование = attachment.Name;
                Описание = attachment.Description;
            }
        }

        protected class ОбщаяПродолжительность
        {
            public int Дней { get; set; }
            public int Часов { get; set; }
            public int Минут { get; set; }

            public ОбщаяПродолжительность(TimeSpan duration)
            {
                Дней = Math.Ceiling(duration.TotalHours / 8).ToInt();
                Часов = duration.Hours;
                Минут = duration.Minutes;
            }

            public ОбщаяПродолжительность(IEnumerable<Period> periods)
            {
                var duration = periods.Select(x => new {Duration = x.End - x.Start}).Aggregate(new TimeSpan(),
                    (current, aggr) => current + aggr.Duration);
                Часов = duration.Hours;
                Минут = duration.Minutes;
                Дней =
                    periods.DistinctBy(x => string.Format("{0}.{1}.{2}", x.Start.Year, x.Start.Month,x.Start.Day))
                        .Count();
            }
        }
    }
}
