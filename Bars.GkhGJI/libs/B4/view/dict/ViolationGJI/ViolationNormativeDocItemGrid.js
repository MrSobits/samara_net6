Ext.define('B4.view.dict.violationgji.ViolationNormativeDocItemGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.TreeSelectField'
    ],
    alias: 'widget.violationNormativeDocItemGrid',
    title: 'Пункты нормативно-правового документа',
    store: 'dict.ViolationNormativeDocItemGji',
    itemId: 'violationNormativeDocItemGrid',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NormativeDocItemName',
                    flex: 1,
                    text: 'Пункт НПД',
                    editor:
                    {
                        xtype: 'treeselectfield',
                        name: 'NormativeDocItem',
                        titleWindow: 'Выбор пункта НПД',
                        store: 'B4.store.dict.NormativeDocItemTreeStore',
                        itemId: 'normativeDocItemSelectField',
                        allowBlank: false,
                        onSelectItem: function () {
                            var me = this,
                                tree = me.treePanel,
                                selection = tree.getSelectionModel();

                            if (selection) {
                                if (!tree.getSelectionModel().getSelection()) {
                                    return;
                                }
                                var data = tree.getSelectionModel().getSelection()[0].data;
                                if (!data.leaf) {
                                    return;
                                }

                                var checkedNodes = tree.getChecked();
                                me.dataSelected = checkedNodes;
                                for (var i = 0; i < checkedNodes.length; i++) {
                                    checkedNodes[i].set("checked", false);
                                }

                                me.setValue(data);
                            }

                            me.onSelectWindowClose();
                        },
                        onSearchWork: function (t, e) {
                            var me = this,
                                value = me.selectWindow.down('textfield[name="tfSearch"]').getValue();

                            if (e.keyCode == 13) {
                                me.treePanel.getStore().load({
                                        params: {
                                            workName: value
                                        }
                                    });
                            }
                        }
                    },
                    filter:
                    {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NormativeDocName',
                    flex: 1,
                    text: 'Наименование НПД',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    dataIndex: 'ViolationStructure',
                    flex: 1,
                    text: 'Состав правонарушения',
                    editor: 'textfield',
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
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                }),
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
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