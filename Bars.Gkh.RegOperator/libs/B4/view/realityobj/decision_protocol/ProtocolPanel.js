Ext.define('B4.view.realityobj.decision_protocol.ProtocolPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.protocolpanel',

    requires: [
        'B4.view.realityobj.decision_protocol.ProtocolGrid',
        'B4.ux.grid.EntityHistoryInfoGrid'
    ],

    title: 'Протоколы и решения',
    closable: true,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    autoScroll: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    flex: 1,
                    items: [
                        {
                            xtype: 'protocolgrid',
                            closable: false,
                            autoScroll: true
                        },
                        {
                            xtype: 'entityhistoryinfogrid',
                            closable: false,
                            autoScroll: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});