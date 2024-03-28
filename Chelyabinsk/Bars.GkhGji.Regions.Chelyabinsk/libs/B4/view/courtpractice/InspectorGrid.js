Ext.define('B4.view.courtpractice.InspectorGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.courtpracticeinspectorgrid',

    requires: [
        'B4.Url',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.LawyerInspector',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Должностные лица',
    store: 'courtpractice.CourtPracticeInspector',

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
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.LawyerInspector',
                    dataIndex: 'LawyerInspector',
                    text: 'Юрист/Инспектор',
                    flex: 0.5,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    flex: 1,
                    text: 'ФИО'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Discription',
                    flex: 1,
                    text: 'Примечание'
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