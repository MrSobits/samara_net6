Ext.define('B4.view.dict.elementoutdoor.WorksElementOutdoorGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.workselementoutdoorgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',

        'B4.ux.grid.plugin.HeaderFilters'
    ],

    title: 'Виды работ',

    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.WorksElementOutdoor');

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeWork',
                    flex: 1,
                    text: 'Тип работы',
                    enumName: 'B4.enums.KindWorkOutdoor',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    flex: 1,
                    text: 'Единица измерения',
                    filter: {
                        xtype: 'textfield'
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4savebutton'
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