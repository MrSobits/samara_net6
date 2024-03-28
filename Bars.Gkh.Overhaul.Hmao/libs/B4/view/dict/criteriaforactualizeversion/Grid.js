Ext.define('B4.view.dict.criteriaforactualizeversion.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.criteriaforactualizeversionpanel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.CriteriaType'
    ],

    title: 'Критерии для актуализации регпрограммы',
    store: 'dict.CriteriaForActualizeVersion',
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
                    dataIndex: 'CriteriaType',
                    flex: 1,
                    text: 'Наименование критерия',
                    renderer: function (val) {
                        return B4.enums.CriteriaType.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ValueFrom',
                    flex: 1,
                    text: 'Нижнее пороговое значение'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ValueTo',
                    flex: 1,
                    text: 'Верхнее пороговое значение'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Points',
                    flex: 1,
                    text: 'Количество баллов'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Weight',
                    flex: 1,
                    text: 'Весовой коэффициент'
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});