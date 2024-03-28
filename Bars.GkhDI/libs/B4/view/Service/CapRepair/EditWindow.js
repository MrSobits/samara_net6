Ext.define('B4.view.service.caprepair.EditWindow', {
    extend: 'B4.form.Window',
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 800,
    height: 600,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Капитальный ремонт',
    itemId: 'capitalRepairServiceEditWindow',
    layout: 'border',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.TemplateService',
        'B4.store.dict.UnitMeasure',        
        'B4.store.service.ContragentForProvider',
        'B4.form.SelectField',
        'B4.view.service.caprepair.TariffForConsumersCapRepGrid',
        'B4.form.ComboBox',
        'B4.view.service.caprepair.WorkCapRepairGrid',
        'B4.view.service.caprepair.ProviderServiceGrid',
            
        'B4.enums.RegionalFundDi'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                anchor: '100%',
                labelAlign: 'right'
            },
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
                            //Не добавлять ниче сюда. Это значения реального enum TypeOfProvisionServiceDi
                            items:[
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
                                    isGetOnlyIdProperty: false,
                                    editable: false,
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
                                    name: 'RegionalFund',
                                    itemId: 'cbRegionalFund',
                                    fieldLabel: 'Региональный фонд',
                                    store: B4.enums.RegionalFundDi.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'tbCapRepairGrids',
                    region: 'center',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'tarifforconsumcaprepgrid',
                            margins: -1
                        },
                        {
                            xtype: 'workcaprepgrid',
                            margins: -1
                        },
                        {
                            xtype: 'caprepproviderservicegrid',
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
