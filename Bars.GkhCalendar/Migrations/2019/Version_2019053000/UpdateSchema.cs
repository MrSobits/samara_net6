namespace Bars.GkhCalendar.Migrations._2019.Version_2019053000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhCalendar.Map;

    [Migration("2019053000")]
    [MigrationDependsOn(typeof(Version_2014090500.UpdateSchema))]

    public class UpdateSchema : Migration
    {

        public override void Up()
        {
            Database.AddEntityTable(
                AppointmentQueueMap.TableName,
                new Column(nameof(AppointmentQueue.TypeOrganisation).ToLower(), DbType.Int32, ColumnProperty.NotNull),
                new Column(nameof(AppointmentQueue.RecordTo).ToLower(), DbType.String, ColumnProperty.NotNull),
                new Column(nameof(AppointmentQueue.Description).ToLower(), DbType.String, ColumnProperty.Null),
                new Column(nameof(AppointmentQueue.TimeSlot).ToLower(), DbType.Int32, ColumnProperty.NotNull)
                );

            Database.AddEntityTable(
                AppointmentDiffDayMap.TableName,
                new Column(nameof(AppointmentDiffDay.StartTime).ToLower(), DbType.DateTime, ColumnProperty.NotNull),
                new Column(nameof(AppointmentDiffDay.EndTime).ToLower(), DbType.DateTime, ColumnProperty.NotNull),
                new Column(nameof(AppointmentDiffDay.StarPauseTime).ToLower(), DbType.DateTime, ColumnProperty.Null),
                new Column(nameof(AppointmentDiffDay.EndPauseTime).ToLower(), DbType.DateTime, ColumnProperty.Null),
                new Column(nameof(AppointmentDiffDay.ChangeAppointmentDay).ToLower(), DbType.String, ColumnProperty.NotNull),
                new RefColumn(nameof(AppointmentDiffDay.AppointmentQueue).ToLower(), $"{ AppointmentDiffDayMap.TableName }_{ AppointmentQueueMap.TableName }_ID", $"{ AppointmentQueueMap.TableName }", nameof(AppointmentQueue.Id).ToLower()),
                new RefColumn(nameof(AppointmentDiffDay.Day).ToLower(), $"{ AppointmentDiffDayMap.TableName }_{ DayMap.TableName }_ID", $"{ DayMap.TableName }", nameof(Day.Id).ToLower())
                );
           
            Database.AddEntityTable(
                AppointmentRecordMap.TableName,
                new Column(nameof(AppointmentRecord.RecordTime).ToLower(), DbType.DateTime, ColumnProperty.NotNull),
                new Column(nameof(AppointmentRecord.Comment).ToLower(), DbType.String, ColumnProperty.NotNull),
                new RefColumn(nameof(AppointmentRecord.AppointmentQueue).ToLower(), $"{ AppointmentRecordMap.TableName }_{ AppointmentQueueMap.TableName }_ID", $"{ AppointmentQueueMap.TableName }", nameof(AppointmentQueue.Id).ToLower()),
                new RefColumn(nameof(AppointmentRecord.Day).ToLower(), $"{ AppointmentRecordMap.TableName }_{ DayMap.TableName }_ID", $"{ DayMap.TableName }", nameof(Day.Id).ToLower())
                );
            
            this.Database.AddEntityTable(
                AppointmentTimeMap.TableName,
                new Column(nameof(AppointmentTime.DayOfWeek).ToLower(), DbType.Int32, ColumnProperty.NotNull),
                new Column(nameof(AppointmentTime.StartTime).ToLower(), DbType.DateTime, ColumnProperty.NotNull),
                new Column(nameof(AppointmentTime.EndTime).ToLower(), DbType.DateTime, ColumnProperty.NotNull),
                new Column(nameof(AppointmentTime.StarPauseTime).ToLower(), DbType.DateTime, ColumnProperty.Null),
                new Column(nameof(AppointmentTime.EndPauseTime).ToLower(), DbType.DateTime, ColumnProperty.Null),
                new RefColumn(nameof(AppointmentTime.AppointmentQueue).ToLower(), $"{ AppointmentTimeMap.TableName }_{ AppointmentQueueMap.TableName }_ID", $"{ AppointmentQueueMap.TableName }", nameof(AppointmentQueue.Id).ToLower())
                );
        }

        public override void Down()
        {
            this.Database.RemoveTable(AppointmentTimeMap.TableName);
            this.Database.RemoveTable(AppointmentRecordMap.TableName);
            this.Database.RemoveTable(AppointmentDiffDayMap.TableName);
            this.Database.RemoveTable(AppointmentQueueMap.TableName);
        }
    }
}