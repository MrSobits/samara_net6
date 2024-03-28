Ext.define('B4.model.contragentclw.ContragentClw', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'DateFrom' },
        { name: 'DateTo' },
        { name: 'Municipality' },
        { name: 'Name' },
        { name: 'ContragentState' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'ContragentClw'
    }
});