Ext.define('B4.view.cashpaymentcenter.ManOrgRealObjGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.cashpaymentcenter.ManOrgRealObj'
    ],

    title: 'Управляемые дома',
    cls: 'x-large-head',
    alias: 'widget.cashpaymentcentermanorgrealobjgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.cashpaymentcenter.ManOrgRealObj');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальный район',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    sortable: false
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    width: 150,
                    format: 'd.m.Y',
                    text: 'Дата включения в договор',
                    editor: 'datefield'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    width: 150,
                    format: 'd.m.Y',
                    text: 'Дата исключения из договора',
                    editor: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
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
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
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