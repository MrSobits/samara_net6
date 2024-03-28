Ext.define('B4.view.appealcits.AppealCitsQuestionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [        
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Enum',

        'B4.enums.QuestionType'
    ],

    title: 'Виды вопросов',
    alias: 'widget.appealcitsquestiongrid',
    columnLines: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.appealcits.AppealCitsQuestion');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    text: 'Наименование вида вопроса',
                    dataIndex: 'QuestionKind',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    header: 'Наименование типа вопроса',
                    dataIndex: 'QuestionType',
                    enumName: 'B4.enums.QuestionType',
                    filter: true,
                    flex: 3,
                    sortable: false
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
                    store: store,
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});