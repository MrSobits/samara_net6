Ext.define('B4.model.boilerroom.HeatingPeriod', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HeatingPeriod'
    },
    fields: [
        { name: 'Start' },
        { name: 'End' },
        { name: 'BoilerRoom' },
        { name: 'IsValid', defaultValue: true }
    ]
});