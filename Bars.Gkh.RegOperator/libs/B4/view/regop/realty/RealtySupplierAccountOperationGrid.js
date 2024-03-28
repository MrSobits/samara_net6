Ext.define('B4.view.regop.realty.RealtySupplierAccountOperationGrid', {
    extend: 'B4.ux.grid.Panel',
    title: 'Операции по оплате',

    requires: [
       'B4.ux.grid.toolbar.Paging',
       'B4.ux.grid.plugin.HeaderFilters',
       'B4.enums.regop.PaymentOperationType',
        'B4.form.EnumCombo',
       'B4.store.regop.realty.RealtySupplierAccountOperation',
       'B4.ux.grid.column.Enum'
    ],
    alias: 'widget.realtysuppaccopgrid',

    initComponent: function () {
        var me = this;
        me.store = Ext.create('B4.store.regop.realty.RealtySupplierAccountOperation');

        Ext.apply(me, {
            columns: [
                {
                    text: 'Дата',
                    dataIndex: 'Date',
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    text: 'Вид работы',
                    dataIndex: 'WorkName',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.regop.PaymentOperationType',
                    text: 'Тип операции',
                    dataIndex: 'OperationType',
                    flex: 1
                },
                {
                    text: 'Обороты по дебету',
                    dataIndex: 'Debt',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                },
                {
                    text: 'Обороты по кредиту',
                    dataIndex: 'Credit',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});