Ext.define('B4.model.actcheck.ControlMeasures', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckControlMeasures'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ControlActivity', defaultValue: null },
        { name: 'ActCheck', defaultValue: null },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'Description' }
    ]
});