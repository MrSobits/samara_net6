Ext.define('B4.store.dict.NormativeDocItemTreeStore', {
    extend: 'Ext.data.TreeStore',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            storeId: 'NormativeDocItemTreeStore',
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/ViolationNormativeDocItemGji/ListTree'),
                reader: {
                    type: 'json'
                }
            }
        }, cfg)]);
    }
});