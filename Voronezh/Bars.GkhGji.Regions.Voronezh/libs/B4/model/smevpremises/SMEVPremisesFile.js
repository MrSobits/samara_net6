Ext.define('B4.model.smevpremises.SMEVPremisesFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVPremisesFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVPremises' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});