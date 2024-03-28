Ext.define('B4.model.cscalculation.CSFormula', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Formula' },
        { name: 'FormulaParameters' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'CSFormula'
    }
});