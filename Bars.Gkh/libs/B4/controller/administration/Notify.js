Ext.define('B4.controller.administration.Notify', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.form.SelectWindow',
        'B4.model.administration.notify.Message',
        'B4.model.administration.notify.Permission',
        'B4.view.administration.notify.Grid',
        'B4.view.administration.notify.EditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'administration.notify.Grid',
    mainViewSelector: 'notifygrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'notifygrid'
        }
    ],

    allowPermissionEdit: false,
    allowMessageView: false,

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Administration.Notify.Create', applyTo: 'b4addbutton', selector: 'notifygrid' },
                { name: 'Administration.Notify.Edit', applyTo: 'b4savebutton', selector: 'notifyeditwindow' },
                { name: 'Administration.Notify.Edit', applyTo: 'b4addbutton', selector: 'notifypermissiongrid' },
                {
                    name: 'Administration.Notify.Edit', applyTo: 'b4deletecolumn', selector: 'notifypermissiongrid',
                    applyBy: function (component, allowed) {
                        this.controller.allowPermissionEdit = allowed;
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'Administration.Notify.Edit', applyTo: 'b4editcolumn', selector: 'notifygrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'Administration.Notify.Delete', applyTo: 'b4deletecolumn', selector: 'notifygrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'Administration.Notify.View', selector: 'notifygrid',
                    applyBy: function (component, allowed) {
                        this.controller.allowMessageView = allowed;
                    }
                },
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'notifyAspect',
            gridSelector: 'notifygrid',
            editFormSelector: 'notifyeditwindow',
            modelName: 'administration.notify.Message',
            editWindowView: 'administration.notify.EditWindow',
            listeners: {
                beforerowaction: function(asp) {
                    return asp.controller.allowMessageView;
                },
                validate: function (asp) {
                    var record = asp.getForm().getForm().getRecord(),
                        startDateField = asp.getForm().down('[name=StartDate]'),
                        endDateField = asp.getForm().down('[name=EndDate]'),
                        textField = asp.getForm().down('[name=Text]'),
                        startDate = record.get('StartDate') || new Date(),
                        endDate = record.get('EndDate') || new Date(),
                        text = record.get('Text') || '';

                    if (startDate > endDate) {
                        startDateField.markInvalid('Некорректно задан период актуальности сообщения');
                        endDateField.markInvalid('Некорректно задан период актуальности сообщения');
                        return false;
                    }

                    if (text.length === 0) {
                        Ext.Msg.alert('Ошибка сохранения', 'Не задан текст сообщения');
                        return false;
                    }

                    return true;
                },
                aftersetformdata: function (asp, record) {
                    var me = asp.controller,
                        messageId = record.getId(),
                        hint = asp.getForm().down('label[name=Hint]'),
                        permissionStore = asp.getForm().down('notifypermissiongrid').getStore(),
                        statsStore = asp.getForm().down('notifystatsgrid').getStore();

                    permissionStore.on('beforeload', function(store, operation) {
                        operation.params.messageId = messageId;
                    });
                    permissionStore.on('datachanged', function (store) {
                        if (store.getCount() > 0)
                            hint.setText('Сообщение получат пользователи указанных ролей:');
                        else
                            hint.setText('Сообщение получат все пользователи. Выберите роли, для которых предназначено это сообщение');
                    });
                    statsStore.on('beforeload', function (store, operation) {
                        operation.params.messageId = messageId;
                    });

                    permissionStore.load();
                    statsStore.load();
                }
            },

            onSaveSuccess: function (asp, rec) {
                var form = asp.getForm(),
                    permissionStore = form.down('notifypermissiongrid').getStore();

                if (permissionStore.getModifiedRecords().length != 0) {
                    permissionStore.sync({
                        failure: function () {
                            Ext.Msg.alert('Ошибка', 'Ошибка при сохранении прав доступа.');
                        }
                    });
                }

                form.close();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'permissionAspect',
            gridSelector: 'notifypermissiongrid',
            editFormSelector: 'notifypermissiongrid b4selectwindow[name=NotifyPermission]',
            parentForm: 'notifyeditwindow',
            modelName: 'administration.notify.Permission',
            listeners: {
                beforerowaction: function(asp) {
                    return asp.controller.allowPermissionEdit;
                }
            },
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
                var asp = this;
                asp.getForm().performSelection(asp.onRoleSelected(asp));
            },

            onRoleSelected: function (asp) {
                return function (records) {
                    if (records) {
                        var perm = asp.getModel();
                        var permStore = asp.getGrid().getStore();
                        var messageId = Ext.ComponentQuery.query(asp.parentForm)[0].getRecord().get('Id');
                        for (var i in records) {
                            var roleId = records[i].Id,
                                roleName = records[i].Name;

                            if (permStore.findBy(function (r) { return r.get('Role') === roleId }) !== -1)
                                continue;

                            var rec = perm.create({ Role: roleId, Message: messageId, RoleName: roleName });
                            permStore.add(rec);
                        }
                    }
                };
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'notifygrid': {
                    'afterrender': {
                        fn: me.getViewData,
                        scope: me
                    }
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);
    },

    getViewData: function (view) {
        view.getStore().load();
    },
});