Ext.define('B4.model.smevownershipproperty.SMEVOwnershipPropertyFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVOwnershipPropertyFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVOwnershipProperty' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});