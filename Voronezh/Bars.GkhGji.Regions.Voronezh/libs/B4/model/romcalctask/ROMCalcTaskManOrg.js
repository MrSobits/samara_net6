Ext.define('B4.model.romcalctask.ROMCalcTaskManOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ROMCalcTaskManOrg'
    },
    fields: [
        { name: 'Id'},
        { name: 'Contragent' },
        { name: 'ShortName' },
        { name: 'Inn' },
        { name: 'ROMCalcTask' }
    ]
});