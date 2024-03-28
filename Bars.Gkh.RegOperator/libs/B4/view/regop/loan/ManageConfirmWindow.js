Ext.define('B4.view.regop.loan.ManageConfirmWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.manageconfirmwin',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],
    closeAction: 'destroy',
    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    saveBtnClickListeners: null,
    callbackScope: null,
    bodyPadding: 10,
    width: 400,
    minHeight: 250,


    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'gridpanel',
                    flex: 1,
                    store: Ext.create('Ext.data.Store', {
                        fields: [
                            { name: 'RealityObject' },
                            { name: 'Address' },
                            { name: 'LoanSum' },
                            { name: 'PlanEndDate' }
                        ]
                    }),
                    columns: [
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1
                        },
                        {
                            text: 'Сумма',
                            dataIndex: 'LoanSum',
                            flex: 1
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    layout: 'fit',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'form',
                            padding: 5,
                            bodyStyle: Gkh.bodyStyle,
                            border: false,
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Заниматель',
                                    readOnly: true,
                                    name: 'Address',
                                    anchor: '100%'
                                },
                                {
                                    xtype: 'numberfield',
                                    fieldLabel: 'Сумма займа',
                                    allowDecimals: true,
                                    readOnly: true,
                                    name: 'LoanSum',
                                    anchor: '100%'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    items: [
                        '->',
                        {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function(btn) {
                                    btn.up('window').close();
                                }
                            }
                        },
                        {
                            xtype: 'b4savebutton',
                            text: 'Подтвердить',
                            listeners: {
                                click: {
                                    fn: me.saveBtnClickListeners || Ext.emptyFn,
                                    scope: me.callbackScope || this
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