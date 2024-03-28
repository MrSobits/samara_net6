Ext.define('B4.view.administration.fstownimportsettings.MainGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Import'
    ],

    title: 'Реестр настроек импорта оплат (txt,csv)',
    alias: 'widget.fstownimportsettingsmiangrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.administration.fsTownImportSettings');
            

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание'
                },
                {
                    xtype: 'b4deletecolumn'
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
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Экспорт',
                                    name: 'exportBtn',
                                    disabled: true,
                                    visible: false
                                },
                                {
                                    xtype: 'b4importbutton',
                                    importId: 'Bars.Gkh.RegOperator.Imports.FsGorod.FsGorodImportInfoSettingsImport',
                                    possibleFileExtensions: 'json',
                                    visible: false
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