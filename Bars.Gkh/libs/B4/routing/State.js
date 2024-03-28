/**
 * @class B4.routing.State
 * Класс, представляющий собой состояние системы. В каждый момент времени хранит 
 * состояние системы (открытые и активные вкладки) в виде дерева. При каждом изменении синхронизирует себя с
 * localStorage либо cookie. {@link B4.routing.Router}  при помощи экземпляра этого класса может восстановить последнее состояние системы.
 * @private
 * 
 */
Ext.define('B4.routing.State', {
    requires: ['B4.routing.model.Path', 'B4.data.proxy.CookieStorage'],

    singleton: true,

    config: {
        store: undefined
    },

    constructor: function() {
        var me = this,
            storeConfig = {
                model: 'B4.routing.model.Path',
                autoLoad: true,
                autoSync: true,
                root: {
                    route: 'root',
                    active: true
                }
            };
        if (!window.localStorage) {
            Ext.apply(storeConfig, {
                proxy: {
                    type: 'cookiestorage',
                    id: 'app-paths'
                }
            });
        }
        me.setStore(Ext.create('Ext.data.TreeStore', storeConfig));
    },

    /**
    * Добавляет переданную строку в соответствующее место в дереве.
    * @param {String} token добавляемая строка
    */
    add: function(token) {
        var rootNode = this.getStore().getRootNode();
        this.appendChild(rootNode, token);
    },

    /**
     * Добавляет строку в соответствующее место в поддереве.
     * @private
     * @param {Ext.data.NodeInterface} root корень поддерева, в которое будем добавлять.
     * @param {String} token добавляемая строка.
     */
    appendChild: function(root, token) {
        var tempNode, params, newNode, route = token;

        if (token.indexOf('?') > -1) {
            params = Ext.Object.fromQueryString(token.substr(token.indexOf('?') + 1));
            route = B4.routing.State.normalizeRoute(token.substring(0, token.indexOf('?')));
        } else {
            route = B4.routing.State.normalizeRoute(route);
            token = B4.routing.State.normalizeRoute(token);
        }

        tempNode = root.findChildBy(function(node) {
            return route.indexOf(node.get('route')) === 0;
        });

        if (!tempNode) {
            if (root.get('route') !== route) {
                newNode = root.appendChild({
                    route: route,
                    params: params
                });
                this.activate(newNode);
                this.getStore().sync();
            } else {
                this.activate(root);
                this.getStore().sync();
            }
        } else {
            this.appendChild(tempNode, token);
        }
    },

    /**
     * Удаляет элемент дерева, соответствующий переданной строке.
     * @private
     * @param {String} token строка, по которой будет удаляться элемент дерева.
     * @return {Ext.data.NodeInterface/undefined} удаленный элемент дерева. 
     * Вернет undefined, если соответствующий token элемент не будет найден.
     */
    remove: function(token) {
        //this.removeRecursive(token, this.getStore().getRootNode());
        var root = this.getStore().getRootNode(),
            toRemove = root.findChildBy(function(node) {
                    return B4.routing.State.normalizeRoute(token.split('?')[0]) === node.get('route');
                }, this, true),
            parent,
            toActivate;

        if (toRemove) {
            if (toRemove.get('active')) {
                parent = toRemove.parentNode;
                toActivate = parent.findChild('active', false, true);
                if (toActivate) {
                    toActivate.set('active', true);
                }
            }
            return toRemove.remove();
        }
    },

    /**
     * Удаляет элемент дерева, соответствующий переданной строке, при этом потомков этого элемента переносить к дочернему элементу.
     * @private
     * @param {String} token строка, по которой будет удаляться элемент дерева.
     * @return {Ext.data.NodeInterface/undefined} удаленный элемент дерева. 
     * Вернет undefined, если соответствующий token элемент не будет найден.
     */
    slice: function(token) {
        var root = this.getStore().getRootNode(),
            toRemove = root.findChildBy(function(node) {
                    return B4.routing.State.normalizeRoute(token.split('?')[0]) === node.get('route');
                }, this, true);

        if (toRemove && toRemove.hasChildNodes()) {
            Ext.Array.each(toRemove.childNodes,
                function(node) {
                    toRemove.parentNode.appendChild(node, true);
                });
        }

        return toRemove.remove();
    },

    /**
     * Активирует элемент дерева.
     * @param {Ext.data.NodeInterface} node активируемый элемент дерева
     */
    activate: function(node) {
        var next, prev, parent;
        if (node && !node.isRoot()) {
            next = node.nextSibling;
            prev = node.previousSibling;
            while (next) {
                next.set('active', false);
                next = next.nextSibling;
            }
            while (prev) {
                prev.set('active', false);
                prev = prev.previousSibling;
            }
            node.set('active', true);
            parent = node.parentNode;
            this.activate(parent);
        }
    },

    /**
    * Возвращает строку, соответсвующую активному элементу дерева.
    * @return {String}
    */
    getActiveToken: function() {
        var node, activeToken = '';
        node = this.getStore().getRootNode();
        while (node.hasChildNodes()) {
            node = node.findChild('active', true);
            activeToken = node.get('route');
        }
        return activeToken;
    },

    /**
     * Возвращает строку, соответветствующую активному элементу из поддерева,
     * корнем которого является элемент, соответствующий передаваемой строке.
     * @param {String} startsWith
     */
    getActiveChild: function(startsWith) {
        var activeToken,
            node,
            route = startsWith ? B4.routing.State.normalizeRoute(startsWith.split('?')[0]) : startsWith;

        if (route === 'root/' || Ext.isEmpty(route)) {
            node = this.getStore().getRootNode();
            while (node.hasChildNodes()) {
                node = node.findChild('active', true);
                activeToken = node.get('route');
            }
            return node.isRoot() ? '' : activeToken;
        }

        node = this.getStore().getRootNode().findChildBy(function(item) {
                return item.get('route') === route;
            }, this, true);

        if (node) {
            activeToken = node.get('route');
            while (node.hasChildNodes()) {
                node = node.findChild('active', true);
                activeToken = node.get('route');
            }
            return activeToken + (node.get('params') ? '?' + Ext.Object.toQueryString(node.get('params')) : '');
        }

        return startsWith;
    },

    /**
     * Возвращает дерево состояний в линейном виде.
     * @return {String[]} массив ссылок в правильной последовательности.
     */
    linearize: function() {
        var node,
            arr = [],
            stack = [],
            tmp,
            me = this,
            root = me.getStore().getRootNode();
        stack = stack.concat(root.childNodes);
        stack = Ext.Array.sort(stack,
            function(a, b) {
                return a.get('active') - b.get('active');
            });
        while (stack.length > 0) {
            node = stack.shift();
            arr.push(node);
            if (node.hasChildNodes()) {
                tmp = Ext.Array.sort(node.childNodes,
                    function(a, b) {
                        return a.get('active') - b.get('active');
                    });
                stack = tmp.concat(stack);
            }
        }
        return Ext.Array.map(arr,
            function(item) {
                var paramsStr = item.get('params') ? '?' + Ext.Object.toQueryString(item.get('params')) : '';
                return item.get('route') + paramsStr;
            });
    },

    /**
     * Проверяет, является ли активным элемент дерева, соответстующий передаваемой строке.
     * @return {Boolean}
     */
    isActiveRoute: function(token) {
        var path = this.findByRoute(B4.routing.State.normalizeRoute(token));
        if (path) {
            return path.get('active');
        }
    },

    /**
     * Возвращает экземпляр {@link B4.routing.Path} по данному route.
     * @private
     * @param {String} route
     * @param {Boolean} deep значение по умолчанию true
     * @return {B4.routing.Path}
     */
    findByRoute: function(route, deep) {
        var path;
        this.getStore().getRootNode().findChildBy(function(item) {
                if (B4.routing.State.normalizeRoute(item.get('route')) === B4.routing.State.normalizeRoute(route.split('?')[0])) {
                    path = item;
                    return;
                }
            }, this, (deep !== false));
        return path;
    },

    normalizeRoute: function(routeStr) {
        return routeStr.lastIndexOf('/') === routeStr.length - 1 ? routeStr : routeStr + '/';
    }
});