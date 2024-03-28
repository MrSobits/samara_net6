/**
 * @class B4.routing.Router
 * Mixin, отвечающий за работу с роутингом. Подписывается на изменение 
 */
Ext.define('B4.routing.Router', {

    requires: [
        'Ext.util.History',
        'Ext.util.HashMap',
        'B4.routing.Route',
        'B4.routing.State',
        'B4.routing.controller.Routing',
        'B4.Ajax',
        'B4.Url',
        'B4.routing.Event',
        'B4.routing.Action'
    ],

    config: {
        /**
         * @cfg {Ext.util.MixedCollection} routes массив {@link B4.routing.Route route}.
         */
        routes: new Ext.util.HashMap(),

        /**
         * @cfg {Ext.util.History} объект {@link Ext.util.History history} для работы с историей.
         */
        history: null,

        /**
         * @cfg {B4.routing.State} {@link B4.routing.State состояние системы}.
         */
        state: null,

        /**
         * @cfg {Boolean} Параметр, отвечающий за восстановление состояния системы
         * Если значение true, то при каждом запуске приложение начинает с чистого состояния.
         */
        ignoreSate: false
    },

    /**
     * @private
     * @property
     * Очередь для восстановления состояния. Формируется при вызове {B4.routing.Router.restoreState} 
     * и изменяется при вызове {B4.routing.Router.dispatch}
     */
    restoreQueue: [],

    /**
     * @private
     * @property
     * true, если идет восстановление состояния.
     */
    isRestoring: false,

    /**
     * @method onClassMixedIn
     */
    onClassMixedIn: function (x) {
        this.__overrideConstructor(x);
        this.__overrideLaunch(x);
    },

    statics: {
        /**
         * @method __overrideConstructor
         */
        __overrideConstructor: function (x) {
            var constrFn = x.prototype.constructor,
                constrFnScope = constrFn.scope;

            x.prototype.constructor = function (config) {
                var me = this;
                me.initConfig(arguments);
                me.setHistory(Ext.History);
                me.setState(B4.routing.State);
                constrFn.apply(constrFnScope || this, arguments);
            };
        },

        /**
         * @method __overrideLaunch
         */
        __overrideLaunch: function (x) {
            var launchFn = x.prototype.launch,
                launchFnScope = launchFn.scope;

            x.prototype.launch = function () {
                var me = this;
                Ext.History.init();
                Ext.History.on('change', this.onHistoryChange, this);
                launchFn.apply(launchFnScope || this, arguments);

                Ext.Ajax.request({
                    url: B4.Url.action('LoadRouteMap', 'ClientRouteMap'),
                    success: function (rsp) {
                        var data;
                        try {
                            // ожидаемый формат: {data:{InitializeControllers: true/false, Routes: {route1: {config}...}}}
                            data = Ext.JSON.decode(rsp.responseText);
                        } catch (e) {
                            console.log("Ошибка получения направлений: " + (e.message ? e.message : e));
                        }

                        data = data ? data.data : {};
                        var routes = data.Routes || {};
                        Ext.iterate(routes, function (k, route) {
                            if (!routes.hasOwnProperty(k)) return;
                            var conditions = {};
                            Ext.each(route.Conditions, function(kvp) {
                                conditions[kvp.Key] = kvp.Value
                            });
                            me.connect(route.Route, {
                                controller: route.Controller,
                                action: route.Action,
                                conditions: conditions
                            });
                        });

                        if (data.InitializeControllers == true) {
                            if (Ext.isEmpty(me.routes) || !me.routes.length) { return; }

                            me.routes.each(function (k, v) {
                                try {
                                    if (Ext.isEmpty(v.controller)) { return; }
                                    var ctrl = me.getController(v.controller);
                                    if (ctrl.isInitialized == true) { return; }
                                    ctrl.init();
                                } catch (e) {
                                    Ext.Error.raise('Ошибка инициализации контроллера: ' + v.controller + ' - ' + (e.message ? e.message : e));
                                }
                            });
                        }

                        if (!Ext.isEmpty(me.defaultRoute)) {
                            me.connect('defaultUrl', me.defaultRoute);
                        }

                        if (!me.ignoreSate) {
                            me.restoreState();
                        } else {
                            me.getState().getStore().removeAll();
                            me.getState().getStore().sync();
                        }
                    },
                    failure: function (rsp) {
                        var e = 'Ошибка загрузки направлений';
                        if (rsp && rsp.responseText) {
                            var er = Ext.JSON.decode(rsp.responseText, true);
                            e = er && er.message ? er.message : rsp.responseText;
                        }

                        Ext.Error.raise(e);
                    }
                });

                // Регистрация роутов, добавленных вручную
                me.afterLaunch.apply(launchFnScope || this, arguments);
            };
        },

        normalizeRoute: function (routeStr) {
            return routeStr.lastIndexOf('/') === routeStr.length - 1 ? routeStr : routeStr + '/';
        }
    },

    /**
     * @method connect
     * @param {String} url темплейт для роута, н-р, 'controller/action/{id}/'.
     * @param {Object} params параметры для создания экземпляра {@link B4.routing.Route Route}.
     * Создает экземпляр {@link B4.routing.Route Route} и добавляет его в {@link B4.routing.Route#routes массив routes Router'а}.
     * @return {B4.routing.Route} Добавленный {@link B4.routing.Route route}.
     */
    connect: function (url, params) {
        var route;
        // Проверка на регистрацию private action (action, который начинается со знака _)
        params.action = params.action || 'index';

        if (!/\/$/.test(url)) {
            url = url + "/";
        }

        if (params.action.indexOf('_') == 0) {
            Ext.log({
                msg: 'Регистрация route с private action',
                level: 'warn',
                dump: params
            });
            return null;
        }

        params = Ext.apply({ url: url }, params || {});
        route = Ext.create('B4.routing.Route', params);
        this.getRoutes().add(route.matcherRegex, route);
        return route;
    },

    /**
     * @method recognize
     * Распознает url и вызывает соответствующий этому url метод контроллера.
     * @param {String} url Url для распознавания.
     * @return {Object/undefined} результат вызова соответствующего метода или undefined, если таковой не нашелся.
     */
    recognize: function (url) {
        var routes = this.getRoutes(),
            result,
            searchUrl = url || 'defaultUrl';

        routes.each(function (key) {
            result = routes.get(key).recognize(searchUrl);
            if (result !== false) {
                return false;
            }
        });

        return result;
    },

    recognizeByAction: function (action, controller, url) {
        var me = this,
            routes = me.getRoutes(),
            result, regexpSource, matcher, match;

        routes.each(function (key, route) {
            if (route.getAction() === action && route.getController() === controller) {
                result = route;
                return false;
            }
        });

        if (result) {
            regexpSource = result.matcherRegex.source;
            matcher = new RegExp(regexpSource.substring(0, regexpSource.length - 1));
            match = B4.routing.Router.normalizeRoute(url).match(matcher);
            return match && match[0];
        }

        return null;
    },


    /**
     * @method clear
     * @private
     * Очистка {@link B4.routing.Router.routes}
     */
    clear: function () {
        this.setRoutes([]);
    },

    /**
     * @method redirectTo
     * @public
     * @param {String} url
     * Меняет url в браузере
     */
    redirectTo: function (url) {
        url = this.getState().getActiveChild(url);
        this.getHistory().add(url);
    },

    /**
     * @method redirectToFrom
     */
    redirectToFrom: function (toUrl, fromUrl) {
        fromUrl = fromUrl || this.currentToken();
        this.redirectTo(fromUrl + toUrl);
    },

    /**
     * @method forward
     * @private
     * @param {String} url вызываемый url.
     * @param {Boolean} addToHistory false, если не надо изменять адресную строку в браузере.
     * @param {Object} extraParams экстра-параметры, если необходимо разово из другого контроллера пробросить параметры
     */
    forward: function (url, addToHistory, extraParams) {
        if (Ext.data && Ext.data.Model && url instanceof Ext.data.Model) {
            var record = url;
            url = record.toUrl();
        }

        var decoded = this.recognize(url);

        if (decoded) {
            decoded.url = url;
            if (record) {
                decoded.data = {};
                decoded.data.record = record;
            }
            this.dispatch(decoded, addToHistory, extraParams);
        }
    },

    /**
     * @method dispatch
     * @param {Object} action config для создания {@link B4.routing.Action Action}, вызов которого произойдет.
     * @param {Boolean} addToHistory false, если не надо изменять адресную строку в браузере.
     * @param {Object} extraParams экстра-параметры, если необходимо разово из другого контроллера пробросить параметры
     */
    dispatch: function (action, addToHistory, extraParams) {
        var me = this;
        action = action || {};
        Ext.applyIf(action, {
            application: me
        });

        action = Ext.create('B4.routing.Action', action);

        if (action) {
            var controller = action.getController();

            if (controller) {
                if (addToHistory !== false) {
                    me.getHistory().setHash(action.getUrl(), true);
                }

                me.getState().add(action.getUrl());

                if (extraParams) {
                    Ext.apply(action.params || (action.params = {}), extraParams);
                }

                controller.execute(action);
            }
        }
    },

    /**
     * @method back
     */
    back: function () {
        this.getHistory().back();
    },

    /**
     * @method getRoutingController
     */
    getRoutingController: function () {
        var controller = this.getController('B4.controller.Routing');
        if (!controller.isInitialized) {
            controller.init(this);
        }
        return controller;
    },

    /**
     * @method registerView
     * регистрируется на закрытие view
     */
    registerView: function (view) {
        if (!view.urlToken) {
            view.urlToken = this.currentToken() || '';
        }
        this.getRoutingController()._registerView(view);
    },

    /**
     * @method unRegisterView
     */
    unRegisterView: function (view) {
        this.getRoutingController()._unRegisterView(view);
    },

    /**
     * @method viewIsActivated
     */
    viewIsActivated: function (view) {
        var url, state = this.getState(), history = this.getHistory();
        if (view.urlToken) {
            url = state.getActiveChild(view.urlToken);
            state.activate(state.findByRoute(url));
        } else {
            url = '';
        }
        history.add(url, false);
    },

    /**
     * @method viewIsDeactivated
     */
    viewIsDeactivated: function (view) {
        var me = this,
            viewUrlToken = view.urlToken || view.ctxKey;
        
        me.unRegisterView(view);
        me.getState().remove(viewUrlToken);
        me.redirectTo(me.getState().getActiveChild('root'));
    },

    /**
     * @method currentToken
     */
    currentToken: function () {
        return this.getHistory().getHash();
    },


    /**
     * @method restoreState
     * @private
     * Восстановление состояния.
     */
    restoreState: function () {
        var me = this,
            redirectUrl,
            queue,
            historyToken = Ext.History.getToken(),
            tokenQueue = [],
            stateStore = me.getState().getStore(),
            toRemove = [],
            parentRoute;

        if (!Ext.isEmpty(historyToken) &&
            (historyToken.indexOf('B4.controller') < 0)) {
            tokenQueue.push(historyToken);
            parentRoute = me.getParentRoute(historyToken);
            while (parentRoute && parentRoute != 'root') {
                tokenQueue.push(parentRoute);
                parentRoute = me.getParentRoute(parentRoute);
            }

            stateStore.getRootNode().cascadeBy(function (node) {
                if (!Ext.Array.contains(tokenQueue, node.get('route'))) {
                    toRemove.push(node);
                }
            });

            Ext.each(toRemove, function (item) {
                item.remove();
            });
        } else {
            stateStore.getRootNode().removeAll();
        }

        if (tokenQueue.length > 0) {
            Ext.each(tokenQueue.reverse(), function(token) {
                me.getState().add(B4.routing.Router.normalizeRoute(token));
            });
            stateStore.sync();
        }

        queue = me.getState().linearize();
        me.restoreQueue = queue;

        if (queue.length > 0) {
            me.isRestoring = true;

            while (queue.length > 0) {
                redirectUrl = queue.shift();
                me.forward(redirectUrl, false);
            }

            me.isRestoring = false;
        }
    },

    /**
     * @method onHistoryChange
     * Вызывается после изменения url.
     * @param {String} token
     * Строка в url после знака '#'. Если url = http://somehost.ru/#controller/action/{id}?&arg1=jbhjk
     * то token = mycontroller/myFunc&arg1=jbhjk
     * @private
     */
    onHistoryChange: function (token) {
        var me = this,
            parentStack = [],
            parent;

        // #warning костыль для работы промежуточной версии
        if (me.onHistoryChangeOld) {
            me.onHistoryChangeOld(token);
        }

        parent = me.getParentRoute(token);
        if (parent) {
            while (parent != 'root') {
                parentStack.push(parent);
                parent = me.getParentRoute(parent);
            }
        }
        Ext.each(parentStack.reverse(), function (item) {
            if (!me.getState().isActiveRoute(item)) {
                me.forward(item, false);
            }
        });

        me.forward(token, false);
    },

    /**
     * @method getRouter
     */
    getRouter: function () {
        return this;
    },

    /**
     * @method getChildRoutes
     */
    getChildRoutes: function (token) {
        var root = this.getState().getStore().getRootNode(),
            node,
            childs = [];
        node = root.findChildBy(function (item) {
            return item.get('route') == token;
        }, this, true);

        if (node) {
            Ext.each(node.childNodes, function (item) {
                childs.push(item.get('route'));
            });
        }
        return childs;
    },

    /**
     * @method getParentRoute
     */


    getParentRoute: function (token) {
        var me = this, routes = me.routes, regexps = [], parentPrefix, matched, cut;
        if (!token || token === 'root') {
            return 'root';
        }

        token = B4.routing.Router.normalizeRoute(token);
        cut = token.lastIndexOf('/', token.lastIndexOf('/') - 1);

        parentPrefix = B4.routing.Router.normalizeRoute(token.substring(0, cut > 0 ? cut : 0));

        routes.each(function (k) {
            var idx = k.lastIndexOf('/');
            if (idx >= 0) {
                regexps.push(k.substring(1, idx));
            }
        });

        matched = Ext.Array.filter(regexps, function (r) {
            return new RegExp(r).test(parentPrefix);
        });

        if (matched.length > 0) {
            return parentPrefix;
        }
        return me.getParentRoute(parentPrefix);
    },

    /**
     * @method deployView
     */
    deployView: function (view, key, data) {
        key = key || 'default';

        var me = this,
            controller = me.controllers.findBy(function (ctrl) {
                var result = false;
                if (Ext.isFunction(ctrl.canDeployView)) {
                    result = ctrl.canDeployView.call(ctrl, key, view, data);
                }
                return result;
            });

        if (Ext.isEmpty(controller)) {
            Ext.Error.raise('Не удалось обнаружить контроллер для ключа: ' + key);
            return;
        }
        if (!view.urlToken) {
            view.urlToken = me.currentToken() || '';
        }
        controller.deployView.call(controller, key, view, data);
        me.getRouter().registerView(view);
    }
});