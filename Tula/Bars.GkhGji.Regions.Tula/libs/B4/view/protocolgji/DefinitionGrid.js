Ext.define('B4.view.protocolgji.DefinitionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeDefinitionProtocol'
    ],

    alias: 'widget.protocolgjiDefinitionGrid',
    title: 'Определения',
    store: 'protocolgji.Definition',
    itemId: 'protocolgjiDefinitionGrid',

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
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    flex: 1,
                    text: 'Номер документа'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ExecutionDate',
                    text: 'Дата исполнения',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'TimeStart',
                    text: 'Время начала',
                    format: 'H:i',
                    width: 100,
                    // Гавно-код связан с тем что умный chrome прибавляет к датам сдвиг по гринвичу
                    // А я так вообще побыстрому скопировал и все
                    renderer: function (val) {
                        if (Ext.isDate(val)) {
                            return Ext.Date.format(new Date(val), 'H:i');
                        } else {
                            if (val && val.indexOf('T') != -1) {
                                var partsDateTime = val.split('T');
                                if (partsDateTime.length == 2) {
                                    var partsHourMinute = partsDateTime[1].split(':');
                                    if (partsHourMinute.length >= 2) {
                                        return partsHourMinute[0] + ':' + partsHourMinute[1];
                                    }
                                }
                            }
                        }

                        return "";
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'TimeEnd',
                    text: 'Время окончания',
                    format: 'H:i',
                    width: 100,
                    // Гавно-код связан с тем что умный chrome прибавляет к датам сдвиг по гринвичу
                    // А я так вообще побыстрому скопировал и все
                    renderer: function (val) {
                        if (Ext.isDate(val)) {
                            return Ext.Date.format(new Date(val), 'H:i');
                        } else {
                            if (val && val.indexOf('T') != -1) {
                                var partsDateTime = val.split('T');
                                if (partsDateTime.length == 2) {
                                    var partsHourMinute = partsDateTime[1].split(':');
                                    if (partsHourMinute.length >= 2) {
                                        return partsHourMinute[0] + ':' + partsHourMinute[1];
                                    }
                                }
                            }
                        }

                        return "";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IssuedDefinition',
                    flex: 1,
                    text: 'ДЛ, вынесшее определение'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeDefinition',
                    flex: 1,
                    text: 'Тип определения',
                    renderer: function (val) { return B4.enums.TypeDefinitionProtocol.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileDescription',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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