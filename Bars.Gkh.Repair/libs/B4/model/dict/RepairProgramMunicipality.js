Ext.define('B4.model.dict.RepairProgramMunicipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RepairProgramMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MunicipalityName' }        
    ]
});