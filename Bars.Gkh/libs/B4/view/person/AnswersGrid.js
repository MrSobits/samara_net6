Ext.define('B4.view.person.AnswersGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
    ],

    title: 'Результаты экзамена',
    store: 'person.QExamQuestion',
    alias: 'widget.qexamanswersgrid',
    closable: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
               
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    flex: 1,
                    text: 'Номер'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'QuestionText',
                    flex: 6,
                    text: 'Вопрос'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'QualifyTestQuestionsAnswers',
                    flex: 6,
                    text: 'Ответ'
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
                            xtype: 'b4updatebutton'
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