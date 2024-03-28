Ext.define('B4.view.regop.personal_account.action.CorrectPaymentsWin',
{
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.correctpaymentswin',

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.view.regop.personal_account.action.CorrectPaymentsGrid'
    ],

    modal: true,
    closable: false,
    width: 800,
    height: 550,
    minHeight: 300,
    title: 'Корректировка оплат',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function() {
        var me = this;

        Ext.applyIf(me,
        {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: 0,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PersonalAccountNum',
                                    fieldLabel: 'Номер ЛС',
                                    readOnly: true,
                                    flex: 1
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'OperationDate',
                                    fieldLabel: 'Дата операции',
                                    allowBlank: false,
                                    flex: 1,
                                    maxValue: new Date()
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    allowBlank: false,
                                    flex: 1
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Reason',
                                    fieldLabel: 'Причина',
                                    allowBlank: true,
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата документа',
                                    allowBlank: true,
                                    labelWidth: 150,
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'b4filefield',
                                    labelWidth: 150,
                                    name: 'Document',
                                    allowBlank: false,
                                    fieldLabel: 'Документ-основание',
                                    flex: 1
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'correctpaymentsgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});