Ext.define('B4.view.finactivity.CommunalServiceGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.fincommunalservicegrid',
    store: 'finactivity.CommunalService',
    itemId: 'finActivityCommunalServiceGrid',
    title: 'Коммунальные услуги',

    requires: [
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.TypeServiceDi'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeServiceDi',
                    flex: 1,
                    text: 'Услуга',
                    renderer: function(val) {
                         return B4.enums.TypeServiceDi.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomeFromProviding',
                    flex: 1,
                    text: 'Доход от представления (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Exact',
                    flex: 1,
                    text: 'Оплачено (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtPopulationStart',
                    flex: 1,
                    text: 'Задолженность населения на начало периода (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtPopulationEnd',
                    flex: 1,
                    text: 'Задолженность населения на конец периода (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtManOrgCommunalService',
                    flex: 1,
                    text: 'Задолженность УК за коммунальные услуги (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidByMeteringDevice',
                    flex: 1,
                    text: 'Оплачено по показаниям общедомовых ПУ (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidByGeneralNeeds',
                    flex: 1,
                    text: 'Оплачено по счетам на общедомовые нужды (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentByClaim',
                    flex: 1,
                    text: 'Выплаты по искам РСО (тыс.руб.)',
                    editor: 'gkhdecimalfield'
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
                                    xtype: 'button',
                                    itemId: 'saveCommunalServiceButton',
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'addDataRealObjButton',
                                    text: 'Заполнить сведениями по домам',
                                    tooltip: 'Заполнить сведениями по домам',
                                    iconCls: 'icon-arrow-in'
                                }                                
                            ]
                        }
                    ]
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        beforeedit: function (editor, e) {
                            if (e.record.data.TypeServiceDi == B4.enums.TypeServiceDi.Summury) {
                                return false;
                            }
                            return true;
                        },
                        edit: function(editor, e) {
                            var store = editor.view.store;
                            var summary = store.findRecord('TypeServiceDi', B4.enums.TypeServiceDi.Summury);
                            var index = store.find('TypeServiceDi', B4.enums.TypeServiceDi.Summury);
                            store.data.items[index].set(e.field, store.sum(e.field) - summary.get(e.field));
                        }

                    }
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});