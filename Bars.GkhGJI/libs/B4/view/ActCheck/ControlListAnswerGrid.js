Ext.define('B4.view.actcheck.ControlListAnswerGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.actcheckcontrollistanswergrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.YesNoNotApplicable',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
    ],

    title: 'Проверочный лист',
    store: 'actcheck.ControlListAnswer',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [

                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Question',
                    flex: 1,
                    editor: 'textfield',
                    text: 'Вопрос'
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NpdName',
                    flex: 1,
                    editor: 'textfield',
                    text: 'Реквизиты НПД'
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YesNoNotApplicable',
                    flex: 0.5,
                    text: 'Ответ',
                    renderer: function (val) {
                        return B4.enums.YesNoNotApplicable.displayRenderer(val);
                    },
                    editor: {
                        xtype: 'b4combobox',
                        valueField: 'Value',
                        displayField: 'Display',
                        items: B4.enums.YesNoNotApplicable.getItems(),
                        editable: false
                    }
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    editor: 'textfield',
                    text: 'Примечание'
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    itemId: 'actcheckcontrollistanswerupdateButton',
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'actcheckcontrollistanswerSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'clistAnswers',
                                    text: 'Ответить на вопросы КЛ',
                                    iconCls: 'icon-accept'
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