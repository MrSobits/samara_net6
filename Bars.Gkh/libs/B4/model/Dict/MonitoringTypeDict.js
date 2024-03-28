Ext.define('B4.model.dict.MonitoringTypeDict', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MonitoringTypeDict'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'Code' }
    ]
});