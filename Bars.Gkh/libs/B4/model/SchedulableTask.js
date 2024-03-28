Ext.define('B4.model.SchedulableTask', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id' },
        { name: 'PeriodType', defaultValue: 0 },
        { name: 'StartDate', useNull: true },
        { name: 'EndDate', useNull: true },
        { name: 'StartNow', defaultValue: false },
        { name: 'StartTimeHour', defaultValue: 0 },
        { name: 'StartTimeMinutes', defaultValue: 0 },
        { name: 'StartDayOfWeekList', useNull: false, defaultValue: [] },
        { name: 'StartMonthList', useNull: false, defaultValue: [] },
        { name: 'StartDaysList', useNull: false, defaultValue: [] },

        { name: 'IsDelete', defaultValue: false },
    ]
});