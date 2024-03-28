Ext.define('B4.model.TaskCalendar', {
    extend: 'B4.base.Model',
    //requires: ['B4.enums.DayType'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar'
    },
    fields: [
        { name: 'Id' },
        { name: 'DayDate' },
        { name: 'TaskCount', defaultValue: 0 },
        { name: 'DayType', defaultValue: 10 }
    ]
});