Ext.define('B4.store.priorityparam.multi.Selected', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.base.Proxy'],
    fields: [
        { name: 'Id', defaultValue: null },
        { name: 'Code', defaultValue: null },
        { name: 'Name', defaultValue: null }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'MultiPriorityParam',
        listAction: 'ListSelected'
    }
});