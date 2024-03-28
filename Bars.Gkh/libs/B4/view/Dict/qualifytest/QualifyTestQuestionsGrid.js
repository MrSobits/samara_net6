Ext.define('B4.view.dict.qualifytest.QualifyTestQuestionsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.enums.YesNoNotSet',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Вопросы квалификационного экзамена',
    store: 'dict.qualifytest.QualifyTestQuestions',
    alias: 'widget.qtestdictquestionsgrid',
    closable: true,

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
                     dataIndex: 'Code',
                     flex: 0.5,
                     text: 'Номер',
                     filter: {
                         xtype: 'textfield'
                     }
                 },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'Question',
                     flex: 2,
                     text: 'Вопрос',
                     filter: {
                         xtype: 'textfield'
                     }
                 },
                 {
                   xtype: 'gridcolumn',
                   dataIndex: 'IsActual',
                   flex: 0.5,
                   text: 'Актуальный',
                   renderer: function (val) {
                       return B4.enums.YesNoNotSet.displayRenderer(val);
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
                            columns: 3,
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