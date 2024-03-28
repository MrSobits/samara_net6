Ext.define('B4.view.dict.ControlListTypicalQuestion.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.grid.SelectFieldEditor',
        'B4.store.dict.NormativeDoc',
        'B4.store.dict.ControlListTypicalQuestion',
        'B4.store.dict.MandatoryReqs'
    ],

    title: 'Типовые вопросы проверочного листа',
    alias: 'widget.controllisttypicalquestiongrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.ControlListTypicalQuestion'),
            mandatoryReqStore = Ext.create('B4.store.dict.MandatoryReqs'),
            normativeeDocsStore = Ext.create('B4.store.dict.NormativeDoc');
        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Question',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Вопрос проверочного листа',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NormativeDoc',
                    text: 'НПА',
                    flex: 1,
                    renderer: function (val) {
                        if (val && val.Name) {
                            return val.Name;
                        }
                        return '';
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: normativeeDocsStore,
                        modalWindow: true,
                        textProperty: 'Name',
                        editable: false,
                        isGetOnlyIdProperty: false,
                        columns: [
                            {
                                text: 'НПА',
                                dataIndex: 'Name',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            }
                        ]
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MandatoryRequirement',
                    text: 'Обязательное требование',
                    flex: 1,
                    renderer: function (val) {
                        if (val && val.MandratoryReqName) {
                            return val.MandratoryReqName;
                        }
                        return '';
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: mandatoryReqStore,
                        modalWindow: true,
                        textProperty: 'MandratoryReqName',
                        editable: false,
                        isGetOnlyIdProperty: false,
                        columns: [
                            {
                                text: 'Обязательное требование',
                                dataIndex: 'MandratoryReqName',
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
                    store: store
                }
            ]
        });

        me.callParent(arguments);
    }
});