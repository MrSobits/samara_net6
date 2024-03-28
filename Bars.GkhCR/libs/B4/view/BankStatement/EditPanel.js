Ext.define('B4.view.bankstatement.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    itemId: 'bankStatementEditPanel',
    title: 'Банковская выписка',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',

        'B4.store.ManagingOrganization',
        'B4.store.Contragent',
        'B4.store.dict.Period',
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.TypeFinanceGroup'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                anchor: "100%",
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                    {
                        xtype: 'textfield',
                        name: 'DocumentNum',
                        fieldLabel: 'Номер',
                        maxLength: 300,
                        allowBlank: false
                    },
                    {
                        xtype: 'container',
                        items: [
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'от',
                            format: 'd.m.Y',
                            anchor: null,
                            width: 290,
                            labelWidth: 190,
                            labelAlign: 'right'
                        }]
                    }
                  ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'BudgetYear',
                            fieldLabel: 'Бюджетный год',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            allowDecimals: false
                        },
                        {
                            xtype: 'b4selectfield',
                            editable: false,
                            name: 'Period',
                            fieldLabel: 'Период',
                           

                            store: 'B4.store.dict.Period',
                            textProperty: 'Name',
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'ManagingOrganization',
                    fieldLabel: 'Управляющая организация',
                   

                    store: 'B4.store.ManagingOrganization',
                    textProperty: 'ContragentName',
                    columns: [{ text: 'Наименование', dataIndex: 'ContragentShortName', flex: 1 }]
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Contragent',
                    fieldLabel: 'Контрагент',
                    store: 'B4.store.Contragent',
                    textProperty: 'Name',
                    columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1 }]
                },
                 {
                     xtype: 'container',
                     layout: 'hbox',
                     padding: '0 0 5 0',
                     defaults: {
                         labelWidth: 190,
                         labelAlign: 'right',
                         flex: 1
                     },
                     items: [
                         {
                             xtype: 'textfield',
                             name: 'PersonalAccount',
                             fieldLabel: 'Лицевой счет',
                             maxLength: 300
                         },
                         {
                             xtype: 'combobox', editable: false,
                             fieldLabel: 'Группа финансирования',
                             store: B4.enums.TypeFinanceGroup.getStore(),
                             displayField: 'Display',
                             valueField: 'Value',
                             name: 'TypeFinanceGroup'
                         }
                     ]
                 },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'IncomingBalance',
                            fieldLabel: 'Входящий остаток',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'OutgoingBalance',
                            fieldLabel: 'Исходящий остаток',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false
                        }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'OperLastDate',
                    fieldLabel: 'Последний день операции по счету',
                    format: 'd.m.Y',
                    anchor: null,
                    width: 290
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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