Ext.define('B4.model.AppointmentDiffDayGridModel', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppointmentDiffDay'
    },
    fields: [
        { name: 'AppointmentQueue'},
        { name: 'Day' },
        { name: 'StartTime' },
        { name: 'EndTime' },
        { name: 'StarPauseTime' },
        { name: 'EndPauseTime' },
        { name: 'ChangeAppointmentDay' }
    ]
});