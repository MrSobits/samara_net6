Ext.define('B4.view.regop.personal_account.action.PaymentDocumentType', {
    extend: 'Ext.window.Window',

    alias: 'widget.paymentdocumenttypewin',

    requires: [
        'B4.ux.button.Close'
    ],

    title: 'Формирование документов на оплату юридических лиц',
    width: 550,
    modal: true,
    closeAction: 'destroy',
    bodyPadding: 10,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
            {
                xtype: 'form',
                border: false,
                bodyStyle: Gkh.bodyStyle,
                defaults: {
                    labelWidth: 200,
                    anchor: '100%'
                },
                items: [
                    {
                        xtype: 'combobox',
                        queryMode: 'local',
                        valueField: 'type',
                        displayField: 'name',
                        editable: false,
                        value: 0,
                        store: Ext.create('Ext.data.Store', {
                            fields: ['type', 'name'],
                            data: [
                                { "type": 0, "name": "Сформировать документы по выбранному адресу" },
                                { "type": 1, "name": "Сформировать документы по всем адресам, находящимся в собственности орг." }
                            ]
                        })
                    }
                ]
            }],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'button',
                            action: 'prepare',
                            text: 'Сформировать',
                            tooltip: 'Сформировать',
                            iconCls: 'icon-accept'
                        },
                        '->',
                        {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function(btn) {
                                    btn.up('window').close();
                                }
                            }
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});