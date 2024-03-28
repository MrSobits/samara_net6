Ext.define('B4.model.chargessplitting.contrpersumm.ContractPeriod', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContractPeriod',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'StartDate' },
        { name: 'Name' },
        { name: 'UoNumber' },
        { name: 'RsoNumber' },
        { name: 'RoNumber' }
    ]
});