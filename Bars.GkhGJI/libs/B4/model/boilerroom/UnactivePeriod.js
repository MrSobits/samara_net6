Ext.define('B4.model.boilerroom.UnactivePeriod', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'UnactivePeriod'
    },
    fields: [
        { name: 'Start' },
        { name: 'End' },
        { name: 'BoilerRoom' }
    ]
});