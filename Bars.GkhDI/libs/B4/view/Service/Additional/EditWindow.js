Ext.define('B4.view.service.additional.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 890,
    height: 710,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Дополнительная услуга',
    itemId: 'additionalServiceEditWindow',
    layout: 'border',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.TemplateService',
        'B4.store.dict.PeriodicityTemplateService',
        'B4.store.dict.UnitMeasure',
        'B4.store.service.ContragentForProvider',
        'B4.form.SelectField',
        'B4.view.service.additional.TariffForConsumersAdditionalGrid',
        'B4.view.service.additional.ProviderServiceGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    height: 320,
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items:
                    [
                         {
                            xtype: 'container',
                            anchor: '100%',
                            defaults: {
                                xtype: 'container',
                                layout: 'anchor',
                                flex: 1,
                                defaults: {
                                    labelWidth: 190,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                }
                            },
                            layout: {
                                type: 'hbox',
                                pack: 'start'
                            },
                            items: [
                                {
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'Periodicity',
                                            itemId: 'sflPeriodicity',
                                            fieldLabel: 'Периодичность выполнения',
                                            editable: false,
                                            allowBlank: false,
                                           

                                            store: 'B4.store.dict.PeriodicityTemplateService'
                                        }
                                    ]
                                },
                                {
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            editable: false,
                                            name: 'UnitMeasure',
                                            itemId: 'sflUnitMeasure',
                                            fieldLabel: 'Ед. измерения',
                                           

                                            store: 'B4.store.dict.UnitMeasure'
                                        }
                                    ]
                                }
                            ]
                         },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Profit',
                            fieldLabel: 'Доход, полученный за предоставление услуги',
                            itemId: 'nfProfit'
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 190,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Поставщик',
                            items: [
                                {
                                    xtype: 'container',
                                    itemId: 'cntProvider',
                                    anchor: '100%',
                                    defaults: {
                                            labelWidth: 190,
                                            labelAlign: 'right'
                                        },
                                    layout: {
                                            type: 'hbox',
                                            pack: 'start'
                                        },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'Provider',
                                            itemId: 'sflProvider',
                                            fieldLabel: 'Поставщик',
                                            isGetOnlyIdProperty: false,
                                            allowBlank: false,
                                            editable: false,
                                           

                                            store: 'B4.store.service.ContragentForProvider',
                                            readOnly: true,
                                            flex: 1,
                                            margins: '0 5 5 0',
                                            columns: [
                                                { text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Изменить',
                                            tooltip: 'Изменить',
                                            itemId: 'changeProviderButton',
                                            width: 120,
                                            margins: '0 0 5 0'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    defaults: {
                                        xtype: 'container',
                                        layout: 'anchor',
                                        flex: 1,
                                        defaults: {
                                            labelWidth: 190,
                                            anchor: '100%',
                                            labelAlign: 'right'
                                        }
                                    },
                                    layout: {
                                        type: 'hbox',
                                        pack: 'start'
                                    },
                                    items: [
                                        {
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Ogrn',
                                                    itemId: 'tfOgrn',
                                                    fieldLabel: 'ОГРН',
                                                    readOnly: true
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateRegistartion',
                                                    itemId: 'dfDateRegistartion',
                                                    fieldLabel: 'Дата регистрации',
                                                    format: 'd.m.Y',
                                                    readOnly: true
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 190,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Сведение о договоре',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Document',
                                    itemId: 'tfDocument',
                                    fieldLabel: 'Документ',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    defaults: {
                                        xtype: 'container',
                                        layout: 'anchor',
                                        flex: 1,
                                        defaults: {
                                            labelWidth: 190,
                                            anchor: '100%',
                                            labelAlign: 'right'
                                        }
                                    },
                                    layout: {
                                        type: 'hbox',
                                        pack: 'start'
                                    },
                                    items: [
                                        {
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'DocumentNumber',
                                                    itemId: 'tfDocumentNumber',
                                                    fieldLabel: 'Номер',
                                                    maxLength: 300
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateStart',
                                                    allowBlank: false,
                                                    itemId: 'dfDateStart',
                                                    fieldLabel: 'Дата начала',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DocumentFrom',
                                                    itemId: 'dfDocumentFrom',
                                                    fieldLabel: 'От',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateEnd',
                                                    allowBlank: false,
                                                    itemId: 'dfDateEnd',
                                                    fieldLabel: 'Дата окончания',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'Total',
                                    itemId: 'tfTotal',
                                    allowBlank: false,
                                    fieldLabel: 'Сумма (руб.)'
                                }
                            ]
                        }

                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'tbAdditionalGrids',
                    region: 'center',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'tariffforconsumersadditgrid',
                            margins: -1
                        },
                        {
                            xtype: 'additproviderservicegrid',
                            margins: -1
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'tbfill'
                                },
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
