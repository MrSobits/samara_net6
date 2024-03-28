Ext.define('B4.model.version.ProgramVersion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'VersionDate', defaultValue: null },
        { name: 'IsMain' }
    ]
});