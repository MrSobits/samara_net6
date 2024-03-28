Ext.define('B4.view.fssp.courtordergku.UploadDownloadInfoPanel', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.uploaddownloadinfopanel',
    title: 'Реестр загрузки/выгрузки информации',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.enums.FsspFileState',
        'B4.ux.grid.column.Enum',
        'B4.view.Control.GkhButtonImport'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.fssp.courtordergku.UploadDownloadInfo');

        Ext.applyIf(me,
            {
                columnLines: true,
                store: store,
                columns: [
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'DownloadFile',
                        text: 'Загруженный файл',
                        renderer: function (v) {
                            return this.getFile(v);
                        }
                    },
                    {
                        xtype: 'datecolumn',
                        flex: 0.4,
                        format: 'd.m.Y H:i:s',
                        text: 'Дата загрузки',
                        dataIndex: 'DateDownloadFile',
                        width: 160,
                        filter: {
                            xtype: 'datefield',
                            format: 'd.m.Y'
                        }
                    },
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'User',
                        text: 'Пользователь'
                    },
                    {
                        xtype: 'b4enumcolumn',
                        dataIndex: 'Status',
                        flex: 0.3,
                        text: 'Статус загрузки',
                        enumName: 'B4.enums.FsspFileState',
                        filter: true
                    },
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'LogFile',
                        text: 'Лог загрузки',
                        recVisible: false,
                        renderer: function (value, meta, record) {
                            var me = this,
                                el = me.getView().panel.down('gridcolumn[dataIndex=LogFile]'),
                                logFile = record.data.LogFile;

                            if (el.recVisible && logFile != null) {
                                return me.getFile(logFile);
                            }
                        }
                    },
                    {
                        xtype: 'gridcolumn',
                        flex: 1,
                        filter: { xtype: 'textfield' },
                        dataIndex: 'UploadFile',
                        text: 'Выгруженный файл',
                        recVisible: false,
                        renderer: function (value, meta, record) {
                            var me = this,
                                el = me.getView().panel.down('gridcolumn[dataIndex=UploadFile]'),
                                uploadFile = record.data.UploadFile;

                            if (el.recVisible && uploadFile != null) {
                                return me.getFile(uploadFile);
                            }
                        }
                    },
                    {
                        xtype: 'actioncolumn',
                        dataIndex: 'FollowToMatchingAddress',
                        text: 'Перейти к сопоставлению адресов<br/>из файла',
                        sortable: false,
                        flex: 0.6,
                        align: 'center',
                        recVisible: false,
                        renderer: function (value, meta, record) {
                            if (this.recVisible) {
                                this.iconCls = 'icon-arrow-right';
                            }
                            else {
                                this.iconCls = null;
                            }
                        },
                        handler: function (gridView, rowIndex, clollIndex, el, e, record) {
                            var grid = this.up('grid');

                            if (this.recVisible) {
                                grid.fireEvent('rowaction', grid, 'matchaddresspanel', record);
                            }
                        }
                    },
                    {
                        xtype: 'actioncolumn',
                        align: 'center',
                        dataIndex: 'ReloadFile',
                        sortable: false,
                        text: 'Обновить файлы',
                        recVisible: false,
                        reloadFileStatus: [
                            B4.enums.FsspFileState.UploadWithErrors,
                            B4.enums.FsspFileState.Failed
                        ],
                        renderer: function (value, meta, record) {
                            if (this.reloadFileStatus.includes(record.get('Status')) && this.recVisible) {
                                this.iconCls = 'icon-arrow-refresh';
                                this.tooltip = "Обновить файлы";
                            }
                            else {
                                this.iconCls = null;
                                this.tooltip = null;
                            }
                        },
                        handler: function (gridView, rowIndex, clollIndex, el, e, record) {
                            if (this.reloadFileStatus.includes(record.get('Status')) && this.recVisible) {
                                var grid = this.up('grid');
                                grid.fireEvent('rowaction', grid, 'reloadFile', record);
                            }
                        }
                    }
                ],
                plugins: [
                    Ext.create('B4.ux.grid.plugin.HeaderFilters')
                ],
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
                                        xtype: 'button',
                                        text: 'Загрузить файл',
                                        tooltip: 'Загрузить файл',
                                        iconCls: 'icon-build',
                                        itemId: 'btnImport',
                                        importId: 'Bars.Gkh.Regions.Tatarstan.Import.Fssp.CourtOrderGku.CourtOrderGkuInfoImport',
                                        possibleFileExtensions: 'csv,xls,xlsx'
                                    },
                                    {
                                        xtype: 'b4updatebutton'
                                    },
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
    },
    getFile: function (v) {
        if (v && v != '|.') {
            var index = v.indexOf('|');
            var fileId = v.substring(0, index);
            var fileName = v.substring(++index, v.length);
            return '<a href="' +
                B4.Url.action('/FileUpload/Download?id=' + fileId) +
                '" target="_blank" style="color: black">' +
                fileName +
                '</a>';
        }
        return '';
    }
});