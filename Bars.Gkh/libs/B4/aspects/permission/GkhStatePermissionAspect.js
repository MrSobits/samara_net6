Ext.define('B4.aspects.permission.GkhStatePermissionAspect', {
    extend: 'B4.aspects.StatePermission',
    alias: 'widget.gkhstatepermissionaspect',

    event: 'afterrender',

    //можно переопределить, если нужно проставлять пермишены при другом событии
    setPermissionEvent: 'aftersetpaneldata',

    permissions: [],

    applyBy: function(component, allowed) {
        if (component) {
            component.editPermissionAllowed = allowed;
            component.setDisabled(!allowed);
        }
    },

    setVisible: function(component, allowed, needSetAllowBlank) {
        if (component) {
            component.viewPermissionAllowed = allowed;
            component.setVisible(allowed);

            // Корректировка обязательности поля - убрать признак обязательности для скрытых полей
            if (needSetAllowBlank === true)
            {
                component.allowBlank = !allowed;
            }
        }
    },

    editFormAspectName: null,

    // если true, пермишены с постфиксом _View скрывают компонент
    applyByPostfix: false,

    mainViewSelector: null,

    /*переопределяем метод.
    1.Грид с формой редактирования. Работает как и в стандартном аспекте
    2.Грид с навигационной панелью. При первом открытии срабатывает данные метод.
    Подписываемся на события и так как есть селектор панели применяем сразу пермишины.
    При повторном открытие пермишины берутся из подписок.
    */
    setPermissions: function(aspect, rec) {
        this.loadPerms(rec);
    },

    setPermissionsByRecord: function(rec) {
        this.loadPerms(rec);
    },

    getGrants: function(grants) {
        //по странному стечению обстоятельств возвращает массив массивов, а не просто массив
        if (grants && grants[0]) {
            return grants[0];
        }
        return grants;
    },

    loadPerms: function (rec) {
        this.loadPermissions(rec)
            .next(function(response) {
                var me = this,
                    grants = Ext.decode(response.responseText),
                    ev = {};

                grants = me.getGrants(grants);

                Ext.each(me.permissions, function (permission, i) {
                    var action = {},
                        selector = Ext.isEmpty(permission.selector) ? me.mainViewSelector : permission.selector,
                        applyOn = { event: me.event, selector: selector },
                        applyTo = selector + ' ' + permission.applyTo,
                        event = Ext.isEmpty(applyOn.event) ? 'afterrender' : applyOn.event,
                        cmp = me.componentQueryAll(applyTo),
                        applyBy;

                    if (Ext.isEmpty(permission.applyBy)) {
                        if (me.applyByPostfix) {
                            applyBy = permission.name.lastIndexOf('_View') !== -1 ? me.setVisible : me.applyBy;
                        } else {
                            applyBy = me.applyBy;
                        }
                    }
                    else {
                        applyBy = permission.applyBy;
                    }

                    if (cmp && cmp.length > 0) {
                        me.applyPermission(Boolean(grants[i]), null, applyBy, applyTo);
                        Ext.each(cmp, function(c) {
                            c.permissionApplied = true;
                        });
                    } else {
                        //вешаемся на события
                        ev[event] = function(c) {
                            me.applyPermission(Boolean(grants[i]), null, applyBy, applyTo);
                            c.permissionApplied = true;
                        };
                        action[applyTo] = ev;
                        me.controller.control(action);
                    }
                });

                me.afterSetRequirements(rec);

            }, this);
    },

    /**
    * Действия после применения требований обязательности
    */
    afterSetRequirements: function(rec) {
        return;
    },

    init: function(controller) {
        var me = this,
            editFormAspect;
        me.controller = controller;

        // Выключаем аспект, если не передан нужный конфиг
        if ((!me.permissions || me.permissions.length == 0)) {
            return;
        }

        me.preDisable();

        if (me.editFormAspectName) {
            editFormAspect = controller.getAspect(me.editFormAspectName);
            editFormAspect.on(me.setPermissionEvent, me.setPermissions, me);
        }
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
    },

    applyPermission: function (allowed, options, applyBy, applyTo) {
        var me = this;
        if (allowed === null) {
            return;
        }

        if (me.fireEvent('beforeapply', me, allowed, options) === false) {
            return;
        }

        var restrictedObjects = me.getRestrictedObjects(options, applyTo);
        Ext.each(restrictedObjects, function (restrictedObject) {
            applyBy.call(me, restrictedObject, allowed);
        });

        me.fireEvent('apply', me, allowed, options);
    },

    getRestrictedObjects: function (options, applyTo) {
        var me = this;
        if (Ext.isFunction(applyTo)) {
            return applyTo.call(me.applyToScope || me);
        } else {
            return me.componentQueryAll(applyTo);
        }
    }
});