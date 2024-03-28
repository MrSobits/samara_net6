/**
* тристор отображения истории изменения решений
*/
Ext.define('B4.store.realityobj.decisionhistory.Tree', {
    extend: 'Ext.data.TreeStore',
    autoLoad: false,
    fields: [
        { name: 'Type' },
        { name: 'DateStart', type: 'date' },
        { name: 'DateEnd', type: 'date' },
        { name: 'Protocol' }
    ],
    constructor: function(cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/Decision/GetHistoryTree'),
                reader: {
                    type: 'json'
                },
                root: {
                    text: 'root',
                    expanded: true
                }
            }
        }, cfg)]);
    }
});