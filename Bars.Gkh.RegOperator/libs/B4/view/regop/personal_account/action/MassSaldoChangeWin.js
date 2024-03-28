Ext.define('B4.view.regop.personal_account.action.MassSaldoChangeWin', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.masssaldochangewin',

    requires: [
        'B4.form.FileField',
        'B4.view.regop.personal_account.action.MassSaldoChangeGrid',
        'B4.form.EnumCombo',
        'B4.enums.SaldoChangeOperationType',
        'B4.enums.SaldoChangeSaldoFromType',
        'B4.enums.SaldoChangeSaldoToType'
    ],

    modal: true,
    maximizable: true,
    width: 1000,
    height: 600,
    minHeight: 300,
    title: 'Установка и изменение сальдо',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    accountOperationCode: null, //'MassSaldoChangeOperation'
    accountData: null,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
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
                            maxWidth: 900,
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 160,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'OperationType',
                                    fieldLabel: 'Вид операции',
                                    enumName: 'B4.enums.SaldoChangeOperationType',
                                    value: 10
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'Document',
                                    fieldLabel: 'Документ-основание',
                                    allowBlank: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            maxWidth: 900,
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 160
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Reason',
                                    fieldLabel: 'Причина',
                                    allowBlank: false,
                                    flex: 1
                                },
                                {
                                    xtype: 'component'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            maxWidth: 900,
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 160,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'SaldoFrom',
                                    fieldLabel: 'Сальдо с которого переносится',
                                    enumName: 'B4.enums.SaldoChangeSaldoFromType',
                                    value: 10,
                                    disabled: true
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'SaldoTo',
                                    fieldLabel: 'Сальдо на который переносится',
                                    enumName: 'B4.enums.SaldoChangeSaldoToType',
                                    queryMode: 'local',
                                    value: 30,
                                    disabled: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'masssaldochangegrid',
                    flex: 1,
                    accountData: me.accountData
                }
            ]
        });

        me.callParent(arguments);
    }
});