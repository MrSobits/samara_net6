Ext.define('B4.view.import.chesimport.payments.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.chesimportpaymentspanel',

    requires: [
        'B4.view.import.chesimport.payments.UnassignedGrid',
        'B4.view.import.chesimport.payments.SummaryPanel',
        'B4.view.import.chesimport.payments.AssignedGrid',
    ],

    bodyStyle: Gkh.bodyStyle,
    closable: true,
    autoScroll: true,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    initComponent: function() {
        var me = this;

        Ext.apply(me,
            {
                items: [
                    {
                        xtype: 'tabpanel',
                        flex: 1,
                        items: [
                            {
                                xtype: 'chesimportpaymentsunassignedgrid'
                            },
                            {
                                xtype: 'chesimportpaymentssummarypanel'
                            },
                            {
                                xtype: 'chesimportpaymentsassignedgrid'
                            }
                        ]
                    }
                ]
            });

        me.callParent(arguments);
    }
});