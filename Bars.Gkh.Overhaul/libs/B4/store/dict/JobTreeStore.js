Ext.define('B4.store.dict.JobTreeStore', {
    extend: 'Ext.data.TreeStore',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            storeId: 'JobTreeStore',
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/Job/ListTree'),
                reader: {
                    type: 'json'
                }
            }
        }, cfg)]);
    }
});