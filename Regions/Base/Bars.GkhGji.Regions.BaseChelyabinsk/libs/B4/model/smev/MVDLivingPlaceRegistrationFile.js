Ext.define('B4.model.smev.MVDLivingPlaceRegistrationFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MVDLivingPlaceRegistrationFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'MVDLivingPlaceRegistration' },
        { name: 'SMEVFileType' },
        { name: 'Name' },
        { name: 'FileInfo' }
    ]
});