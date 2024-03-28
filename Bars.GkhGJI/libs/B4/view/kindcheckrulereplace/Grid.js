Ext.define('B4.view.kindcheckrulereplace.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        
        'B4.enums.TypeCheck'
    ],

    title: 'Правила проставления вида проверки',
    store: 'KindCheckRuleReplace',
    alias: 'widget.kindcheckrulereplacegrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код вида проверки',
                    editor: {
                        xtype: 'b4combobox',
                        displayField: 'Value',
                        valueField: 'Value',
                        items: B4.enums.TypeCheck.getItems(),
                        editable: false
                    }
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
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});