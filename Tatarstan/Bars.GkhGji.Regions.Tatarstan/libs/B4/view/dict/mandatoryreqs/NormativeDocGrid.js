Ext.define('B4.view.dict.mandatoryreqs.NormativeDocGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.MandatoryReqsNormativeDoc',
        'B4.store.dict.NormativeDoc',
        'B4.grid.SelectFieldEditor',
    ],

    alias: 'widget.mandatoryreqsnormativedocgrid',    

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.MandatoryReqsNormativeDoc'),
            normativeDocStore = Ext.create('B4.store.dict.NormativeDoc');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Npa',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' },
                    renderer: function (val) {
                        if (val && val.FullName) {
                                return val.FullName;
                        }
                        return '';
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        title: 'Выбор нормативно-правового документа',
                        name: 'NormativeDoc',
                        store: normativeDocStore,
                        textProperty: 'FullName',
                        editable: false,
                        isGetOnlyIdProperty: false,
                        flex: 1,
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                header: 'Наименование',
                                dataIndex: 'FullName',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            }
                        ]                        
                    },
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
                                    xtype: 'b4updatebutton',
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept',
                                    actionName: 'save',
                                    name: 'normativeDocSaveButton'
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
    }
});