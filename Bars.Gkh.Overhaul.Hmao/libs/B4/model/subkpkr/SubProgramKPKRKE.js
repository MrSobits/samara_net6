Ext.define('B4.model.subkpkr.SubProgramKPKRKE', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'GetKE',
        timeout: 30 * 60 * 1000 // 30 минут
    },
    fields: [
        { name: 'KE' },
        { name: 'Address' },
        { name: 'Sum' },
    ]
});