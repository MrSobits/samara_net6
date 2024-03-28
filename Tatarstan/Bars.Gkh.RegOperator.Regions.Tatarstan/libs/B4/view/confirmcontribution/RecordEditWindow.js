Ext.define('B4.view.confirmcontribution.RecordEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 800,
    minWidth: 600,
    height: 250,
    bodyPadding: 5,
    alias: 'widget.confContribRecordEditWindow',
    title: 'Редактирование записи',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Платежное поручение',
                    layout: 'anchor',
                    defaults: {
                        labelWidth: 130,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'RealityObject',
                            fieldLabel: 'Жилой дом',
                            labelWidth: 100,
                            store: 'B4.store.confirmcontribution.RealityObject',
                            textProperty: 'Address',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                {
                                    text: 'Муниципальное образование',
                                    dataIndex: 'Municipality',
                                    flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListWithoutPaging'
                                    }
                                },
                                {
                                    text: 'Адрес',
                                    dataIndex: 'Address',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            margin: '6 0 6 0',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    labelWidth: 100,
                                    flex: 1,
                                    labelAlign: 'right',
                                    maxLength: 50,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'TransferDate',
                                    itemId: 'transferDate',
                                    labelWidth: 150,
                                    flex: 1,
                                    labelAlign: 'right',
                                    format: 'M Y',
                                    fieldLabel: 'Период перечисления',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateFrom',
                                    format: 'd.m.Y',
                                    labelWidth: 100,
                                    flex: 1,
                                    labelAlign: 'right',
                                    fieldLabel: 'от',
                                    itemId: 'dfDateFrom',
                                    allowBlank: false
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'Scan',
                            labelWidth: 100,
                            labelAlign: 'right',
                            fieldLabel: 'Скан документа'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Amount',
                            labelWidth: 100,
                            fieldLabel: 'Сумма платежного поручения',
                            flex: 1,
                            allowBlank: false,
                            decimalSeparator: ',',
                            decimalPrecision: 2
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
                        columns: 2,
                        items: [
                            {
                                xtype: 'b4savebutton'
                            }
                        ]
                        
                    },
                    { xtype: 'tbfill' },
                    {
                        xtype: 'buttongroup',
                        columns: 2,
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