Ext.define('B4.view.dict.ControlType.InspectorPositionsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.InspectorPositions',
        'B4.store.dict.ControlTypeInspectorPos',
        'Ext.grid.plugin.CellEditing',
        'B4.grid.SelectFieldEditor',
        'Ext.ux.CheckColumn'
    ],

    title: 'Должности инспекторов',
    alias: 'widget.controltypeinspectorposgrid',

    initComponent: function (){
        var me = this,
            store = Ext.create('B4.store.dict.ControlTypeInspectorPos'),
            insPosStore = Ext.create('B4.store.dict.InspectorPositions');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectorPosition',
                    text: 'Наименование',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    renderer: function (value) {
                        return value?.Name;
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: insPosStore,
                        title: 'Выбор должности инспектора',
                        textProperty: 'Name',
                        editable: false,
                        isGetOnlyIdProperty: false,
                        flex: 1,
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                header: 'Наименование',
                                dataIndex: 'Name',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            }
                        ]
                    },
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'IsIssuer',
                    width: 130,
                    text: 'Вынесший документ'
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'IsMember',
                    width: 170,
                    text: 'Участвующий в мероприятии'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErvkId',
                    text: 'Идентификатор записи',
                    flex: 1,
                    editor: {
                        xtype: 'textfield',
                        maxLength: 36
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
                    clicksToEdit: 2,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    layout: 'vbox',
                    dock: 'top',
                    padding: 5,
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
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept',
                                    actionName: 'save',
                                    name: 'inspPosGridSaveButton'
                                },
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'InspectorPosGuid',
                            fieldLabel: 'Идентификатор справочника',
                            readOnly: true,
                            padding: '5 0',
                            labelWidth: 160,
                            width: 450
                        },
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