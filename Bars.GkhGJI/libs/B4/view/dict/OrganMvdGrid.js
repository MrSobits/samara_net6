Ext.define('B4.view.dict.OrganMvdGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.organmvdgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Органы МВД',
    store: 'dict.OrganMvd',
    closable: true,

    initComponent: function () {
        var me = this;
        me.storeMo = Ext.create('B4.base.Store', {
            fields: ['Id', 'Name'],
            proxy: {
                type: 'b4proxy',
                controllerName: 'Municipality',
                listAction: 'ListMoAreaWithoutPaging'
            },
            autoLoad: true
        });

        me.storeMo.load();
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Municipality',
                //    flex: 1,
                //    text: 'Муниципальное образование',
                //    renderer: function(val) {
                //        var record = this.storeMo.findRecord('Id', val);
                //        return record != null ? record.get('Name') : val;
                //    },
                //    editor: {
                //        xtype: 'b4combobox',
                //        operand: CondExpr.operands.eq,
                //        hideLabel: true,
                //        editable: false,
                //        valueField: 'Id',
                //        displayField: 'Name',
                //        store: this.storeMo
                //    }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
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
                            columns: 4,
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
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});