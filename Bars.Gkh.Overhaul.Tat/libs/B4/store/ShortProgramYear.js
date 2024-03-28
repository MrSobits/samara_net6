Ext.define('B4.store.ShortProgramYear', {
    extend: 'B4.base.Store',
    fields: ['Id', 'Name', 'Default'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ShortProgramRecord',
        listAction: 'GetYears'
    }
});