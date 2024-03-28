Ext.define('B4.model.smev.MVDStayingPlaceRegistrationFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MVDStayingPlaceRegistrationFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'MVDStayingPlaceRegistration' },
        { name: 'SMEVFileType' },
        { name: 'Name' },
        { name: 'FileInfo' }
    ]
});