Ext.define('B4.view.dict.mocoefficient.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 800,
    bodyPadding: 5,
    itemId: 'mocoefficientEditWindow',
    title: 'Коэффициент',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.TreeSelectField',
        'B4.form.SelectField',
        'B4.form.TreeSelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.Municipality',
        'B4.store.dict.MunicipalitySelectTree',
        'B4.store.cscalculation.CategoryCSMKD',
        'B4.model.Fias'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'treeselectfield',
                    name: 'Municipality',
                    itemId: 'tsMunicipality',
                    fieldLabel: 'Муниципальное образование',
                    titleWindow: 'Выбор муниципального образования',
                    allowBlank: false,
                    store: 'B4.store.dict.MunicipalitySelectTree',
                    editable: false
                },                
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Категория',
                    name: 'CategoryCSMKD',
                    itemId: 'tsCategoryCSMKD',
                    store: 'B4.store.cscalculation.CategoryCSMKD',
                    editable: false,
                    allowBlank: true,
                    columns: [
                        {
                            text: 'Наименование', dataIndex: 'Name', flex: 1,
                            filter: {
                                xtype: 'textfield'
                            }
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    allowBlank: false,
                    hidden: false,
                    fieldLabel: 'Наименование'
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            allowBlank: false,
                            hidden: false,
                            fieldLabel: 'Код'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Value',
                            allowBlank: false,
                            decimalPrecision: 4,
                            fieldLabel: 'Значение',
                            itemId: 'nfValue'
                        },
                        {
                            xtype: 'textfield',
                            name: 'UnitMeasure',
                            allowBlank: true,
                            hidden: false,
                            fieldLabel: 'Ед.изм.'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateFrom',
                            fieldLabel: 'Дата с',
                            format: 'd.m.Y',
                            itemId: 'dfDateFrom',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateTo',
                            fieldLabel: 'по',
                            format: 'd.m.Y',
                            itemId: 'dfDateTo',
                            allowBlank: true
                        },
                        {
                            xtype: 'component'
                        }
                    ]
                },
                {
                    xtype: 'component'
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
                        {
                            xtype: 'tbfill'
                        },
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