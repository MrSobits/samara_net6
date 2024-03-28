Ext.define('B4.view.administration.localadminrolesettings.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.localadminrolesettingspanel',

    requires: [
        'B4.store.administration.LocalAdminRole',
        'B4.store.administration.LocalAdminRolePermission',
        'B4.store.administration.LocalAdminStatePermission',
        'B4.model.Role',
        'B4.form.ComboBox',
        'B4.form.TreeSelectField',
        'B4.view.Permission.Panel',
        'B4.view.StatePermission.Panel',
        'B4.view.StatePermission.StateObjectsPanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Настройка локальных администраторов',
    closable: true,

    bodyStyle: Gkh.bodyStyle,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.administration.LocalAdminRole');
        store.load();

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'breadcrumbs',
                    data: {
                        text: me.title
                    },
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    border: false,
                    defaults: {
                        bodyStyle: Gkh.bodyStyle,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                    },
                    items: [
                        {
                            title: 'Настройка ограничений',
                            border: false,
                            name: 'RolePermissionTab',
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'LocalAdmin',
                                    editable: false,
                                    labelWidth: 160,
                                    fieldLabel: 'Локальный администратор',
                                    store: store,
                                    queryMode: 'local',
                                    triggerAction: 'all',
                                    maxWidth: 450,
                                    padding: 5
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Настройка ограничений доступа для роли',
                                    flex: 1,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'rolepermissionpanel',
                                            closable: false,
                                            header: false,
                                            border: false,
                                            flex: 1,
                                            roleStore: Ext.create('Ext.data.Store', {
                                                model: 'B4.model.Role',
                                                proxy: 'memory'
                                            }),
                                            rolePermissionStore: Ext.create('B4.store.administration.LocalAdminRolePermission')
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            title: 'Настройка ограничений по статусам',
                            border: false,
                            name: 'StatePermissionTab',
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'LocalAdmin',
                                    fieldLabel: 'Локальный администратор',
                                    store: store,
                                    queryMode: 'local',
                                    triggerAction: 'all',
                                    labelWidth: 160,
                                    maxWidth: 450,
                                    padding: 5,
                                    editable: false,
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Настройка ограничений статусных объектов',
                                    flex: 1,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'stateobjectspanel',
                                            closable: false,
                                            header: false,
                                            border: false,
                                            flex: 1,
                                            roleStore: Ext.create('Ext.data.Store', {
                                                model: 'B4.model.Role',
                                                proxy: 'memory'
                                            })
                                        },
                                    ]
                                },
                            ]
                        },
                        //{
                        //    title: 'Настройка ограничений по статусам',
                        //    border: false,
                        //    name: 'StatePermissionTab',
                        //    items: [
                        //        {
                        //            xtype: 'b4combobox',
                        //            name: 'LocalAdmin',
                        //            editable: false,
                        //            labelWidth: 160,
                        //            fieldLabel: 'Локальный администратор',
                        //            store: store,
                        //            queryMode: 'local',
                        //            triggerAction: 'all',
                        //            maxWidth: 450,
                        //            padding: 5
                        //        },
                        //        {
                        //            xtype: 'fieldset',
                        //            title: 'Настрока ограничений по статусам',
                        //            flex: 1,
                        //            layout: {
                        //                type: 'vbox',
                        //                align: 'stretch'
                        //            },
                        //            items: [
                        //                {
                        //                    xtype: 'statepermissionpanel',
                        //                    closable: false,
                        //                    header: false,
                        //                    border: false,
                        //                    flex: 1,
                        //                    roleStore: Ext.create('Ext.data.Store', {
                        //                        model: 'B4.model.Role',
                        //                        proxy: 'memory'
                        //                    }),
                        //                    statePermissionStore: Ext.create('B4.store.administration.LocalAdminStatePermission')
                        //                }
                        //            ]
                        //        },
                        //    ]
                        //}
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});