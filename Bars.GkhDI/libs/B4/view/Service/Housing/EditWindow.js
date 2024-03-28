Ext.define('B4.view.service.housing.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 800,
    height: 600,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Жилищная услуга',
    itemId: 'housingServiceEditWindow',
    layout: 'border',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.TemplateService',
        'B4.store.dict.UnitMeasure',
        'B4.store.dict.PeriodicityTemplateService',
        'B4.store.service.ContragentForProvider',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.view.service.housing.TariffForConsumersHousingGrid',
        'B4.view.service.housing.HousingCostItemGrid',
        'B4.view.service.housing.ProviderServiceGrid',
        
        'B4.enums.EquipmentDi'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    height: 200,
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
                                    [40, 'Собственники отказались от предоставления услуги']
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
                                    editable: false,
                                    name: 'UnitMeasure',
                                    flex: 1,
                                    itemId: 'sflUnitMeasure',
                                    fieldLabel: 'Ед. измерения',
                                    anchor: '100%',
                                   

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
                                            editable: false,
                                            name: 'Provider',
                                            itemId: 'sflProvider',
                                            fieldLabel: 'Поставщик',
                                            isGetOnlyIdProperty: false,
                                            allowBlank: false,
                                           

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
                                                    xtype: 'gkhdecimalfield',
                                                    name: 'Profit',
                                                    fieldLabel: 'Доход, полученный за предоставление услуги',
                                                    itemId: 'nfProfit'
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    editable: false,
                                                    name: 'Periodicity',
                                                    itemId: 'sflPeriodicity',
                                                    fieldLabel: 'Периодичность выполнения',
                                                   

                                                    store: 'B4.store.dict.PeriodicityTemplateService'
                                                }
                                            ]
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
                                                    name: 'ProtocolNumber',
                                                    flex: 2,
                                                    fieldLabel: 'Номер протокола',
                                                    itemId: 'nfProtocolNumber',
                                                    maxLength: 300
                                                }
                                            ]
                                        },
                                        {
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ProtocolFrom',
                                                    itemId: 'dfProtocolFrom',
                                                    fieldLabel: 'от',
                                                    format: 'd.m.Y',
                                                    width: 290
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4filefield',
                                    fieldLabel: 'Файл',
                                    name: 'Protocol',
                                    itemId: 'ffProtocol'
                                },
                                {
                                    xtype: 'combobox', editable: false,
                                    name: 'Equipment',
                                    itemId: 'cbEquipment',
                                    fieldLabel: 'Оборудование',
                                    anchor: '100%',
                                    store: B4.enums.EquipmentDi.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'tbHousingGrids',
                    region: 'center',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'housingcostitemgrid',
                            margins: -1
                        },
                        {
                            xtype: 'tariffforconsumhousinggrid',
                            margins: -1
                        },
                        {
                            xtype: 'housproviderservicegrid',
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
