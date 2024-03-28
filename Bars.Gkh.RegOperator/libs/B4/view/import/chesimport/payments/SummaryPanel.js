Ext.define('B4.view.import.chesimport.payments.SummaryPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.chesimportpaymentssummarypanel',

    requires: [
        'B4.ux.button.Update',
        'B4.enums.regop.WalletType',
        'B4.view.import.chesimport.payments.SummaryGrid'
    ],

    columnLines: true,
    title: 'Сводные суммы',

    bodyStyle: Gkh.bodyStyle,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    overflowX: 'auto',
                    layout: { type: 'anchor' },
                    defaults: {
                        xtype: 'panel',
                        collapsible: true,
                        collapsed: false,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'chesimportpaymentssummarygrid'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                //{
                                //    xtype: 'button',
                                //    text: 'Экспорт',
                                //    action: 'Export',
                                //    iconCls: 'icon-table-go'
                                //},
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function (button) {
                                        button.up('chesimportpaymentssummarypanel')
                                            .down('chesimportpaymentssummarygrid').getStore().load();
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
