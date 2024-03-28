Ext.define('B4.view.builder.LoanEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 900,
    minWidth: 900,
    height: 500,
    minHeight: 500,
    bodyPadding: 5,
    itemId: 'builderLoanEditWindow',
    title: 'Займ',
    closeAction: 'hide',
    closable: false,
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: { type: 'vbox', align: 'stretch' },
                    bodyPadding: 5,
                    defaults: {
                        labelWidth: 120,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentName',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 100
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'от',
                                    format: 'd.m.Y H:i:s'
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Lender',
                            fieldLabel: 'Заимодатель',
                            store: 'B4.store.Contragent',
                            editable: false,
                            columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                flex: 0.33
                            },
                            items: [
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'Amount',
                                    fieldLabel: 'Сумма выданного'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateIssue',
                                    fieldLabel: 'Дата выдачи',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DatePlanReturn',
                                    fieldLabel: 'Плановая дата возврата',
                                    format: 'd.m.Y',
                                    labelWidth: 150
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Описание',
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'builderloanrepaymentgrid',
                    flex: 1
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});