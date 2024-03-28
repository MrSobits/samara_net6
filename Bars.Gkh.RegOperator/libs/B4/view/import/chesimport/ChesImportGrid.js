Ext.define('B4.view.import.chesimport.ChesImportGrid', {
    extend: 'Ext.grid.Panel',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Progress',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.view.Control.GkhButtonImport',
        'Ext.ux.RowExpander',
        'Ext.grid.feature.RowBody',

        'B4.enums.ChesImportState',
        'B4.enums.ChesAnalysisState',
        'B4.enums.regop.FileType'
    ],

    title: 'Импорт сведений от биллинга',
    alias: 'widget.chesimportgrid',
    closable: true,
    columnLines: true,

    plugins: [
        {
            ptype: 'rowexpander',
            pluginId: 'rowExpander',
            expandOnDblClick: false,
            rowBodyTpl: [
                '<p>',
                '<tpl if="LoadedFiles">',
                '<b>Загруженные файлы: </b>',
                '<tpl for="LoadedFiles" between=",">',
                '{[xindex !== 1 ? ", " : ""]}',
                '{[B4.enums.regop.FileType.displayRenderer(values)]}',
                '</tpl>',
                '<br/>',
                '</tpl>',
                '</p>'
            ]
        }],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.import.ChesImport');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Period',
                    text: 'Период',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    format: 'd.m.Y',
                    text: 'Дата загрузки',
                    flex: 1,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    flex: 1,
                    enumName: 'B4.enums.ChesImportState',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'User',
                    text: 'Пользователь',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'AnalysisState',
                    text: 'Статус разбора',
                    enumName: 'B4.enums.ChesAnalysisState',
                    flex: 1,
                    filter: true
                },
                {
                    xtype: 'progresscolumn',
                    dataIndex: 'Task',
                    text: 'Прогресс',
                    showValue: true,
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'gkhbuttonimport'
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
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
