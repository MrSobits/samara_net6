Ext.define('B4.model.realityobj.ServiceOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [
        'B4.enums.TypeServiceOrg'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectServiceOrg'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Organization', defaultValue: null },
        { name: 'ContragentName' },
        { name: 'TypeServiceOrg', defaultValue: 10 },
        { name: 'Description' },
        { name: 'DocumentName' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'File', defaultValue: null }
    ]
});