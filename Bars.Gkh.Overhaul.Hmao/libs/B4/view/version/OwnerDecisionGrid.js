Ext.define('B4.view.version.OwnerDecisionGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.versionownerdecisiongrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],
  
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.version.ChangeYearOwnerDecision');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,

            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObject',
                    flex: 1,
                    text: 'Объект общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StructuralElement',
                    flex: 1,
                    text: 'Конструктивный элемент',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OldYear',
                    text: 'Старый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NewYear',
                    text: 'Новый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    renderer: function (value) {
                        return value
                            ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + value.Id) + '" target="_blank" style="color: black">Скачать</a>')
                            : '';
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
                            columns: 5,
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        click: function(button) {
                                            button.up('grid').getStore().load();
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});