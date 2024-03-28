Ext.define('B4.model.realityobj.Land', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectLand'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'DateLastRegistration' },
        { name: 'DocumentName' },
        { name: 'DocumentNum' },
        { name: 'DocumentDate' },
        { name: 'CadastrNumber' },
        { name: 'File', defaultValue: null }
    ]
});