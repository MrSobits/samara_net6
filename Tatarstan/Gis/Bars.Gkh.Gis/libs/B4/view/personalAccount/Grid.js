Ext.define('B4.view.personalAccount.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.personal_account_grid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.PersonalAccount'
    ],

    title: 'Лицевые счета дома',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.PersonalAccount');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { xtype: 'b4editcolumn' },
                { xtype: 'gridcolumn', dataIndex: 'Uic', flex: 1, text: 'УИК', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'gridcolumn', dataIndex: 'AccountId', flex: 1, text: 'Номер лиц.счета', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'gridcolumn', dataIndex: 'PSS', flex: 1, text: 'ПСС', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'PaymentCode', flex: 1, text: 'Платежный код', filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                { xtype: 'gridcolumn', dataIndex: 'ApartmentNumber', flex: 1, text: 'Номер квартиры', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'RoomNumber', flex: 1, text: 'Номер комнаты', filter: { xtype: 'textfield' } }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    if (record.get('IsOpen') != '1') {
                        return 'font-tomato-noimportant';
                    }

                    return '';
                }
            },
            dockedItems: [{
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: store,
                dock: 'bottom'
            }]
        });

        me.callParent(arguments);
    }
});