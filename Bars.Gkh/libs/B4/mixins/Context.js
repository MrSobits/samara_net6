/**
 * @mixin
 * Mixin добавляющий функциональность контекста контроллеру.
 * При этом экземпляр контроллера остается один, а обработка событий происходит в контексте инициатора.
 * В качестве инициатора события могуть быть действия на визальных компонентах и изменение адресной строки.
 * Для работы с контекстом определено понятие ключ контекста (ctxKey) - свойство содержащее значние ключа контекста.
 */
Ext.define('B4.mixins.Context', {
    contextCollection: null,

    /**
     * @method containsContextKey
     */
    containsContextKey: function (ctxKey) {
        return this.contextCollection.containsKey(ctxKey);
    },

    /**
     * @method getCurrentContextKey
     */
    getCurrentContextKey: function () {
        var ctxKey = this.application.getRouter().currentToken() || 'root',
            index = ctxKey.indexOf('?');
        return ctxKey.substring(0, index === -1 ? undefined : index);
    },

    /**
     * @method onClassMixedIn
     */
    onClassMixedIn: function (x) {
        this.__init(x);
        this.__getRef(x);
    },

    getContext: function (target) {
        var ctx,
            ctxKey = Ext.isEmpty(target)
                ? this.getCurrentContextKey()
                : B4.mixins.Context.getViewContextKey(target);

        if (Ext.isEmpty(ctxKey)) {
            return this;
        }

        if (this.containsContextKey(ctxKey)) {
            ctx = this.contextCollection.get(ctxKey);
        } else {
            ctx = Ext.apply({ ctxKey: ctxKey }, this);
            this.contextCollection.add(ctxKey, ctx);
        }

        return ctx;
    },

    /**
    * Получение кнтекстно-зависимого значения
    */
    getContextValue: function (target, key, defaultValue) {
        var ctx;
        if (Ext.isString(target)) {
            ctx = this.getContext(null);
            key = target;
        } else {
            ctx = this.getContext(target || {});
        }

        return (ctx || {})[key] || defaultValue;
    },

    setContextValue: function (target, key, value) {
        var ctx;
        if (Ext.isString(target)) {
            ctx = this.getContext(null);
            value = key;
            key = target;
        } else {
            ctx = this.getContext(target || {});
        }

        ctx[key] = value;
    },
    /**
     * @method bindContext
     */
    bindContext: function (cmp) {
        var me = this,
            ctrlName = me.self.getName(),
            actionName = me.bindContext.caller.$name,
            key = me.getCurrentContextKey();

        if (Ext.isEmpty(cmp.ctxKey)) {
            cmp.ctxKey = key;

            key = me.application.getRouter().recognizeByAction(actionName, ctrlName, key);
            if (key) {
                cmp.ctxKey = key;
            }
        }
    },

    /**
     * @method isCurrentContextTarget
     */
    isCurrentContextTarget: function (target, key) {
        var targetCtxKey, targetCtxParent;
        if (key === 'root') {
            return true;
        }

        if (!target || !key) {
            return false;
        }

        targetCtxKey = target.ctxKey;
        if (!targetCtxKey) {
            targetCtxParent = target.up('[ctxKey]') || B4.mixins.Context.__getCtxParent(target);
            if (targetCtxParent) {
                targetCtxKey = targetCtxParent.ctxKey;
            }
        }

        if (targetCtxKey === 'root') {
            return true;
        }

        if (!targetCtxKey) {
            return false;
        }

        targetCtxKey = B4.mixins.Context.normalizeRoute(targetCtxKey);
        key = B4.mixins.Context.normalizeRoute(key);

        return key.indexOf(targetCtxKey) === 0;
    },

    /**
     * @method contextLevel
     */
    contextLevel: function (target, key, level) {
        if (!target || !key) {
            return -1;
        }

        if (!level) {
            level = 0;
        }

        if (target.ctxKey) {
            if (target.ctxKey == key) {
                return ++level;
            }
        } else if (target.up(Ext.String.format('[ctxKey={0}]', key)) != null) {
            return ++level;
        }

        return this.contextLevel(target, this.application.getRouter().getParentRoute(key), ++level);

    },

    /**
     * @method getCmpInContext
     */
    getCmpInContext: function (selector) {
        return this.cmpQueryInContext(selector)[0];
    },

    /**
    * @method cmpQueryInContext
    * return {Ext.Component[]} массив компонентов, удовлетворяющих селектору и находящихся в контексте
    */
    cmpQueryInContext: function (selector) {
        var me = this,
            queryResult = Ext.ComponentQuery.query(selector) || [],
            ctxKey = this.getCurrentContextKey();

        queryResult = Ext.Array.filter(queryResult, function (cmp) {
            return me.isCurrentContextTarget(cmp, ctxKey);
        });

        // Если запрос вернул больше 1 отсортировать по уровню
        if (queryResult.length > 1) {

            Ext.Array.sort(queryResult, function (cmp1, cmp2) {
                return me.contextLevel(cmp1, ctxKey) - me.contextLevel(cmp2, ctxKey);
            });
        }
        return queryResult;
    },

    statics: {

        /**
         * @method getViewContextKey
         */
        getViewContextKey: function (view) {
            if (!view) {
                return null;
            }

            if (view.ctxKey) {
                return view.ctxKey;
            }

            if (view.up) {
                var parent = view.up('[ctxKey]');
                if (parent) {
                    return parent.ctxKey;
                }
            }

            return null;
        },

        /**
         * @method __init
         */
        __init: function (x) {
            var initFn = x.prototype.init;
            var initScope = initFn.scope;

            x.prototype.init = function (app) {
                this.contextCollection = new Ext.util.MixedCollection();
                if (Ext.isFunction(initFn)) {
                    initFn.apply(initScope || this, [app]);
                }
            };
        },

        normalizeRoute: function (routeStr) {
            return routeStr.lastIndexOf('/') === routeStr.length - 1 ? routeStr : routeStr + '/';
        },

        /**
         * @method __getRef
         */
        __getRef: function (x) {
            x.prototype.getRef = function (ref, info, config) {
                this.refCache = this.refCache || {};
                info = info || {};

                // в качестве config может придти либо конфигурация для создания компонента,
                // либо строка, либо null

                // если в качестве config передана строка, то произошел вызов @ref, например this.getRef(ctxKey)
                // и необходимо считать полученную строку одним из возможных контекстов поиска
                var refCtx = null;
                if (Ext.isString(config)) {
                    refCtx = config;
                    config = null;
                }
                // если передан конфиг создания компонента (объект),
                // то сохраняем конфиг в объект, который будет передан в
                // метод создания компонента                 
                if (Ext.isObject(config)) {
                    Ext.apply(info, config);
                }

                if (info.forceCreate) {
                    return Ext.ComponentManager.create(info, 'component');
                }

                var me = this,
                    ctxKey = this.ctxKey || refCtx || this.getCurrentContextKey() || '', // ctxKey - должен быть минимум ''
                    cached = me.refCache[ref + ctxKey],
                    queryResult;

                if (!cached) {
                    queryResult = Ext.ComponentQuery.query(info.selector);

                    queryResult = Ext.Array.filter(queryResult, function (cmp) {
                        return me.isCurrentContextTarget(cmp, ctxKey);
                    });

                    // Если запрос вернул больше 1 отсортировать по уровню
                    if (queryResult.length > 1) {

                        Ext.Array.sort(queryResult, function (cmp1, cmp2) {
                            return me.contextLevel(cmp1, ctxKey) - me.contextLevel(cmp2, ctxKey);
                        });
                    }

                    if (queryResult.length < 1) {
                        return null;
                    }

                    ctxKey = B4.mixins.Context.getViewContextKey(queryResult[0]);

                    me.refCache[ref + ctxKey] = cached = queryResult[0];
                    if (!cached && info.autoCreate) {
                        me.refCache[ref + ctxKey] = cached = Ext.ComponentManager.create(info, 'component');
                    }
                    if (cached) {
                        cached.on('beforedestroy', function () {
                            me.refCache[ref + ctxKey] = null;
                        });
                    }
                }
                return cached;
            };
        },

        __getOwnerCt: function (cmp) {
            if(cmp.ownerCt) {
                return cmp.ownerCt;
            }

            if(cmp.el) {
                var parent = cmp.el.parent();
                if(parent && parent.id) {
                    return Ext.getCmp(parent.id);
                }
            }

            return null;
        },

        __getCtxParent: function(cmp) {
            do {
                if(cmp.ctxKey) {
                    return cmp;
                }

                cmp = this.__getOwnerCt(cmp);
            } while(cmp != null);
        }
    }
});