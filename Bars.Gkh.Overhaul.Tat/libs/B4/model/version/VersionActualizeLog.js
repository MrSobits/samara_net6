Ext.define('B4.model.version.VersionActualizeLog', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VersionActualizeLog'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProgramVersion' },
        { name: 'Municipality' },
        { name: 'UserName' },
        { name: 'DateAction' },
        { name: 'ActualizeType' },
        { name: 'CountActions' },
        { name: 'LogFile' }
    ]
});