Ext.define('B4.view.chargessplitting.budgetorg.Grid',
{
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField',
        'B4.view.chargessplitting.budgetorg.FilterPanel'
    ],

    alias: 'widget.budgetorggrid',
    title: 'Договоры ресурсоснабжения (Бюджет)',

    cls: 'x-large-head',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.chargessplitting.budgetorg.BudgetOrgPeriodSumm');

        me.relayEvents(store, ['beforeload', 'load'], 'store.');

        Ext.applyIf(me,
        {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальный район'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Organization',
                    flex: 1,
                    text: 'Организация'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeCustomer',
                    flex: .5,
                    text: 'Вид потребителя'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PublicServiceOrg',
                    flex: 1,
                    text: 'Ресурсоснабжающая организация'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Service',
                    width: 150,
                    text: 'Услуга'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Charged',
                    width: 120,
                    text: 'Начислено',
                    editor: {
                        xtype: 'gkhdecimalfield',
                        minValue: 0,
                        negativeText: 'Значение не может быть отрицательным',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Paid',
                    width: 120,
                    text: 'Оплачено',
                    editor: {
                        xtype: 'gkhdecimalfield',
                        minValue: 0,
                        negativeText: 'Значение не может быть отрицательным',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EndDebt',
                    width: 120,
                    text: 'Задолженность на конец месяца',
                    editor: {
                        xtype: 'gkhdecimalfield',
                        minValue: 0,
                        negativeText: 'Значение не может быть отрицательным',
                        allowBlank: false
                    }
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing',
                {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    name: 'buttons',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отчетные периоды',
                                    action: 'GoToPeriods'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Операции',
                                    iconCls: 'icon-cog-go',
                                    action: 'Operations',
                                    menu: {
                                        items: [
                                            {
                                                text: 'Импорт',
                                                action: 'Import',
                                                importId: 'Bars.Gkh.Regions.Tatarstan.Import.BudgetOrgContractImport',
                                                iconCls: 'icon-page-white-text',
                                                possibleFileExtensions: 'csv'
                                            },
                                            {
                                                text: 'Экспорт',
                                                action: 'Export',
                                                iconCls: 'icon-page-white-put'
                                            }
                                        ]
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    name: 'filters',
                    items: [
                        {
                            xtype: 'budgetorgfilterpanel'
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});