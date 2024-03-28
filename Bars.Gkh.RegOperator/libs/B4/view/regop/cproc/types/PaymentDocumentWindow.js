Ext.define('B4.view.regop.cproc.types.PaymentDocumentWindow', {
    extend: 'Ext.window.Window',

    requires: [
        'B4.ux.button.Close'
    ],

    alias: 'widget.comprpaydocewindow',

    title: 'Документ на оплату',

    modal: true,
    closeAction: 'destroy',
    width: 500,
    height: 350,
    layout: 'fit',

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'grid',
                    border: false,
                    columns: [
                        {
                            text: 'Лицевой счет',
                            dataIndex: 'AccountNum',
                            flex: 1
                        },
                        {
                            text: 'Документ на оплату',
                            dataIndex: 'Document',
                            flex: 1,
                            renderer: function(v) {
                                console.log(v);
                                return v;
                            }
                        }
                    ],
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Скачать все'
                                },
                                '->',
                                {
                                    xtype: 'b4closebutton'
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