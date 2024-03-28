Ext.define('B4.model.volumediscrepancy.DiscrepancyList', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseServiceRegister',
        listAction: 'DiscrepancyList'
    },
    fields: [
        { name: 'HouseAddress' },
        { name: 'Service' },
        { name: 'ServiceName' },
        { name: 'ServiceGroup' },
        { name: 'ServiceId' },
        { name: 'UOData' },
        { name: 'RSOData' },
        { name: 'Discrepancy' },
        { name: 'IsPublished' }
    ]
});