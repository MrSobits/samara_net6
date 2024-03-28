Ext.define('B4.model.objectcr.HousekeeperReportFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HousekeeperReportFile'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'HousekeeperReport' },       
        { name: 'Description' },
        { name: 'FileInfo' }
    ]
});