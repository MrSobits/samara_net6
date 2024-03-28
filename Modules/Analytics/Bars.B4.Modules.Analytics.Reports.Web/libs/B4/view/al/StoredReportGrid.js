Ext.define('B4.view.al.StoredReportGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.base.Store',
        'B4.base.Proxy',

        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
    ],

    alias: 'widget.storedreportgrid',
    title: 'Справочник отчетов',
    closable: true,

    initComponent: function () {
        var me = this;
        var type = 'StoredReport';

        me.store = Ext.create('B4.base.Store', {
            model: 'B4.model.al.StoredReport',
            autoLoad: true
        });

        Ext.apply(me, {
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Название отчета',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    text: 'Файл шаблона',
                    flex: 1,
                    renderer: function (v) {
                         return Ext.String.format('<a href="{0}" target="_blank" style="color: blue;">{1}</a>', B4.Url.action('GetTemplate', 'StoredReport', { reportId: v }), "Файл шаблона");
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    text: 'Пример шаблона',
                    flex: 1,
                    renderer: function (v) {
                        return Ext.String.format('<a href="{0}" target="_blank" style="color: blue;">{1}</a>', B4.Url.action('GetEmptyTemplate', 'EmptyTemplate', { reportId: v }), "Пример шаблона");
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Roles',
                    text: 'Доступно ролям',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DataSourcesNames',
                    text: 'Источники данных',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    text: 'Шаблон с тестовыми данными',
                    flex: 1,
                    renderer: function (v) {
                        return Ext.String.format('<a href="{0}" target="_blank" style="color: blue;">{1}</a>', B4.Url.action('GetTemplateWithSampleData', 'EmptyTemplate', { reportId: v }), "Шаблон с тестовыми данными");
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    text: 'Онлайн-редактор шаблона',
                    flex: 1,
                    renderer: function (v) {
                        return Ext.String.format('<a href="{0}" target="_blank" style="color: blue;">{1}</a>', B4.Url.action('', 'StimulDesigner', { id: v, type: type }), "Онлайн-редактор шаблона");
                    }
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    action: 'GetExternalLink',
                                    disabled: true,
                                    text: 'Получить внешнюю ссылку'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});