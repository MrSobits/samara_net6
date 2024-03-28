Ext.define('B4.model.version.ProgramVersion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion'
    },
    fields: [
        { name: 'Name' },
        { name: 'State' },
        { name: 'VersionDate', defaultValue: null },
        { name: 'Municipality' },
        { name: 'IsMain' }
    ]
});