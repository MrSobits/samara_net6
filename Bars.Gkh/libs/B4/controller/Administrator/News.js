Ext.define('B4.controller.Administrator.News', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.form.SelectWindow',
        'B4.aspects.Permission'
    ],

    models: ['Administrator.News', 'Administrator.NewsPermissions', 'Role'],

    stores: ['Administrator.News', 'Administrator.NewsPermissions', 'Role'],

    views: [
        'Administrator.Grid',
        'Administrator.EditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'Administrator.Grid',
    mainViewSelector: '#newsGrid',

    aspects: [
        {
            xtype: 'permissionaspect',
            event: 'afterrender',
            applyBy: function (component, allowed) {
                if (component)
                    component.setDisabled(!allowed);
            },
            permissions: [
                { name: 'News.Create', applyTo: 'b4addbutton', selector: '#newsGrid', applyOn: { event: this.event, selector: this.selector } },
                { name: 'News.Edit', applyTo: 'b4savebutton', selector: '#newsEditWindow', applyOn: { event: this.event, selector: this.selector } },
                {
                    name: 'News.Delete', applyTo: 'b4deletecolumn', selector: '#newsGrid', applyOn: { event: this.event, selector: this.selector },
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'News.Edit', applyTo: 'b4editcolumn', selector: '#newsGrid', applyOn: { event: this.event, selector: this.selector },
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'newsAspect',
            gridSelector: '#newsGrid',
            editFormSelector: '#newsEditWindow',
            storeName: 'Administrator.News',
            modelName: 'Administrator.News',
            editWindowView: 'Administrator.EditWindow',

            listeners: {
                'aftersetformdata': function (asp, rec) {
                    var me = Ext.apply(this, { rec: rec, asp: asp });
                    var permStore = asp.controller.getStore('Administrator.NewsPermissions');
                    permStore.on('beforeload', this.onPermissionsBeforeLoad, me);
                    permStore.on('datachanged', this.onPermissionsDataChanged, me);
                    permStore.load();
                }
            },

            onSaveSuccess: function (aspect, rec) {
                var permStore = aspect.controller.getStore('Administrator.NewsPermissions');

                if (permStore.getModifiedRecords().length == 0) {
                    permStore.load();
                    return;
                }

                permStore.sync({
                    failure: function () {
                        Ext.Msg.alert('Ошибка', 'Ошибка при сохранении прав доступа.');
                    },
                    success: function () {
                        permStore.load();
                    }
                });
            },

            onPermissionsBeforeLoad: function (store, operation, eOpts) {
                var recId = this.rec.get('Id');

                this.asp.getForm().down('#newsRoleGrid').setDisabled(recId == 0);

                if (recId == 0)
                    return false;

                var params = operation.params || {};
                params.id = recId;
                operation.params = params;
            },

            onPermissionsDataChanged: function (store) {
                var form = this.asp.getForm();
                var hint = form.down('#selectRolesHint');
                if (store.getCount() > 0)
                    hint.setText('Новость будет доступна только для указанных ролей:');
                else
                    hint.setText('Новость доступна всем. Выберите роли, для которых предназначена эта новость:');
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'newsRoleAspect',
            gridSelector: '#newsRoleGrid',
            editFormSelector: '#newsPermissionAddWindow',
            parentForm: '#newsEditWindow',
            storeName: 'Administrator.NewsPermissions',
            modelName: 'Administrator.NewsPermissions',
            getForm: function () {
                var asp = this,
                    win = asp.editWindow ||
                        Ext.create('B4.form.SelectWindow', {
                            name: 'NotifyPermission',
                            store: 'B4.store.Role',
                            textProperty: 'Name',
                            title: 'Выбор роли',
                            selectionMode: 'MULTI',
                            renderTo: B4.getBody().getActiveTab().getEl(),
                            ctxKey: asp.controller.getCurrentContextKey ? asp.controller.getCurrentContextKey() : ''
                        });
                asp.editWindow = win;
                return win;
            },

            editRecord: function () {
                this.getForm().performSelection(this.onRoleSelected(this));
            },

            onRoleSelected: function (scope) {
                var me = scope;
                return function (records) {
                    if (records) {
                        var perm = me.getModel();
                        var permStore = me.controller.getStore('Administrator.NewsPermissions');
                        var newsId = Ext.ComponentQuery.query(me.parentForm)[0].getRecord().get('Id');
                        for (var i in records) {
                            var roleId = records[i].Id;
                            if (permStore.findBy(function (r) { return r.get('Role') === roleId }) !== -1)
                                continue;

                            var rec = perm.create({ Role: roleId, News: newsId });
                            permStore.add(rec);
                        }
                    }
                };
            }
        }
    ],

    init: function () {
        this.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        me.getStore('Administrator.News').load();
        me.getStore('Role').load();
    }
});