Ext.define('B4.view.gisaddressmatching.AddressMatchingPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.addressmatchingpnl',

    closable: true,

    title: 'Сопоставление адресов',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    requires: ['B4.view.gisaddressmatching.GisGrid'],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'gisgrid',
                    flex: 2
                },
                {
                    xtype: 'fiasgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});