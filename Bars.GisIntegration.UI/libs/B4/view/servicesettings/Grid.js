Ext.define('B4.view.servicesettings.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',

        'B4.enums.IntegrationService',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum'
    ],

    title: 'Настройки сервисов',
    store: 'ServiceSettings',
    alias: 'widget.servicesettingsGrid',
    closable: false,

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
                    xtype: 'b4enumcolumn',
                    dataIndex: 'IntegrationService',
                    enumName: 'B4.enums.IntegrationService',
                    flex: 1,
                    text: 'Сервис интеграции'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ServiceAddress',
                    flex: 1,
                    text: 'Адрес сервиса'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AsyncServiceAddress',
                    flex: 1,
                    text: 'Адрес асинхронного сервиса'
                },
                {
                    xtype: 'b4deletecolumn'
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
                            items: [
                                {
                                    xtype: 'b4addbutton'
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