Ext.define('B4.model.objectcr.BuildContractForMassBuild', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuildContractForMassBuild'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'State' },
        { name: 'ObjectCr' },
        { name: 'Municipality' },
        { name: 'DocumentNum' },
        { name: 'DocumentDateFrom' },
        { name: 'Sum' }
    ]
});