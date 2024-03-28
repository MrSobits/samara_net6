Ext.define('B4.aspects.FieldRequirementAspect', {
    extend: 'B4.base.Aspect',

    alias: 'widget.requirementaspect',

    event: 'afterrender',
    /**
    * @cfg {Object[]} requirements
    * Массив объектов-требований обязательности
    *   {
    *       name: 'B4.Permission'
    *       applyTo: '#selector',
    *       applyOn: { event: 'show', selector: '#windowSelector' },
    *   }
    */
    requirements: [],

    /**
    * @cfg {Function} applyBy
    * Функция, применяющая требование обязательности для каждого объекта из коллекции requirements, если у него не задана своя функция applyBy
    */
    applyBy: function (component, required) {
        if (component) {
            component.permissionRequired = required;
            component.allowBlank = !required;

            if (component.getValue && Ext.isEmpty(component.getValue())) {
                
                if (!component.allowBlank) {
                    component.markInvalid();
                } else {
                    component.clearInvalid();
                }
            }
        }
    },

    /**
    * @cfg {Object} applyOn
    * Объект вида { event: 'show', selector: '#windowSelector' }, определяющий момент, когда применяется требование обязательности
    * Используется для тех объектов из коллекции requirements, у которых не задан свой объект applyOn
    */
    applyOn: null,

    /**
    * @cfg {String/Function} applyTo
    * Селектор компонента или функция, позволяющая получить компонент, к которому применяется требование обязательности
    * Используется для тех объектов из коллекции requirements, у которых не задан свой объект applyTo
    */
    applyTo: null,

    viewSelector: null,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents(
            'apply',
            'beforeapply'
        );
    },

    onAfterRender: function () {
        // Выключаем аспект, если не передан нужный конфиг
        if ((!this.requirements || this.requirements.length == 0)) {
            return;
        }

        this.loadRequirements()
            .next(function (response) {
                var me = this,
                    grants = Ext.decode(response.responseText),
                    ev = {};

                for (var n = this.requirements.length, i = 0; i < n; ++i) {
                    var requirement = this.requirements[i],
                        action = {},
                        applyOn = { event: this.event, selector: requirement.selector },
                        applyBy = Ext.isEmpty(requirement.applyBy) ? this.applyBy : requirement.applyBy,
                        applyTo = requirement.selector + ' ' + requirement.applyTo,
                        event = Ext.isEmpty(requirement.event) ? this.event : requirement.event;


                    var cmp = Ext.ComponentQuery.query(applyTo);
                    // Если компонент, к которому необходимо применить требование обязательности, уже создан, то сразу применяем, иначе вешаемся на событие.
                    if (cmp && cmp[0]) {
                        me.applyRequirements(Boolean(grants[i]), null, applyBy, applyTo);
                    } else {
                        ev[event] = Ext.Function.pass(me.applyRequirements, [Boolean(grants[i]), null, applyBy, applyTo], me);

                        action[applyOn.selector] = ev;

                        me.controller.control(action);
                    }
                }

                me.afterSetRequirements();

            }, this);
    },

    /**
    * Инициализация аспекта
    * @param {B4.base.Controller} controller Контроллер
    */
    init: function (controller) {
        var me = this,
            conf = {};

        me.controller = controller;

        conf[me.viewSelector || controller.mainViewSelector] = {
            afterrender: Ext.bind(me.onAfterRender, me)
        };

        controller.control(conf);
    },

    /**
    * По каждому требованию обязательности получаем с сервера true или false
    * @return {Deferred} deffered  
    */
    loadRequirements: function () {
        var me = this;
        return B4.Ajax.request({
            url: B4.Url.action('/FieldsRequirement/GetRequirements'),
            params: {
                requirements: Ext.encode(me.collectRequirements())
            }
        });
    },

    /**
    * Собираем все требования обязательности для отправки на сервер
    */
    collectRequirements: function () {
        var result = [];
        for (var p = this.requirements, n = p.length, i = 0; i < n; ++i) {
            result.push(p[i].name);
        }
        return result;
    },

    /**
    * Действия после применения требований обязательности
    */
    afterSetRequirements: function () {
        return;
    },

    /**
    * Применяем требования обязательности
    * @param {Boolean} allowed Флаг: разрешить/запретить
    * @param {Object} options Options
    * @param {Function} applyBy Функция, применяющая ограничения
    * @param {String/Function} applyTo Селектор или функция, позволяющие получить объект
    */
    applyRequirements: function (required, options, applyBy, applyTo) {
        if (required === null)
            return;

        if (this.fireEvent('beforeapply', this, required, options) === false)
            return;

        var restrictedObject = this.getObject(options, applyTo);
        applyBy.call(this, restrictedObject, required);

        this.fireEvent('apply', this, required, options, restrictedObject);
    },

    /**
    * Получаем объект
    * @param {Object} options Options
    * @param {String/Function} applyTo Селектор или функция, позволяющие получить объект
    */
    getObject: function (options, applyTo) {
        if (Ext.isFunction(applyTo)) {
            return applyTo.call(this.applyToScope || this);
        } else {
            var cmp = this.componentQueryAll(applyTo);
            if (cmp && cmp[0]) {
                return cmp[0];
            } else {
                return null;
            }
        }
    }
});