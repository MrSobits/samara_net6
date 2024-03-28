Ext.define('B4.view.dict.surveysubject.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.enums.SurveySubjectRelevance'
    ],

    title: 'Предметы проверки',
    store: 'dict.SurveySubject',
    alias: 'widget.surveysubjectgrid',
    closable: true,

    initComponent: function() {
        var me = this,
            relevanceStore = B4.enums.SurveySubjectRelevance.getStore();

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 2000
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Formulation',
                    flex: 1,
                    text: 'Формулировка для плана проверок',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Relevance',
                    flex: 1,
                    text: 'Актуальность формулировки',
                    renderer: function (value) {
                        var record = relevanceStore.findRecord('Value', value);
                        if (record) {
                            return record.get('Display');
                        }
                        return '';
                    },
                    editor: {
                        xtype: 'b4combobox',
                        items: B4.enums.SurveySubjectRelevance.getItems(),
                        displayField: 'Display',
                        valueField: 'Value',
                        allowBlank: false,
                        editable: false,
                        listeners: {
                            select: {
                                fn: function (cmp) {
                                    cmp.ownerCt.completeEdit();
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GisGkhCode',
                    flex: 1,
                    text: 'Код ГИС ЖКХ',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 255
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GisGkhGuid',
                    flex: 1,
                    text: 'Guid ГИС ЖКХ',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 36
                    },
                    filter: {
                        xtype: 'textfield'
                    }
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    dock: 'bottom',
                    displayInfo: true,
                    store: this.store
                }
            ]
        });

        me.callParent(arguments);
    }
});