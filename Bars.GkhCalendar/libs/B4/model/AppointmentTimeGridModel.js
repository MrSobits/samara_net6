Ext.define('B4.model.AppointmentTimeGridModel', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppointmentTime'
    },
    fields: [
        { name: 'AppointmentQueue'},
        { name: 'DayOfWeek' },
        { name: 'StartTime' },
        { name: 'EndTime' },
        { name: 'StarPauseTime' },
        { name: 'EndPauseTime' }
    ]
});