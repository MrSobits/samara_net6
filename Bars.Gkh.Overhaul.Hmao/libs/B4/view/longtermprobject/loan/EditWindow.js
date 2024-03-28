Ext.define('B4.view.longtermprobject.loan.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.longtermprobjectloanwindow',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.base.Store',
        'B4.model.LongTermPrObject',
        'B4.base.Proxy'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    height: 250,
    bodyPadding: 5,
    title: 'Займ',
    closable: false,

    initComponent: function () {
        var me = this,
            storeObjects = Ext.create('B4.base.Store', {
                model: 'B4.model.LongTermPrObject',
                fields: [
                    { name: 'Id' }, { name: 'Address' }
                ],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'LongTermPrObject',
                    listAction: 'ListHasDecisionRegopAccount'
                }
            });

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 130
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'ObjectIssued',
                    fieldLabel: 'МКД, выдавший займ',
                    editable: false,
                    store: storeObjects,
                    allowBlank: false,
                    textProperty: 'Address',
                    columns: [
                        { dataIndex: 'Address', text: 'Объект региональной программы', flex: 1, filter: {xtype: 'textfield'} }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 130,
                        flex: 1,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'LoanAmount',
                            hideTrigger: true,
                            decimalSeparator: ',',
                            minValue: 0,
                            fieldLabel: 'Сумма займа (руб.)'
                        },
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            name: 'DateIssue',
                            fieldLabel: 'Дата выдачи'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 130,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            name: 'DateRepayment',
                            fieldLabel: 'Срок погашения',
                            allowBlank: false
                        },
                        {
                            xtype: 'numberfield',
                            name: 'PeriodLoan',
                            hideTrigger: true,
                            allowDecimals: false,
                            readOnly: true,
                            fieldLabel: 'Период займа (мес.)'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Документ',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                flex: 1,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    maxLength: 300,
                                    labelWidth: 120,
                                    fieldLabel: 'Номер'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    format: 'd.m.Y',
                                    maxWidth: 180,
                                    minWidth: 180,
                                    labelWidth: 50,
                                    fieldLabel: 'Дата'
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            labelWidth: 120,
                            fieldLabel: 'Файл',
                            labelAlign: 'right'
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
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