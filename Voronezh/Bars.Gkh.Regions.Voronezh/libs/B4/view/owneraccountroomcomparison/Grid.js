Ext.define('B4.view.owneraccountroomcomparison.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.YesNoNotSet',
        'B4.ux.grid.column.Enum',
        'B4.enums.AccountMergeStatus',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox'
    ],

    title: 'Сопоставление аккаунтов с выписками Росреестра',
    store: 'OwnerAccountRoomComparison',
    alias: 'widget.owneraccountroomcomparisongrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },

                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Account_num',
                    flex: 1,
                    text: 'Номер л/с',
                    filter: { xtype: 'textfield' },
               
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'IsAccountsMerged',
                    text: 'Данные сопоставлены',
                    enumName: 'B4.enums.AccountMergeStatus',
                    flex: 1,
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'IsDataUpdated',
                    text: 'Данные изменены',
                    enumName: 'B4.enums.YesNoNotSet',
                    flex: 1,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FIO',
                    flex: 1,
                    text: 'Собственник',
                    filter: { xtype: 'textfield' },
              
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AddressContent',
                    flex: 1,
                    text: 'Адрес из выписки',
                    filter: { xtype: 'textfield' },
               
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ROAddress',
                    flex: 1,
                    text: 'Адрес в системе',
                    filter: { xtype: 'textfield' },

                },
                {
                     xtype: 'datecolumn',
                     dataIndex: 'DataUpdateDate',
                    flex: 0.5,
                    text: 'Дата обновления данных',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DataUpdateDateFrom',
                    flex: 0.5,
                    text: 'Дата вступления в силу новых значений',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
              

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
                            xtype: 'b4updatebutton'
                        },
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});