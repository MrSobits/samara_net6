Ext.define('B4.view.dict.workprice.MassAdditionWindow', {
    extend: 'Ext.Window',
    alias: 'widget.massaddwindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.ComboBox',
        'B4.store.dict.municipality.MoArea',
        'B4.store.dict.municipality.Settlement'
    ],

    width: 400,
    title: 'Массовое добавление расценок',
    layout: 'fit',

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            items: [
                {
                    xtype: 'form',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    padding: '5 5 0 5',
                    defaults: {
                        labelWidth: 120,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: 2,
                            style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                            html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">На основе значений базового года будут добавлены расценки по каждому году указанного периода с учетом коэффициента инфляции.</span>'
                        },
                        {
                            xtype: 'fieldset',
                            padding: '5 5 0 5',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Муниципальный район',
                                    name: 'Municipality',                                   
                                    store: 'B4.store.dict.municipality.MoArea',
                                    editable: false,
                                    allowBlank: false,
                                    disabled: false,
                                    columns: [
                                        {
                                            text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Муниципальное образование',
                                    name: 'Settlement',
                                    disabled: false,
                                    store: 'B4.store.dict.municipality.Settlement',
                                    editable: false,
                                    allowBlank: false,
                                    columns: [
                                        {
                                            text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }
                                        }
                                    ]
                                },
                                {
                                    fieldLabel: 'Базовый год',
                                    xtype: 'b4combobox',
                                    url: Ext.String.format('/{0}/{1}', 'WorkPrice', 'YearList'),
                                    storeAutoLoad: false,
                                    displayField: 'Year',
                                    name: 'Year',
                                    listeners: {
                                        change: function (combo, newVal) {
                                            var from = combo.up('form').down('numberfield[name="From"]'),
                                                to = combo.up('form').down('numberfield[name="To"]');

                                            newVal = Number(newVal);
                                            if (newVal) {
                                                from.setMinValue(newVal + 1);
                                                to.setMinValue(newVal + 1);
                                                from.setMaxValue(newVal + 40);
                                                to.setMaxValue(newVal + 40);
                                            } else {
                                                from.setMinValue(null);
                                                to.setMinValue(null);
                                                from.setMaxValue(null);
                                                to.setMaxValue(null);
                                            }
                                        }
                                    }

                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'InflCoef',
                                    fieldLabel: ' Коэффициент инфляции (%)',
                                    allowDecimals: true,
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ','
                                },
                                {
                                    xtype: 'fieldset',
                                    padding: '0 5 0 5',
                                    title: 'Период',
                                    defaults: {
                                        labelWidth: 115,
                                        labelAlign: 'right',
                                        anchor: '100%',
                                        xtype: 'numberfield',
                                        hideTrigger: true
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'С',
                                            name: 'From'
                                        },
                                        {
                                            fieldLabel: 'По',
                                            name: 'To'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ],
            tbar: {
                items: [
                    {
                        xtype: 'b4savebutton',
                        text: 'Продолжить',
                        tooltip: 'Продолжить'
                    },
                    { xtype: 'tbfill' },
                    { xtype: 'b4closebutton' }
                ]
            }
        });
        me.callParent(arguments);
    }
});