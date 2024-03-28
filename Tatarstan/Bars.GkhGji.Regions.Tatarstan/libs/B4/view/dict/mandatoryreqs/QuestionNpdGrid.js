Ext.define('B4.view.dict.mandatoryreqs.QuestionNpdGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.ControlListTypicalQuestion'
    ],

    alias: 'widget.mandatoryreqsquestionnpdgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.ControlListTypicalQuestion'),
            selectFieldStore = Ext.create('B4.store.dict.ControlListTypicalQuestion');
        selectFieldStore.on('beforeload', me.onBeforeLoad, me);

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Question',
                    text: 'Вопрос',
                    flex: 1,
                    renderer: function (val) {
                        if (val && val.Question != undefined) {
                            return val.Question;
                        }

                        return val;
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: selectFieldStore,
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
                            column: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton',
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
                                    name: 'questionsSaveButton'
                                },
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
    },

    onBeforeLoad: function (store, operation) {
        //загружаем только записи без MandatoryRequirement
        operation.params.needWithoutMandatory = true;
    }
});