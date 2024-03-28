Ext.define('B4.view.StatePermission.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.statepermissionpanel',

    requires: [
        'B4.store.Role',
        'B4.store.StatePermission',
        'B4.store.StatefulEntity',
        'B4.store.StateByType',
        'B4.form.ComboBox',
        'B4.ux.form.PermissionTreePanel'
    ],

    title: 'Настройка ограничений по статусам',

    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closable: true,

    roleStore: null,
    statePermissionStore: null,

    initComponent: function () {
        var me = this,
            stateStore = Ext.create('B4.store.StateByType'),
            roleStore = me.roleStore || Ext.create('B4.store.Role'),
            statePermissionStore = me.statePermissionStore || Ext.create('B4.store.StatePermission');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'Role',
                    editable: false,
                    fieldLabel: 'Роль',
                    store: roleStore,
                    pageSize: 30,
                    triggerAction: 'all',
                    valueField: 'Id',
                    displayField: 'Name',
                    maxWidth: 450,
                    margins: '10 5 0'
                },
                {
                    xtype: 'combobox',
                    name: 'ObjectType',
                    fieldLabel: 'Тип объекта',
                    valueField: 'TypeId',
                    displayField: 'Name',
                    editable: false,
                    queryMode: 'local',
                    triggerAction: 'all',
                    store: {
                        fields: [
                            { name: 'TypeId', useNull: false },
                            { name: 'Name' }
                        ]
                    },
                    maxWidth: 450,
                    margins: '5 5 0'
                },
                {
                    xtype: 'combobox',
                    name: 'ObjectState',
                    disabled: true,
                    fieldLabel: 'Статус',
                    queryMode: 'local',
                    editable: false,
                    store: stateStore,
                    valueField: 'Id',
                    displayField: 'Name',
                    maxWidth: 450,
                    margins: '5 5 0'
                },
                {
                    xtype: 'permissiontreepanel',
                    name: 'Permissions',
                    flex: 1,
                    store: statePermissionStore,
                    disabled: true,
                }
            ],
        });
        me.callParent(arguments);
    }
});