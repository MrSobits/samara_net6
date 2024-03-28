Ext.define('B4.view.controllist.editwindow.ControlListQuestionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.grid.SelectFieldEditor',
        'B4.store.dict.ControlListTypicalQuestion',
        'B4.store.dict.ControlListTypicalAnswer'
    ],

    title: 'Вопросы проверочного листа',
    alias: 'widget.controllistquestiongrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.controllist.ControlListQuestion'),
            controlListTypicalQuestionsStore = Ext.create('B4.store.dict.ControlListTypicalQuestion'),
            controlListTypicalAnswersStore = Ext.create('B4.store.dict.ControlListTypicalAnswer');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'QuestionContent',
                    flex: 1,
                    text: 'Вопрос проверочного листа',
                    editor: {
                        xtype: 'textfield',
                        allowBlank: false
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypicalQuestion',
                    text: 'Типовой вопрос',
                    flex: 1,
                    renderer: function (val) {
                        if (val && val.Question) {
                            return val.Question;
                        }

                        return '';
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: controlListTypicalQuestionsStore,
                        modalWindow: true,
                        textProperty: 'Question',
                        editable: false,
                        isGetOnlyIdProperty: false,
                        columns: [
                            {
                                text: 'Вопрос проверочного листа',
                                dataIndex: 'Question',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            },
                            {
                                text: 'НПА',
                                dataIndex: 'NpaName',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            }
                        ]
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypicalAnswer',
                    text: 'Ответ на вопрос проверочного листа',
                    flex: 1,
                    renderer: function (val) {
                        if (val && val.Answer) {
                            return val.Answer;
                        }

                        return '';
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: controlListTypicalAnswersStore,
                        modalWindow: true,
                        textProperty: 'Answer',
                        editable: false,
                        isGetOnlyIdProperty: false,
                        columns: [
                            {
                                text: 'Ответ на вопрос проверочного листа',
                                dataIndex: 'Answer',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            }
                        ]
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })],
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
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept',
                                    actionName: 'save',
                                    name:'questionsSaveButton'
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