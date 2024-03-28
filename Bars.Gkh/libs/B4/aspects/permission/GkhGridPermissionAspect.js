Ext.define('B4.aspects.permission.GkhGridPermissionAspect', {
    extend: 'B4.aspects.Permission',
    alias: 'widget.gkhgridpermissionaspect',

    gridSelector: null,

    permissionPrefix: null,

    event: 'afterrender',

    permissions: [
        {
            name: 'Create',
            applyTo: 'b4addbutton',
            applyBy: function (component, allowed) {
                component.setDisabled(!allowed);
            }
        },
        {
            name: 'Delete',
            applyTo: 'b4deletecolumn',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ],

    /*переопределяем метод. 
    1.Грид с формой редактирования. Работает как и в стандартном аспекте
    2.Грид с навигационной панелью. При первом открытии срабатывает данные метод.
        Подписываемся на события и так как есть селектор панели применяем сразу пермишины.
        При повторном открытие пермишины берутся из подписок.
    */
    init: function (controller) {
        this.controller = controller;

        var result = this.addPrefix();

        this.loadPermissions(result)
            .next(function (response) {
                var me = this,
                    grants = Ext.decode(response.responseText),
                    ev = {};

                for (var n = result.length, i = 0; i < n; ++i) {
                    var permission = result[i],
                        action = {},
                        applyOn = { event: this.event, selector: this.gridSelector },
                        applyBy = Ext.isEmpty(permission.applyBy) ? this.applyBy : permission.applyBy,
                        applyTo = this.gridSelector + ' ' + permission.applyTo,
                        event = this.event;

                    //вешаемся на события
                    ev[event] = Ext.Function.pass(me.applyPermission, [Boolean(grants[i]), null, applyBy, applyTo], me);
                    action[applyOn.selector] = ev;
                    me.controller.control(action);

                    var cmp = Ext.ComponentQuery.query(applyTo);
                    // Если компонент, к которому необходимо применить ограничение, уже создан, то сразу применяем
                    if (cmp && cmp[0]) {
                        me.applyPermission(Boolean(grants[i]), null, applyBy, applyTo);
                    }
                }
            }, this);
    },

    loadPermissions: function (permissions) {
        var me = this;
        return B4.Ajax.request({
            url: B4.Url.action('/Permission/GetPermissions'),
            params: {
                permissions: Ext.encode(me.collectPermissions(permissions))
            }
        });
    },

    /**
    * Собираем все ограничения для отправки на сервер
    */
    collectPermissions: function (permissions) {
        var result = [];
        for (var p = permissions, n = p.length, i = 0; i < n; ++i) {
            result.push(p[i].name);
        }
        return result;
    },

    //добавляем префикс к имени пермишенов
    addPrefix: function () {
        var prefix = this.permissionPrefix;
        var result = Ext.clone(this.permissions);

        for (var n = result.length, i = 0; i < n; i++) {
            var name = result[i].name;
            name = prefix + '.' + name;
            result[i].name = name;
        }

        return result;
    }
});