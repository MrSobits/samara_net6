Ext.define('B4.view.realityobj.MeteringDeviceGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realityobjMeteringDeviceGrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.TypeAccounting'
    ],

    title: 'Приборы учета и узлы регулирования',
    store: 'realityobj.MeteringDevice',
    closable: true,

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
                    dataIndex: 'MeteringDevice',
                    flex: 2,
                    text: 'Прибор учета'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccuracyClass',
                    width: 110,
                    text: 'Класс точности'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateRegistration',
                    flex: 1,
                    text: 'Дата постановки на учет',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeAccounting',
                    flex: 1,
                    text: 'Тип учета',
                    renderer: function (val) {
                        return B4.enums.TypeAccounting.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание'
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
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
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