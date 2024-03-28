Ext.define('B4.view.dict.controlobjectkind.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.ControlObjectKind',
        'B4.store.dict.ControlObjectType',
        'B4.store.dict.ControlType',
        'B4.grid.SelectFieldEditor'
    ],

    title: 'Виды объекта контроля',
    alias: 'widget.controlobjectkind',
    closable: true,

    initComponent: function () {
        let me = this,
            store = Ext.create('B4.store.dict.ControlObjectKind');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1000,
                        allowBlank: false
                    },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlObjectType',
                    text: 'Тип объекта контроля',
                    renderer: function (val){
                        if(val && val.Name){
                            return val.Name;
                        }
                        return '';
                    },
                    editor:{
                        xtype: 'b4selectfieldeditor',
                        store: 'B4.store.dict.ControlObjectType',
                        modalWindow: true,
                        textProperty: 'Name',
                        allowBlank: false,
                        editable: false,
                        isGetOnlyIdProperty: false,
                        columns:[
                            {
                                text: 'Тип объекта контроля',
                                dataIndex: 'Name',
                                flex: 1,
                                filter: {xtype: 'textfield'}
                            }
                        ]
                    },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlType',
                    text: 'Вид контроля',
                    renderer: function (val){
                        if(val && val.Name){
                            return val.Name;
                        }
                        return '';
                    },
                    editor:{
                        xtype: 'b4selectfieldeditor',
                        store: 'B4.store.dict.ControlType',
                        modalWindow: true,
                        textProperty: 'Name',
                        allowBlank: false,
                        editable: false,
                        isGetOnlyIdProperty: false,
                        columns:[
                            {
                                text: 'Вид контроля',
                                dataIndex: 'Name',
                                flex: 1,
                                filter: {xtype: 'textfield'}
                            }
                        ]
                    },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErvkId',
                    text: 'Идентификатор в ЕРВК',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 36
                    },
                    flex: 1
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
                    layout: {
                        type: 'vbox'
                    },
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