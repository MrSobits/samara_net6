Ext.define('B4.model.servorg.RealityObjectContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ServiceOrgContract'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ServOrg', defaultValue: null },
        { name: 'ServOrgId' },
        { name: 'Address' },
        { name: 'ContragentName' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'FileInfo', defaultValue: null },
        { name: 'Note' },
        { name: 'RealityObjectId' }
    ]
});