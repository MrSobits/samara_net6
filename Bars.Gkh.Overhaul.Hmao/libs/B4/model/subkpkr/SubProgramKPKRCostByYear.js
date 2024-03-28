Ext.define('B4.model.subkpkr.SubProgramKPKRCostByYear', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'GetCostsByYear',
        timeout: 30 * 60 * 1000 // 30 минут
    },
    fields: [
        { name: 'Year'},
        { name: 'Sum' },
        { name: 'GrantedSum' },
    ]
});