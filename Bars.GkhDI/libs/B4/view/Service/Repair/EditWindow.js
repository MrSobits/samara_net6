Ext.define('B4.view.service.repair.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 1000,
    height: 600,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Ремонт',
    itemId: 'repairServiceEditWindow',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhDecimalField',
        'B4.store.service.ContragentForProvider',
        'B4.store.dict.UnitMeasure',
        'B4.view.service.repair.TariffForConsumersRepairGrid',
        'B4.view.service.repair.ProviderServiceGrid',
        'B4.view.service.repair.WorkRepairListGrid',
        'B4.view.service.repair.WorkRepairDetailGrid',
        'B4.view.service.repair.WorkRepairTechServGrid',
        'B4.form.ComboBox',
        'B4.form.SelectField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    height: 200,
                    defaults: {
                        labelWidth: 190,
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
                                labelAlign: 'right'
                            },
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
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
                                    xtype: 'gkhdecimalfield',
                                    name: 'Profit',
                                    fieldLabel: 'Доход, полученный за предоставление услуги',
                                    itemId: 'nfProfit'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'tbRepairGrids',
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    border: false,
                    defaults: {
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'tariffforconsumersrepgrid'
                        },
                        {
                            minHeight: 100,
                            border: false,
                            title: 'ППР',
                            layout: {type: 'hbox', align: 'stretch'},
                            items: [
                                {
                                    xtype: 'workreplistgrid',
                                    flex: 1
                                },
                                {
                                    xtype: 'workrepdetailgrid',
                                    minWidth: 380,
                                    flex: 0.5
                                }
                            ]
                        },
                        {
                            xtype: 'workreptechservgrid'
                        },
                        {
                            xtype: 'repairproviderservicegrid'
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
