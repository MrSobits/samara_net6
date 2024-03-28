Ext.define('B4.view.administration.massdelete.SelectGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.administration.massdelete.RealityObjectStructuralElement'
    ],

    title: 'Конструктивные элементы жилого дома',
    alias: 'widget.massdeleteroseselectgrid',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.administration.massdelete.RealityObjectStructuralElement', {
                proxy: {
                    type: 'b4proxy',
                    listAction: 'ListForMassDelete',
                    controllerName: 'RealityObjectStructuralElement'
                }
            });

        Ext.applyIf(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'MULTI' }),
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Object',
                    flex: 1,
                    text: 'Объект общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Group',
                    flex: 1,
                    text: 'Группа',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Конструктивный элемент',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LastOverhaulYear',
                    width: 150,
                    text: 'Год последнего капремонта',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        allowDecimals: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Volume',
                    width: 100,
                    text: 'Объем',
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    }
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Удалить выбранные КЭ',
                                    action: 'DeleteStructElems',
                                    icon: 'content/img/icons/delete.png'
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