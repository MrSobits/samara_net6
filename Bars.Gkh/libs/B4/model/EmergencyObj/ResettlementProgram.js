Ext.define('B4.model.emergencyobj.ResettlementProgram', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EmerObjResettlementProgram'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EmergencyObject', defaultValue: null },
        { name: 'ResettlementProgramSource', defaultValue: null },
        { name: 'CountResidents', defaultValue: null },
        { name: 'Area', defaultValue: null },
        { name: 'Cost', defaultValue: null },
        { name: 'ActualCost', defaultValue: null },
        { name: 'SourceName' }
    ]
});