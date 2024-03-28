Ext.define('B4.aspects.permission.GkhPermissionAspect', {
    extend: 'B4.aspects.Permission',
    alias: 'widget.gkhpermissionaspect',

    permissions: [],

    applyBy: function(component, allowed) {
        if (component) {
            component.editPermissionAllowed = allowed;
            component.setDisabled(!allowed);
        }
    },

    setDisabled: function(component, allowed) {
        if (component) {
            component.editPermissionAllowed = allowed;
            component.setDisabled(!allowed);
        }
    },

    setVisible: function(component, allowed) {
        if (component) {
            component.viewPermissionAllowed = allowed;
            component.setVisible(allowed);
        }
    },

    setAllowBlank: function(component, allowed) {
        if (component) {
            component.allowBlank = !allowed;
            //component.validate();
        }
    },

    permissionGrant: null,

    /*переопределяем метод. 
    1.Грид с формой редактирования. Работает как и в стандартном аспекте
    2.Грид с навигационной панелью. При первом открытии срабатывает данные метод.
    Подписываемся на события и так как есть селектор панели применяем сразу пермишины.
    При повторном открытие пермишины берутся из подписок.
    */
    init: function(controller) {
        var self = this;
        self.controller = controller;

        self.preDisable();

        self.loadPermissions()
            .next(function(response) {
                var me = this,
                    grants = Ext.decode(response.responseText);

                Ext.each(me.permissions, function (permission, i) {
                    var action = {},
                        ev = {},
                        applyOn = Ext.isEmpty(permission.applyOn) ? me.applyOn : permission.applyOn,
                        applyBy = Ext.isEmpty(permission.applyBy) ? me.applyBy : permission.applyBy,
                        applyTo = Ext.isEmpty(permission.applyTo) ? permission.selector : permission.selector + ' ' + permission.applyTo,
                        event = applyOn === null || Ext.isEmpty(applyOn.event) ? 'afterrender' : applyOn.event;

                    if (me.permissionGrant) {
                        me.permissionGrant[applyTo] = Boolean(grants[i]);
                    }

                    //вешаемся на события
                    ev[event] = function(c) {
                        me.applyPermission(Boolean(grants[i]), null, applyBy, applyTo);
                        c.permissionApplied = true;
                    };

                    action[applyTo] = ev;

                    me.controller.control(action);

                    // Если компонент, к которому необходимо применить ограничение, уже создан, то сразу применяем
                    var cmp = applyTo ? me.componentQueryAll(applyTo) : null;
                    if (cmp && cmp.length > 0) {
                        me.applyPermission(Boolean(grants[i]), null, applyBy, applyTo);
                        Ext.each(cmp, function (c) {
                            c.permissionApplied = true;
                        });
                    }
                });
            }, self);
    },

    preDisable: function() {
        var me = this;

        Ext.each(me.permissions, function(permission) {
            var applyBy = Ext.isEmpty(permission.applyBy) ? me.applyBy : permission.applyBy,
                applyTo = permission.selector + ' ' + permission.applyTo,
                cmp = me.componentQueryAll(applyTo),
                action = {};

            if (cmp && cmp.length > 0) {
                Ext.each(cmp, function(c) {
                        if (!c.permissionApplied) {
                            applyBy.call(me, c, false);
                        }
                    }
                );
            } else {
                action[applyTo] = {
                    'render': {
                        fn: function(c) {
                            if (!c.permissionApplied) {
                                applyBy.call(me, c, false);
                            }
                        },
                        scope: me
                    }
                };
                me.controller.control(action);
            }
        });
    }
});