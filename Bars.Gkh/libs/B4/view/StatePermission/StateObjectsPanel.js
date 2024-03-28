Ext.define('B4.view.StatePermission.StateObjectsPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.stateobjectspanel',

    requires: [
        'B4.store.Role',
        'B4.store.StatePermission',
        'B4.store.StatefulEntity',
        'B4.store.StateByType',
        'B4.form.ComboBox',
        'B4.ux.form.PermissionTreePanel'
    ],

    title: 'Настройка ограничений статусных объектов',

    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closable: true,

    roleStore: null,
    stateObjectsStore: null,

    initComponent: function () {
        var me = this,
            roleStore = me.roleStore || Ext.create('B4.store.Role'),
            stateObjectsStore = me.stateObjectsStore ||
                Ext.create('Ext.data.TreeStore', {
                    autoLoad: false,
                    proxy: {
                        type: 'ajax',
                        url: B4.Url.action('/State/GetObjectPermissions'),
                        reader: {
                            type: 'json'
                        }
                    }
                });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    margins: '8 0 6 6',
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'Role',
                            emptyText: 'Выберите роль',
                            editable: false,
                            valueField: 'Id',
                            displayField: 'Name',
                            width: 250,
                            labelWidth: 35,
                            fieldLabel: 'Роль',
                            store: roleStore,
                            queryMode: 'local',
                            triggerAction: 'all',
                            margin: '0 50 0 0'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    flex: 1,
                    items: [
                        {
                            xtype: 'permissiontreepanel',
                            name: 'Permissions',
                            flex: 1,
                            store: stateObjectsStore,
                            disabled: true,
                            tbar: [
                                { iconCls: 'icon-arrow-refresh', text: 'Обновить', name: 'Update' },
                                { iconCls: 'icon-disk', text: 'Сохранить', name: 'Save' },
                                { xtype: 'tbseparator' },
                                { iconCls: 'icon-accept', text: 'Отметить все', name: 'MarkAll' },
                                { iconCls: 'icon-decline', text: 'Снять все отметки', name: 'UnmarkAll' },
                                { xtype: 'tbseparator' },
                                { iconCls: 'icon-add', text: 'Копирование', name: 'CopyRole', tooltip: 'Копировать права из роли' },
                                {
                                    xtype: 'tbfill'
                                },
                                {
                                    xtype: 'b4searchfield',
                                    name: 'Search',
                                    typeAhead: false,
                                    hideLabel: true,
                                    emptyText: 'Поиск',
                                    margin: '0 10 0 0',
                                    width: 250
                                }
                            ],
                        }
                    ]
                }
            ],
        });
        me.callParent(arguments);
    }
});