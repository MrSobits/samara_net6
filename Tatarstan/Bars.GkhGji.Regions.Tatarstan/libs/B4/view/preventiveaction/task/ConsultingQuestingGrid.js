Ext.define('B4.view.preventiveaction.task.ConsultingQuestingGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',
        'Ext.grid.plugin.CellEditing',
        
        'B4.store.preventiveaction.PreventiveActionTaskConsultingQuestion'
    ],

    alias: 'widget.consultingquestiongrid',
    itemId: 'consultingQuestionGrid',
    title: 'Вопросы для консультирования',
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.preventiveaction.PreventiveActionTaskConsultingQuestion');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Question',
                    text: 'Вопрос',
                    flex: 1,
                    editor: { xtype: 'textfield'} 
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Answer',
                    text: 'Ответ',
                    flex: 1,
                    editor: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlledPerson',
                    text: 'Подконтрольное лицо',
                    flex: 0.5,
                    editor: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
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
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'consultingQuestionSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
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