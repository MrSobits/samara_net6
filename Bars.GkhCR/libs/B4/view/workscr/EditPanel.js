Ext.define('B4.view.workscr.EditPanel', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.form.SelectField',
        'B4.store.RealityObject',
        'B4.store.regop.ChargePeriod',
        'B4.view.objectcr.TypeWorkCrHistoryGrid'
    ],

    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    title: 'Паспорт объекта',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    
    alias: 'widget.workscreditpanel',
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 200
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    fieldLabel: 'Объект недвижимости',
                    textProperty: 'Address',
                    readOnly: true,
                    anchor: '100%'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    fieldLabel: 'Программа КР',
                    textProperty: 'Name',
                    readOnly: true,
                    anchor: '100%'
                },
                {
                    xtype: 'fieldset',
                    layout: 'anchor',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 190
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'FinanceSource',
                            itemId: 'tfFinanceSource',
                            fieldLabel: 'Разрез финансирования',
                            store: 'B4.store.dict.FinanceSource',
                            textProperty: 'Name',
                            allowBlank: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Work',
                            fieldLabel: 'Вид работы',
                            readOnly: true,
                            textProperty: 'Name'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'YearRepair',
                            itemId: 'nfYearRepair',
                            fieldLabel: 'Год ремонта',
                            allowDecimals: false,
                            hideTrigger: true,
                            anchor: '50%'
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'numberfield',
                                decimalSeparator: ',',
                                minValue: 0,
                                flex: 1,
                                labelAlign: 'right',
                                labelWidth: 190,
                                hideTrigger: true
                            },
                            items: [
                                {
                                    fieldLabel: 'Объем',
                                    itemId: 'nfVolume',
                                    name: 'Volume'
                                },
                                {
                                    fieldLabel: 'Сумма (руб.)',
                                    itemId: 'nfSum',
                                    name: 'Sum'
                                }
                            ]
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
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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