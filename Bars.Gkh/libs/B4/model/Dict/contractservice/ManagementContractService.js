Ext.define('B4.model.dict.contractservice.ManagementContractService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'managementContractService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'ServiceType' },
        { name: 'UnitMeasure' },
        { name: 'Name' },

        { name: 'UnitMeasureName' }
    ]
});