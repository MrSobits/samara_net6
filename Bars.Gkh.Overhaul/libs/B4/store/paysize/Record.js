Ext.define('B4.store.paysize.Record', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.paysize.Record'],
    model: 'B4.model.paysize.Record',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/PaysizeRecord/ListTree'),
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