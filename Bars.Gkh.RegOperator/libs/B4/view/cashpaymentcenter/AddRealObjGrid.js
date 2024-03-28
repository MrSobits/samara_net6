Ext.define('B4.view.cashpaymentcenter.AddRealObjGrid', {
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
        'B4.store.cashpaymentcenter.RealObjForAdd'
    ],

    title: 'Объекты',
    alias: 'widget.cashpaymentcenteraddrealobjgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.cashpaymentcenter.RealObjForAdd');

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
                    dataIndex: 'Settlement',
                    flex: 1,
                    text: 'Муниципальное образование',
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
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountNum',
                    flex: 1,
                    hidden: true,
                    text: 'Лицевой счет',
                    sortable: false
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
                                //,
                                //{
                                //    xtype: 'checkbox',
                                //    action: 'showPersAcc',
                                //    boxLabel: 'Показать ЛС',
                                //    labelWidth: 100                               
                                //}
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});