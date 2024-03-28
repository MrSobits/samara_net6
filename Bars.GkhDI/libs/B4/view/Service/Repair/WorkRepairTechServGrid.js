Ext.define('B4.view.service.repair.WorkRepairTechServGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.workreptechservgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',        
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.view.Control.GkhDecimalField'
    ],
    store: 'service.WorkRepairTechServ',
    itemId: 'workRepairTechServGrid',
    title: 'Работы по ТО',
    closable: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'workRepairDetailAddButton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'tbfill'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'SumWorkTo',
                                    itemId: 'sumWorkTo',
                                    width: 200,
                                    labelWidth: 100,
                                    fieldLabel: 'Сумма (руб.)',
                                    allowBlank: false
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