Ext.define('B4.view.finactivity.RealityObjEditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Финансовые показатели (для рейтинга)',
    itemId: 'finActivityRealityObjEditPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    requires: [
        'B4.view.finactivity.RealityObjCommunalServiceGrid',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Save'
    ],
    bodyStyle: Gkh.bodyStyle,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fincommunalservicerogrid',
                    region: 'north'
                },
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    bodyPadding: 5,
                    padding: 5,
                    trackResetOnLoad: true,
                    autoScroll: true,
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 365
                        
                    },
                    title: 'Финансовые показатели по ремонту, содержанию дома',
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'WorkRepair',
                            margin: '5 12 3 0',
                            fieldLabel: 'Объем работ по ремонту (тыс. руб.)'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'WorkLandscaping',
                            margin: '7 12 15 0',
                            fieldLabel: 'Объем работ по благоустройству (тыс. руб.)'
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 355
                            },
                            title: 'Объем привлеченных средств на ремонт, модернизацию общего имущества и благоустройства',
                            items: [
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'Subsidies',
                                    fieldLabel: 'Субсидии (тыс. руб.)'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'Credit',
                                    fieldLabel: 'Кредиты (тыс. руб.)'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'FinanceLeasingContract',
                                    fieldLabel: 'Финансирование по договорам лизинга (тыс. руб.)'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'FinanceEnergServContract',
                                    fieldLabel: 'Финансирование по энергосервисным договорам (тыс. руб.)'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'OccupantContribution',
                                    fieldLabel: 'Целевые взносы жителей (тыс. руб.)'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'OtherSource',
                                    fieldLabel: 'Иные источники (тыс. руб.)'
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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
