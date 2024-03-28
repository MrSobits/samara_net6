Ext.define('B4.view.service.communal.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 800,
    height: 600,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Коммунальная услуга',
    itemId: 'communalServiceEditWindow',
    layout: 'border',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhDecimalField',
        'B4.view.service.communal.ConsumptionNormsPanel',
        'B4.store.dict.TemplateService',
        'B4.store.dict.UnitMeasure',
        'B4.store.service.ContragentForProvider',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.view.service.communal.TariffForConsumersCommunalGrid',
        'B4.view.service.communal.TariffForRsoCommunalGrid',
        'B4.view.service.communal.ConsumptionNormsPanel',
        'B4.view.service.communal.ConsumptionNormsNpaGrid',
        'B4.enums.KindElectricitySupplyDi'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    height: 230,
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items:
                    [
                        {
                            xtype: 'b4combobox',
                            editable: false,
                            name: 'TypeOfProvisionService',
                            itemId: 'cbTypeOfProvisionService',
                            fieldLabel: 'Тип предоставления услуги',
                            anchor: '100%',
                            //Не добавлять ниче сюда. Это значения реального enum TypeOfProvisionServiceDi
                            items: [
                                    [10, 'Услуга предоставляется через УО'],
                                    [20, 'Услуга предоставляется без участия УО'],
                                    [30, 'Услуга не предоставляется']
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            itemId: 'fsService',
                            defaults: {
                                labelWidth: 190,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Услуга предоставляется через УО',
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'UnitMeasure',
                                    flex: 1,
                                    itemId: 'sflUnitMeasure',
                                    fieldLabel: 'Ед. измерения',
                                    anchor: '100%',
                                    editable: false,
                                   

                                    store: 'B4.store.dict.UnitMeasure'
                                },
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
                                    xtype: 'gkhdecimalfield',
                                    name: 'Profit',
                                    fieldLabel: 'Доход, полученный за предоставление услуги',
                                    itemId: 'nfProfit'
                                },
                                {
                                    xtype: 'combobox', editable: false,
                                    fieldLabel: 'Вид электроснабжения',
                                    store: B4.enums.KindElectricitySupplyDi.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'KindElectricitySupply',
                                    itemId: 'cbKindElectricitySupply'
                                },
                                {
                                    xtype: 'container',
                                    anchor: '100%',
                                    defaults: {
                                        xtype: 'container',
                                        layout: 'anchor',
                                        flex: 1,
                                        defaults: {
                                            labelWidth: 230,
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
                                                    xtype: 'gkhdecimalfield',
                                                    name: 'VolumePurchasedResources',
                                                    fieldLabel: 'Объем закупаемых ресурсов',
                                                    itemId: 'nfVolumePurchasedResources'
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'gkhdecimalfield',
                                                    name: 'PricePurchasedResources',
                                                    fieldLabel: 'Цена закупаемых ресурсов (руб.)',
                                                    itemId: 'nfPricePurchasedResources'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'tbCommunalGrids',
                    region: 'center',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'tariffforconsumcommungrid',
                            margins: -1
                        },
                        {
                            xtype: 'tariffforrsocommungrid',
                            margins: -1
                        },
                        {
                            itemId: 'tbContainer',
                            title: 'Нормативы потребления',
                            margins: -1,
                            autoScroll: true,
                            items: [
                                {
                                    xtype: 'consumptionnormspanel',
                                    border: false
                                }
                            ]
                        },
                        {
                            xtype: 'consumptionnormsnpagrid',
                            itemId: 'cnNpaGrid',
                            margins: -1
                        },
                        {
                            xtype: 'communproviderservicegrid',
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
