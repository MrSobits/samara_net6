Ext.define('B4.view.regoperator.persaccountmu.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.regoperpersaccmugrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.regoperator.PersAccMunicipality',
        'B4.store.regoperator.MunicipalityForSelect'
    ],

    title: 'Лицевые счета по МО',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regoperator.PersAccMunicipality'),
            munStore = Ext.create('B4.store.regoperator.MunicipalityForSelect'),
            numberfield = {
                xtype: 'numberfield',
                hideTrigger: true,
                keyNavEnabled: false,
                mouseWheelEnabled: false,
                allowDecimals: true,
                decimalSeparator: ','
            },
            numberRenderer = function (val) {
                return val ? Ext.util.Format.currency(val) : '';
            };

        Ext.applyIf(me, {
            columnLines: true,
            cls: 'x-large-head',
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 2,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        store: munStore
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersAccountNum',
                    flex: 1,
                    text: 'Лицевой счет',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerFio',
                    flex: 2,
                    text: 'ФИО собственника',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidContributions',
                    flex: 1,
                    text: 'Оплачено взносов, (руб.) ',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CreditContributions',
                    flex: 1,
                    text: 'Начислено взносов, (руб.)',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidContributions',
                    flex: 1,
                    text: 'Оплачено взносов, (руб.) ',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CreditPenalty',
                    flex: 1,
                    text: 'Начислено пени, (руб.)',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidPenalty',
                    flex: 1,
                    text: 'Оплачено пени, (руб.)',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SubsidySumLocalBud',
                    flex: 1,
                    text: 'Сумма субсидии из МБ, (руб.)',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SubsidySumSubjBud',
                    flex: 1,
                    text: 'Сумма субсидии из БС, (руб.)',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SubsidySumFedBud',
                    flex: 1,
                    text: 'Сумма субсидии из ФБ, (руб.)',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumAdopt',
                    flex: 1,
                    text: 'Сумма заимствования, (руб.)',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RepaySumAdopt',
                    flex: 1,
                    text: 'Погашенная сумма заимствования, (руб.)',
                    filter: numberfield,
                    renderer: numberRenderer
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
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