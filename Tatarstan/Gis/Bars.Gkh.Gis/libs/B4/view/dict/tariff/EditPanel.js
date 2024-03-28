Ext.define('B4.view.dict.tariff.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.tariffdicteditpanel',

    title: 'Сведения о тарифе',

    bodyStyle: Gkh.bodyStyle,
    closable: false,
    layout: 'form',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.dict.tariff.ContragentInfoFieldSet',
        'B4.view.dict.tariff.TariffInfoFieldSet',
        'B4.store.dict.Municipality',
        'B4.store.dict.Service'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            overflowY: 'auto',
            items: [
                {
                    xtype: 'container',
                    margin: '0 20 0 5',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'fit',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                margin: '5 10'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    name: 'EaisDateFields',
                                    layout: 'hbox',
                                    disabled: true,
                                    hidden: true,
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'EiasUploadDate',
                                            fieldLabel: 'Дата загрузки в ЕИAС',
                                            format: 'd.m.Y',
                                            readOnly: true,
                                            margin: '5 0 5 0'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'EiasEditDate',
                                            fieldLabel: 'Дата последнего изменения в ЕИAС',
                                            format: 'd.m.Y',
                                            readOnly: true,
                                            margin: '0 0 5 0'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Municipality',
                                    fieldLabel: 'Муниципальный район',
                                    store: 'B4.store.dict.Municipality',
                                    editable: false,
                                    allowBlank: false,
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            header: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Service',
                                    fieldLabel: 'Услуга',
                                    store: 'B4.store.dict.Service',
                                    editable: false,
                                    allowBlank: false,
                                    columns: [
                                        {
                                            xtype: 'gridcolumn',
                                            header: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 3,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            xtype: 'gridcolumn',
                                            header: 'Код',
                                            dataIndex: 'Code',
                                            flex: 1,
                                            filter: {
                                                xtype: 'numberfield',
                                                hideTrigger: true,
                                                keyNavEnabled: false,
                                                mouseWheelEnabled: false,
                                                minValue: 1,
                                                operand: CondExpr.operands.eq,
                                                allowDecimals: false
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'UnitMeasure',
                                    fieldLabel: 'Единица измерения',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'contragentinfofieldset'
                        },
                        {
                            xtype: 'tariffinfofieldset'
                        }
                    ],
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
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