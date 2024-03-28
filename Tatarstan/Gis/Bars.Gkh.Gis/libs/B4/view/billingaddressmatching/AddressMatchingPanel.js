Ext.define('B4.view.billingaddressmatching.AddressMatchingPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.importaddressmatchingpnl',

    closable: true,

    title: 'Сопоставление адресов',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    requires: ['B4.view.billingaddressmatching.ImportedGrid', 'B4.view.billingaddressmatching.FiasGrid'],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'importedgrid',
                    flex: 2
                },
                {
                    xtype: 'importedfiasgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});