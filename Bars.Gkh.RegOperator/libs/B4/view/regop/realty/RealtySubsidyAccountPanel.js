Ext.define('B4.view.regop.realty.RealtySubsidyAccountPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.view.regop.realty.RealtySubsidyAccountTransferGrid',
        'B4.view.regop.realty.RealtyPlanSubsidyAccountOperationGrid'
    ],

    alias: 'widget.realtysubsidyaccpanel',
    closable: true,
    title: 'Счет субсидий',

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    docked: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        me.fireEvent('updateme', me);
                                    }
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'form',
                    border: false,
                    padding: '0 0 5 0',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '10 0 0 0',
                            defaults: {
                                labelWidth: 150,
                                width: 250,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Номер счета',
                                    name: 'AccountNum',
                                    labelWidth: 140,
                                    editable: false
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата открытия счета',
                                    name: 'DateOpen',
                                    width: 230,
                                    editable: false
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата последней операции',
                                    name: 'LastOperationDate',
                                    labelWidth: 160
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '7 0 10 0',
                            defaults: {
                                labelWidth: 150,
                                width: 250,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Итого плановый объем субсидий',
                                    labelWidth: 140,
                                    name: 'PlanOperTotal'
                                },
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    fieldLabel: 'Итого фактический объем субсидий',
                                    width: 230,
                                    name: 'FactOperTotal'
                                },
                                {
                                    margin: '5 0 0 0',
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    fieldLabel: 'Сальдо',
                                    name: 'CurrentBalance',
                                    labelWidth: 160
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    flex: 1,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    padding: '5 5 5 5',
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Плановое поступление субсидий',
                            layout: 'fit',
                            margin: '0 2 0 0',
                            flex: 1,
                            items: {
                                header: false,
                                xtype: 'realtyplansubsidyoperationgrid'
                            }
                        },
                        { xtype: 'splitter' },
                        {
                            xtype: 'fieldset',
                            title: 'Фактическое поступление субсидий',
                            layout: 'fit',
                            flex: 1,
                            margin: '0 0 0 2',
                            items: {
                                header: false,
                                xtype: 'realtysubsidytransfergrid'
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});