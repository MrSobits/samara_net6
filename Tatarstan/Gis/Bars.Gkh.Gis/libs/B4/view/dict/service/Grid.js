Ext.define('B4.view.dict.service.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Услуги',
    alias: 'widget.servicedictgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.Service');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    width: 100,
                    text: 'Код',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeService',
                    flex: 1,
                    text: 'Тип услуги',
                    enumName: 'B4.enums.TypeServiceGis',
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeCommunalResource',
                    flex: 1,
                    text: 'Вид коммунального ресурса',
                    enumName: 'B4.enums.TypeCommunalResource',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    flex: 1,
                    text: 'Единица измерения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});