Ext.define('B4.model.dict.service.BilTarifStorage', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BilTarifStorage'
    },
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'ServiceName' },
        { name: 'ServiceTypeName' },
        { name: 'SupplierName' },
        { name: 'TarifValue' },
        { name: 'TarifTypeName' },
        { name: 'FormulaName' },
        { name: 'LsCount' }
    ]
});