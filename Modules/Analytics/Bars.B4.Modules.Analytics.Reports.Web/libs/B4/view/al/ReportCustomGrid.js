Ext.define('B4.view.al.ReportCustomGrid', {
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

    alias: 'widget.reportcustomgrid',
    title: 'Замена шаблонов',
    closable: true,

    initComponent: function () {
        var me = this;
        var type = 'ReportCustom';

        me.store = Ext.create('B4.base.Store', {
            model: 'B4.model.al.ReportCustom',
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
                    dataIndex: 'ReportName',
                    text: 'Название отчета',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Template',
                    text: 'Файл шаблона',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CodedReportKey',
                    text: '',
                    flex: 1,
                    renderer: function (v) {
                        return Ext.String.format('<a href="{0}" target="_blank" style="color: blue;">Скачать</a>', B4.Url.action('DownloadTemplate', 'CodedReportManager', { CodedReportKey: v }));
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