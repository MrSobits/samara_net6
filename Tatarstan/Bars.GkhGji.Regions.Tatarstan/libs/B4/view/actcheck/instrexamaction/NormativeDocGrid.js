Ext.define('B4.view.actcheck.instrexamaction.NormativeDocGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.grid.SelectFieldEditor',
        'B4.store.dict.NormativeDoc'
    ],

    alias: 'widget.instrexamactionnormativedocgrid',
    title: 'Нормативно-правовые акты',

    // Приписка к itemId
    itemIdInnerMessage: '',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            itemId: 'instrExamActionNormativeDocGrid' + me.itemIdInnerMessage,
            store: 'actcheck.InstrExamActionNormativeDoc' + me.itemIdInnerMessage,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NormativeDoc',
                    text: 'Наименование',
                    flex: 1,
                    renderer: function (value) {
                        return value?.FullName;
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: 'B4.store.dict.NormativeDoc',
                        textProperty: 'FullName',
                        modalWindow: true,
                        isGetOnlyIdProperty: false,
                        editable: false
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            viewConfig: {
                loadMask: true
            },
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 2,
                    pluginId: 'cellEditing'
                })
            ],
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
                                    itemId: 'saveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});