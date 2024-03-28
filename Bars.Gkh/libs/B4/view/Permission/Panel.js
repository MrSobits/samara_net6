Ext.define('B4.view.Permission.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.rolepermissionpanel',

    requires: [
        'B4.store.Role',
        'B4.store.Permission',
        'B4.ux.form.PermissionTreePanel'
    ],

    title: 'Настройка ограничений',
    closable: true,
    bodyStyle: Gkh.bodyStyle,

    roleStore: null,
    rolePermissionStore: null,

    initComponent: function() {
        var me = this,
            roleStore = me.roleStore || Ext.create('B4.store.Role'),
            rolePermissionStore = me.rolePermissionStore || Ext.create('B4.store.Permission');

        Ext.applyIf(me, {
            columnLines: true,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
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
                            width: 300,
                            labelWidth: 35,
                            fieldLabel: 'Роль',
                            store: roleStore,
                            pageSize: 30,
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
                            store: rolePermissionStore,
                            disabled: true,
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});