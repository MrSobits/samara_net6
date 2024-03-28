Ext.define('B4.model.romcalctask.ROMCalcTask', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ROMCalcTask'
    },
    fields: [
        { name: 'Id'},
        { name: 'Inspector' },
        { name: 'KindKND' },
        { name: 'TaskDate' },
        { name: 'YearEnums' },        
        { name: 'CalcDate' },
        { name: 'CalcState' },
        { name: 'FileInfo' }
        
    ]
});