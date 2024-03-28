Ext.define('B4.model.smevemergencyhouse.SMEVEmergencyHouseFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVEmergencyHouseFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVEmergencyHouse' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});