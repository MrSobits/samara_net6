Ext.define('B4.view.administration.risdataexport.ExportInfoGrid',
{
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.enums.FormatDataExportState',
        'B4.enums.FormatDataExportObjectType'
    ],

    alias: 'widget.risdataexportinfogrid',
    title: 'Сведения об отправленных данных',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.administration.risdataexport.FormatDataExportInfo');

        Ext.applyIf(me,
        {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.FormatDataExportState',
                    flex: 1,
                    dataIndex: 'State',
                    text: 'Статус'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.FormatDataExportObjectType',
                    flex: 1,
                    dataIndex: 'ObjectType',
                    text: 'Объект'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    dataIndex: 'LoadDate',
                    text: 'Дата загрузки'
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
                    name: 'buttons',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function () {
                                        store.load();
                                    }
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