Ext.define('B4.view.analytics.importdata.importdataot.ImportPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField',
        'B4.store.import.OpenTatarstan',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Update',
        'B4.enums.ImportResult',
        'B4.enums.TypeImportFormat',
        'B4.enums.TypeIncImportFormat'
    ],

    title: 'Загрузка данных в "Открытый Татарстан"',
    alias: 'widget.importdataotpanel',
    layout: 'anchor',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,

    initComponent: function () {
        var me = this,
            loadedFileStore = Ext.create('B4.store.import.OpenTatarstan'),
            monthStore = Ext.create('Ext.data.Store', {
                fields: ['num', 'name'],
                data: [
                    { "num": 1, "name": "Январь" },
                    { "num": 2, "name": "Февраль" },
                    { "num": 3, "name": "Март" },
                    { "num": 4, "name": "Апрель" },
                    { "num": 5, "name": "Май" },
                    { "num": 6, "name": "Июнь" },
                    { "num": 7, "name": "Июль" },
                    { "num": 8, "name": "Август" },
                    { "num": 9, "name": "Сентябрь" },
                    { "num": 10, "name": "Октябрь" },
                    { "num": 11, "name": "Ноябрь" },
                    { "num": 12, "name": "Декабрь" }
                ]
            });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    itemId: 'ctnText',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" ' +
                        'style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">' +
                        'Выберите файл с данными для загрузки. Допустимые типы файлов: .xls, .xlsx</span>'
                },
                {
                    xtype: 'form',
                    margin: '0 10 5 10',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    itemId: 'importForm',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    name: 'FileImport',
                                    fieldLabel: 'Файл',
                                    labelWidth: 60,
                                    allowBlank: false,
                                    editable: false,
                                    labelAlign: 'right',
                                    flex: 1,
                                    itemId: 'fileImport',
                                    possibleFileExtensions: 'xls,xlsx'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-build',
                                    name: 'btnLoadFile',
                                    margin: '0 5 5 10',
                                    text: 'Загрузить'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                labelWidth: 60,
                                allowBlank: false,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'name',
                                    fieldLabel: 'Название',
                                    flex: 3
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'month',
                                    fieldLabel: 'Месяц',
                                    flex: 1,
                                    valueField: 'num',
                                    displayField: 'name',
                                    editable: false,
                                    store: monthStore
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'year',
                                    fieldLabel: 'Год',
                                    flex: 1,
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    minValue: 2014,
                                    maxValue: 2043,
                                    margin: '0 5 5 10'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'form',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    anchor: '0 -90',
                    layout: {
                        type: 'anchor'
                    },
                    items: [
                        {
                            xtype: 'gridpanel',
                            anchor: '0 0',
                            margin: 5,
                            name: 'ImportGrid',
                            store: loadedFileStore,
                            columnLines: true,
                            cls: 'x-large-head',
                            columns: {
                                defaults: {
                                    sortable: false,
                                    hideable: false
                                },
                                items: [
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'ImportName',
                                        flex: 3,
                                        text: 'Наименование импорта'
                                    },
                                    {
                                        xtype: 'b4enumcolumn',
                                        flex: 3,
                                        dataIndex: 'ImportResult',
                                        text: 'Статус импорта',
                                        enumName: 'B4.enums.ImportResult'
                                    },
                                    {
                                        xtype: 'datecolumn',
                                        dataIndex: 'ObjectCreateDate',
                                        flex: 2,
                                        align: 'center',
                                        text: 'Дата загрузки',
                                        format: 'd.m.Y H:i:s'
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        flex: 2,
                                        dataIndex: 'UserName',
                                        text: 'Пользователь'

                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        flex: 2,
                                        dataIndex: 'ResponseCode',
                                        text: 'Код ответа'

                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        flex: 5,
                                        dataIndex: 'ResponseInfo',
                                        text: 'Текст ответа'

                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'FileName',
                                        flex: 3,
                                        text: 'Наименование файла'
                                    },
                                    {

                                        xtype: 'actioncolumn',
                                        flex: 1,
                                        align: 'center',
                                        text: 'Скачать файл',
                                        items: [
                                            {
                                                altText: 'Выгрузка',
                                                getClass: function (v, meta, rec) {
                                                    return 'icon-disk-black';
                                                },
                                                handler: function (gridview, row, column, btn, meta, record) {
                                                    me.downloadFile(gridview, record.get('FileName'), record.get('FileId'));
                                                }
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'b4deletecolumn'
                                    }
                                ]
                            },
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
                                            defaults: {
                                                margin: '2 0 2 0'
                                            },
                                            columns: 1,
                                            items: [
                                                {
                                                    xtype: 'b4updatebutton',
                                                    listeners: {
                                                        click: function (button) {
                                                            button.up('gridpanel').getStore().load();
                                                        }
                                                    }
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4pagingtoolbar',
                                    displayInfo: true,
                                    dock: 'bottom',
                                    store: loadedFileStore
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    //Скачать файл
    downloadFile: function (gridview, fileName, fileId) {
        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action('CheckFile', 'FileUpload'),
            params: { id: fileId },
            timeout: 999999
        }).next(function (response) {
            var result = Ext.decode(response.responseText);
            if (result.success) {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action(Ext.urlAppend('/FileUpload/Download?' + Ext.urlEncode({ id: fileId }), '_dc=' + (new Date().getTime())))
                });
            } else {
                B4.QuickMsg.msg('Предупреждение', "Файл не найден", 'warning');
            }
        }).error(function () {
            B4.QuickMsg.msg('Ошибка!', 'Не удалось загрузить файл', 'error');
        });
    },

    //скачать лог
    downloadLog: function (gridview, log) {
        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action('CheckLog', 'IncrementalData'),
            params: { log: log },
            timeout: 999999
        }).next(function (response) {
            var result = Ext.decode(response.responseText);
            if (result.success) {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframeLog',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action(Ext.urlAppend('/IncrementalData/DownloadLog?' + Ext.urlEncode({ log: log }), '_dc=' + (new Date().getTime())))
                });
            } else {
                B4.QuickMsg.msg('Предупреждение', "Файл не найден", 'warning');
            }
        }).error(function () {
            B4.QuickMsg.msg('Ошибка!', 'Не удалось загрузить файл', 'error');
        });
    }
});