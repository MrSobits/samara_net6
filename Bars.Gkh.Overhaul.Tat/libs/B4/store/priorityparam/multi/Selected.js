Ext.define('B4.store.priorityparam.multi.Selected', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.base.Proxy'],
    fields: [
        { name: 'Code', defaultValue: null }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'MultiPriorityParam',
        listAction: 'ListSelected'
    }
});