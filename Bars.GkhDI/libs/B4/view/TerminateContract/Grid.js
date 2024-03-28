Ext.define('B4.view.terminatecontract.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.terminatecontractgrid',
    store: 'TerminateContract',
    itemId: 'terminateContractGrid',
    title: 'Сведения о расторгнутых договорах',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AddressName',
                    flex: 1,
                    text: 'Адрес'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TerminateReason',
                    flex: 1,
                    text: 'Основание расторжения договора'
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});