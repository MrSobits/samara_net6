Ext.define('B4.view.version.ActualizationFileLoggingGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.actualizationfilelogginggrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',        
        'B4.store.version.VersionActualizeLog',
        'B4.enums.VersionActualizeType'
    ],

    title: 'Файловое логирование актуализации',
    closable: false,
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.version.VersionActualizeLog');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UserName',
                    flex: 1,
                    text: 'Пользователь',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    dataIndex: 'DateAction',
                    text: 'Дата действия',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ActualizeType',
                    flex: 1,
                    text: 'Тип актуализации',
                    renderer: function (val, meta, rec) {
                        var progCrName = rec.get('ProgramCrName');
                        if (progCrName && progCrName.length > 0) {
                            progCrName = ' (' + progCrName + ')';
                        } else {
                            progCrName = '';
                        }
                        return B4.enums.VersionActualizeType.displayRenderer(val) + progCrName;
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.VersionActualizeType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountActions',
                    flex: 1,
                    text: 'Количество выполненных действий',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LogFile',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});