Ext.define('B4.aspects.permission.GkhEditPermissionAspect', {
    extend: 'B4.aspects.Permission',
    alias: 'widget.gkheditpermissionaspect',

    event: 'afterrender',

    editWindowSelector: null,

    /*по умолчанию для каждого контрола применяется этот метод
    * если нужно скрывать/показывать контрол то нужно перекрыть этот метод 
    */
    applyBy: function (component, allowed) {
        if (component)
            component.setDisabled(!allowed);
    },

    /*переопределяем метод. 
    1.Грид с формой редактирования. Работает как и в стандартном аспекте
    2.Грид с навигационной панелью. При первом открытии срабатывает данные метод.
        Подписываемся на события и так как есть селектор панели применяем сразу пермишины.
        При повторном открытие пермишины берутся из подписок.*/
    init: function (controller) {
        this.controller = controller;

        this.loadPermissions()
            .next(function (response) {
                var me = this,
                    grants = Ext.decode(response.responseText),
                    ev = {};

                for (var n = this.permissions.length, i = 0; i < n; ++i) {
                    var permission = this.permissions[i],
                        action = {},
                        applyOn = { event: this.event, selector: this.editWindowSelector },
                        applyBy = Ext.isEmpty(permission.applyBy) ? this.applyBy : permission.applyBy,
                        applyTo = this.editWindowSelector + ' ' + permission.applyTo,
                        event = this.event;

                    //вешаемся на событие
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
    }
});