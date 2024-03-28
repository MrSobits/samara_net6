Ext.define('B4.view.diagnostic.DiagnosticResultGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.diagnostic.DiagnosticResult',
        'B4.enums.DiagnosticResultState'
    ],

    alias: 'widget.diagnosticresultgrid',
    title: 'Результаты диагностики',
    store: 'diagnostic.DiagnosticResult',
    closable: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    width: 100,
                    flex: 1,
                    sortable: false,
                    text: 'Тип диагностики',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Message',
                    flex: 3,
                    sortable: false,
                    text: 'Сообщение',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'State',
                    width: 100,
                    flex: 1,
                    sortable:false,
                    text: 'Состояние',
                    renderer: function (val) {
                        return B4.enums.DiagnosticResultState.displayRenderer(val);
                    }
                }
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
                            columns: 2,
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});