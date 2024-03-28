Ext.define('B4.view.administration.GkhParamsPanel', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.form.ComboBox',
        'B4.enums.MoLevel',
        'B4.enums.WorkpriceMoLevel',
        'B4.ux.button.Save'
    ],

    title: 'Настройки приложения',
    alias: 'widget.gkhparamspanel',
    layout: 'vbox',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                //anchor: '50%',
                width: 500
                //labelWidth: 260
            },
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Общие',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        xtype: 'container',
                        layout: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'RegionName',
                            fieldLabel: 'Название региона'
                        },
                        {
                            xtype: 'textfield',
                            name: 'RegionOKATO',
                            fieldLabel: 'ОКАТО региона',
                            maskRe: /^[0-9]$/,
                            maxLength: 10
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Жилые дома',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        xtype: 'container',
                        layout: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            itemId: 'cbShowStlRealityGrid',
                            name: 'ShowStlRealityGrid',
                            fieldLabel: 'Отображать МО в реестре жилых домов',
                            labelWidth: 260,
                            width: 500
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbUseAdminOkrug',
                            name: 'UseAdminOkrug',
                            fieldLabel: 'Использовать администр.округ в адресе дома',
                            labelWidth: 260,
                            width: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Долгосрочная программа',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        xtype: 'container',
                        layout: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            name: 'MoLevel',
                            itemId: 'MoLevel',
                            items: B4.enums.MoLevel.getItems(),
                            displayField: 'Display',
                            valueField: 'Value',
                            fieldLabel: 'Уровень МО для ДПКР',
                            editable: false,
                            labelWidth: 260,
                            width: 500
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'RealEstTypeMoLevel',
                            items: B4.enums.MoLevel.getItems(),
                            displayField: 'Display',
                            valueField: 'Value',
                            fieldLabel: 'Уровень МО в типах домов',
                            editable: false,
                            labelWidth: 260,
                            width: 500
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'WorkPriceMoLevel',
                            items: B4.enums.WorkpriceMoLevel.getItems(),
                            displayField: 'Display',
                            valueField: 'Value',
                            fieldLabel: 'Уровень МО в расценках по работам',
                            editable: false,
                            labelWidth: 260,
                            width: 500
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbShowUrbanAreaHigh',
                            name: 'ShowUrbanAreaHigh',
                            fieldLabel: 'Отображать городские округа на уровне районов в ДПКР',
                            labelWidth: 350,
                            width: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Краткосрочная программа',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        xtype: 'container',
                        layout: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'ShowStlObjectCrGrid',
                            fieldLabel: 'Отображать МО в разделе объектов КР',
                            labelWidth: 240,
                            width: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Реестр неплательщиков',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        xtype: 'container',
                        layout: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'ShowStlDebtorGrid',
                            fieldLabel: 'Отображать МО в реестре неплательщиков',
                            labelWidth: 240,
                            width: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Реестр подрядчиков, нарушивших условия договора',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        xtype: 'container',
                        layout: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'ShowStlBuildContractGrid',
                            fieldLabel: 'Отображать МО в реестре подрядчиков',
                            labelWidth: 240,
                            width: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Претензионная работа',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        xtype: 'container',
                        layout: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'ShowStlClaimWork',
                            fieldLabel: 'Отображать МО в модуле',
                            labelWidth: 240,
                            width: 500
                        }
                    ]
                }
            ],

            dockedItems: [
                {
                    xtype: 'buttongroup',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4savebutton'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});