Ext.define('B4.view.dict.meteringdevice.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.MeteringDeviceGroup'
    ],

    title: 'Приборы учета',
    store: 'dict.MeteringDevice',
    alias: 'widget.meteringDeviceGrid',
    closable: true,
    
    initComponent: function() {
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
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Group',
                    flex: 1,
                    text: 'Группа',
                    renderer: function (val) {
                        return B4.enums.MeteringDeviceGroup.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccuracyClass',
                    flex: 1,
                    text: 'Класс точности'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReplacementCost',
                    flex: 1,
                    text: 'Стоимость замены (руб.)'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LifeTime',
                    flex: 1,
                    text: 'Срок эксплуатации'
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});