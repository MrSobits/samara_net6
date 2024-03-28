Ext.define('B4.view.protocolrso.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Протокол РСО',
    alias: 'widget.protocolRSOPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.protocolrso.Grid',
        'B4.view.protocolrso.FilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'protocolRSOFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6'
                },
                {
                    xtype: 'protocolRSOGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
