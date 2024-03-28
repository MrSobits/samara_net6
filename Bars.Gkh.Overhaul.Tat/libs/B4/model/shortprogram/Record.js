Ext.define('B4.model.shortprogram.Record', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ShortProgramRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ShortProgramObject'},
        { name: 'TypeWork'},
        { name: 'WorkName'},
        { name: 'Volume'},
        { name: 'Cost'},
        { name: 'Stage1'},
        { name: 'Work'},
        { name: 'ServiceCost'},
        { name: 'TotalCost'},
        { name: 'TypeDpkrRecord', defaultValue: 10 }
    ]
});