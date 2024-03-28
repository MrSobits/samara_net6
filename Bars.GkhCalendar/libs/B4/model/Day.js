Ext.define('B4.model.Day', {
    extend: 'B4.base.Model',
    //requires: ['B4.enums.DayType'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Day'
    },
    fields: [
        { name: 'Id' },
        { name: 'DayDate' },
        { name: 'DayType', defaultValue: 10 }
    ]
});