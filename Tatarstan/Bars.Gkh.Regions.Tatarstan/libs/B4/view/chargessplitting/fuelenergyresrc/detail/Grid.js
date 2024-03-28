Ext.define('B4.view.chargessplitting.fuelenergyresrc.detail.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField'
    ],

    alias: 'widget.fuelenergyresourcedetailgrid',

    cls: 'x-large-head',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.chargessplitting.fuelenergyresrc.FuelEnergyOrgContractInfo');

        me.relayEvents(store, ['beforeload', 'load'], 'store.');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FuelEnergyOrg',
                    text: 'Поставщик ТЭР',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommunalResource',
                    text: 'Ресурс',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SaldoIn',
                    text: 'Задолженность на начало месяца',
                    flex: 1,
                    editor: { xtype: 'gkhdecimalfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtIn',
                    text: 'Входящая просроченная задолженность',
                    flex: 1,
                    editor: { xtype: 'gkhdecimalfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Charged',
                    text: 'Начислено за месяц',
                    flex: 1,
                    editor: { xtype: 'gkhdecimalfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Paid',
                    text: 'Оплачено за месяц',
                    flex: 1,
                    editor: { xtype: 'gkhdecimalfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SaldoOut',
                    text: 'Задолженность на конец месяца',
                    flex: 1,
                    editor: { xtype: 'gkhdecimalfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtOut',
                    text: 'Исходящая просроченная задолженность',
                    flex: 1,
                    editor: { xtype: 'gkhdecimalfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanPaid',
                    text: 'Планируемая оплата',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidDelta',
                    text: 'Расхождение в оплатах',
                    flex: 1
                }
            ],
            plugins:[
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
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