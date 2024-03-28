Ext.define('B4.view.service.control.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 800,
    height: 600,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Услуга управление МКД',
    itemId: 'controlServiceEditWindow',
    layout: 'border',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.TemplateService',
        'B4.store.service.ContragentForProvider',
        'B4.store.dict.UnitMeasure',
        'B4.form.SelectField',
        'B4.view.service.control.TariffForConsumersControlGrid',
        'B4.view.service.control.ProviderServiceGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    height: 100,
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'UnitMeasure',
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
                            itemId: 'nfProfit',
                            fieldLabel: 'Доход, полученный за предоставление услуги (руб.)'
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'tbControlGrids',
                    region: 'center',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'tariffforconsumcontrolgrid',
                            margins: -1
                        },
                        {
                            xtype: 'contrproviderservicegrid',
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
