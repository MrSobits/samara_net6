Ext.define('B4.view.importexport.incrementalimport.Panel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Update',
        'B4.enums.TypeStatus',
        'B4.enums.TypeImportFormat',
        'B4.enums.TypeIncImportFormat',
        'B4.form.EnumCombo'
    ],

    title: 'Импорт инкрементальных данных',
    alias: 'widget.incrementalimportpanel',
    layout: 'anchor',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,

    initComponent: function () {
        var me = this,
            loadedFileStore = Ext.create('B4.store.importexport.IncrementalImport');

        Ext.applyIf(me, {
            items: [
            {
                xtype: 'container',
                itemId: 'ctnText',
                style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите файл с данными для загрузки. Допустимый тип файла: *.zip</span>'
            },
            {
                xtype: 'form',
                margin: '0 10 5 10',
                border: false,
                bodyStyle: Gkh.bodyStyle,
                itemId: 'importForm',
                layout: {
                    type: 'hbox'
                },
                items: [
                    {
                        xtype: 'b4enumcombo',
                        margin: '0 10 5 10',
                        fieldLabel: 'Формат загрузки',
                        enumName: 'B4.enums.TypeIncImportFormat',
                        allowBlank: false,
                        name: 'Format',
                        editable: false,
                        labelWidth: 100,
                        width: 300,
                    },
                    {
                        xtype: 'filefield',
                        margin: '0 20 5 10',
                        flex: 1,
                        name: 'IncFile',
                        fieldLabel: 'Выберите файлы',
                        labelAlign: 'right',
                        labelWidth: 150,
                        msgTarget: 'side',
                        allowBlank: false,
                        buttonText: 'Выбрать файлы',
                        regex: /^.*\.(zip|ZIP|j|J)$/,
                        regexText: 'Возможна загрузка только zip-файлов',
                        listeners: {
                            afterrender: function (field) {
                                field.fileInputEl.set({
                                    multiple: 'multiple'
                                });
                            }
                        }
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
                    columns: {
                        defaults: {
                            sortable: false,
                            hideable: false
                        },
                        items: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'FileName',
                            flex: 1,
                            text: 'Наименование файла',
                            filter: {
                                xtype: 'textfield'
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            width: 210,
                            dataIndex: 'TypeStatus',
                            text: 'Статус загрузки',
                            enumName: 'B4.enums.TypeStatus',
                            filter: true
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'ObjectCreateDate',
                            width: 170,
                            align: 'center',
                            text: 'Дата загрузки',
                            format: 'd.m.Y H:i:s',
                            filter: {
                                xtype: 'datefield'
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            text: 'Скачать файл',
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    width: 55,
                                    align: 'center',
                                    text: 'Лог',
                                    dataIndex: 'LogId',
                                    renderer: function (v) {
                                            return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v) + '" target="_blank" style="color: black">Скачать</a>') : '';
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    width: 55,
                                    align: 'center',
                                    text: 'Загрузка',
                                    dataIndex: 'FileId',
                                    renderer: function(v) {
                                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v) + '" target="_blank" style="color: black">Скачать</a>') : '';
                                    }
                                }
                            ]
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
    }
});