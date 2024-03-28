Ext.define('B4.view.objectcr.DeletedObjectCrGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.deletedobjectcrgrid',
    requires: [
        'B4.ux.button.Update',
        'Ext.ux.CheckColumn',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',
        'B4.model.dict.ProgramCr',
        'B4.store.dict.ProgramCrObj',
        'B4.Url'
    ],

    store: 'DeletedObjectCr',
    closable: false,
    features: [{
        ftype: 'summary'
    }],
    enableColumnHide: true,
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            selModel: Ext.create('Ext.selection.CheckboxModel', {}),
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProgramNum',
                    text: '№',
                    width: 50,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProgramCrName',
                    text: 'Программа',
                    flex: 3,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/ProgramCr/ListWithoutPaging?forObjCr=true'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальный район',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Settlement',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObjName',
                    text: 'Адрес',
                    flex: 3,
                    filter: { xtype: 'textfield' }
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
                            xtype: 'buttongroup',
                            columns: 5,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Восстановить',
                                    textAlign: 'left',
                                    itemId: 'btnRecover'
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
