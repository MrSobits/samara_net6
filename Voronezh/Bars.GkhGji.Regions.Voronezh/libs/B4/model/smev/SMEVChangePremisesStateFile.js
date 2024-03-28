Ext.define('B4.model.smev.SMEVChangePremisesStateFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVChangePremisesStateFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVChangePremisesState' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});