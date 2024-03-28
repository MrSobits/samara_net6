Ext.define('B4.model.StateDuty', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'CourtType' },
        { name: 'Formula' },
        { name: 'FormulaParameters' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'StateDuty'
    }
});