Ext.define('B4.view.import.chesimport.AnalysisPanel', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.enums.regop.FileType',
        'B4.enums.CheckState',
        'B4.view.Control.GkhFileColumn',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Delete',
        'B4.view.Control.GkhButtonPrint',
        'B4.store.import.chesimport.ChesImportFile'
    ],

    title: 'Разбор файла',
    alias: 'widget.chesimportanalysisgrid',

    columnLines: true,
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.import.chesimport.ChesImportFile');

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'FileType',
                    text: 'Тип файла',
                    enumName: 'B4.enums.regop.FileType',
                    flex: 1,
                    filter: false
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'CheckState',
                    text: 'Состояние проверки',
                    enumName: 'B4.enums.CheckState',
                    flex: 1,
                    filter: false,
                    renderer: function(val) {
                        if (val != 0) {
                            return B4.enums.CheckState.displayRenderer(val);
                        }
                    }
                },
                {
                    xtype: 'gkhfilecolumn',
                    dataIndex: 'File',
                    text: 'Отчет по проверке',
                    flex: 1,
                    iconCls: 'icon-arrow-right'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                                    xtype: 'button',
                                    text: 'Загрузить данные в систему',
                                    iconCls: 'icon-application-go',
                                    action: 'import'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Запустить проверку',
                                    action: 'runCheck'
                                }
                            ]
                        }
                    ]
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});