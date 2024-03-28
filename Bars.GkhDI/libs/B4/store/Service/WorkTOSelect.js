Ext.define('B4.store.service.WorkToSelect', {
    extend: 'Ext.data.TreeStore',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: false,
            storeId: 'workToForSelectedStore',
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/WorkRepairTechServ/ListTree'),
                reader: {
                    type: 'json'
                },
                root: {
                    text: 'root',
                    expanded: false
                }
            }
        }, cfg)]);
    }
});