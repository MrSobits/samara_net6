Ext.define('B4.view.regop.personal_account.action.CalcDebtWin',
{
    extend: 'B4.form.Window',
    alias: 'widget.calcdebtwin',

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.field.AreaShareField',
        'B4.view.regop.personal_account.action.CalcDebtGrid'
    ],

    modal: true,
    saveBtnClickListeners: null,
    maximized: true,
    bodyPadding: 10,
    closable: false,
    width: 1000,
    height: 550,
    minHeight: 300,
    title: 'Расчет долга',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function() {
        var me = this,
            currentDate = new Date();

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
                                    name: 'Address',
                                    fieldLabel: 'Адрес',
                                    readOnly: true,
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
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'CurrentOwner',
                                    fieldLabel: 'Текущий собственник',
                                    readOnly: true,
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
                                    xtype: 'areasharefield',
                                    name: 'AreaShare',
                                    fieldLabel: 'Доля',
                                    readOnly: true,
                                    labelWidth: 100,
                                    flex: 1
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'Area',
                                    fieldLabel: 'Площадь',
                                    readOnly: true,
                                    labelWidth: 100,
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
                                    xtype: 'b4selectfield',
                                    name: 'PreviousOwner',
                                    fieldLabel: 'Прежний собственник',
                                    store: 'B4.store.regop.owner.LegalAccountOwner',
                                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
                                    allowBlank: false,
                                    editable: false
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
                                    name: 'DateStart',
                                    fieldLabel: 'Период с',
                                    allowBlank: false,
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateEnd',
                                    fieldLabel: 'Период по',
                                    allowBlank: false,
                                    format: 'd.m.Y',
                                    maxValue: currentDate
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
                                    xtype: 'textfield',
                                    name: 'AgreementNumber',
                                    fieldLabel: 'Номер соглашения',
                                    allowBlank: false,
                                    labelWidth: 150,
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'b4filefield',
                                    labelWidth: 150,
                                    name: 'Document',
                                    allowBlank: false,
                                    fieldLabel: 'Документ-основание',
                                    extractFileInput: function () {
                                        var me = this,
                                            fileInput = me.fileInputEl.dom;
                                        return fileInput;
                                    },
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'calcdebtgrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4savebutton',
                            disabled: true
                        },
                        {
                            xtype: 'button',
                            text: 'Выгрузить данные',
                            action: 'export',
                            disabled: true
                        },
                        {
                            xtype: 'button',
                            text: 'Печать',
                            action: 'print',
                            disabled: true
                        },'->', {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function (btn) {
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