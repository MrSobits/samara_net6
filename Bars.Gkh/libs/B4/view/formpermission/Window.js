Ext.define('B4.view.formpermission.Window', {
    extend: 'Ext.window.Window',
    alias: 'widget.formpermissionwindow',
    width: 850,
    height: 600,
    autoDestroy: true,
    closeAction: 'destroy',
    overflowY: 'auto',
    overflowX: 'hidden',
    layout: {
        type: 'fit',
        align: 'stretch'
    },
    modal: true,
    title: 'Настройка прав доступа',
    requires: [
    'B4.view.formpermission.Grid',
    'B4.ux.button.Save',
    'B4.ux.button.Update',
    'B4.ux.button.Close'
    ],
    initComponent: function() {
        var me = this,
            typeStore = Ext.create('B4.base.Store', {
                idProperty: 'TypeId',
                fields: ['TypeId', 'Name'],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'FormPermission',
                    listAction: 'GetEntityTypes'
                },
                autoLoad: false
            });

        me.relayEvents(typeStore, ['beforeload', 'load'], 'typeStore.');

        Ext.applyIf(me,
        {
            items: [
                {
                    xtype: 'formpermissiongrid'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    handler: function(btn) {
                                        btn.up('window').close();
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    padding: '5',
                    defaults: {
                        padding: '0 5'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'Role',
                            emptyText: 'Выберите роль',
                            editable: false,
                            displayField: 'Name',
                            width: 250,
                            labelWidth: 35,
                            fieldLabel: 'Роль',
                            pageSize: 25,
                            store: Ext.create('B4.store.Role'),
                            valueField: 'Id'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'Stateful',
                            boxLabel: 'по статусам',
                            hidden: true
                        },
                        {
                            xtype: 'combobox',
                            name: 'EntityType',
                            width: 250,
                            labelWidth: 70,
                            labelAlign: 'right',
                            fieldLabel: 'Тип объекта',
                            valueField: 'TypeId',
                            displayField: 'Name',
                            editable: false,
                            store: typeStore,
                            hidden: true,
                            queryMode: 'local'
                        },
                        {
                            xtype: 'combobox',
                            name: 'State',
                            width: 200,
                            labelWidth: 50,
                            labelAlign: 'right',
                            fieldLabel: 'Статус',
                            editable: false,
                            store: Ext.create('B4.store.StateByType'),
                            valueField: 'Id',
                            displayField: 'Name',
                            hidden: true,
                            disabled: true,
                            queryMode: 'local'
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            disabled: true,
                            name: 'selectButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    action: 'selectAll',
                                    text: 'Разрешить все',
                                    iconCls: 'icon-basket-add'
                                },
                                {
                                    xtype: 'button',
                                    action: 'deselectAll',
                                    text: 'Запретить все',
                                    iconCls: 'icon-delete'
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