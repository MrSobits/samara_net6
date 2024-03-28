/**
 * Глобальный наблюдатель за компонентами, на которые вешается плагин.
 * Способен на централизованный вызов у всех подписчиков метода @subscribeMethod
 *  по событие @subscribeOnEvent
 */
Ext.define('B4.plugins.GlobalComponentWatcher', {
    extend: 'Ext.AbstractPlugin',
    
    alias: 'plugin.globalcomponentwatcher',

    /**
     * @property {string}
     *
     * Наименование метода на который идет подписывание компонента и по которому будут синхронизироваться
     *  остальные компоненты
     */
    subscribeOnEvent: '',
    
    /**
     * @property {object}
     *
     * Метод, вызываемый при вызове события subscribeOnEvent
     */
    subscribeMethod: null,

    watcher: (function () {
        var _components = [];
            
        var _getComponents = function () {
            return _components;
        };

        return {
            getComponents: _getComponents
        };
    })(),

    init: function (component) {
        var me = this,
            components = me.watcher.getComponents();

        if (me.subscribeMethod && !Ext.isEmpty(me.subscribeOnEvent)) {
            component.on(me.subscribeOnEvent, me.subscribeMethod, me);
            me.privateMembers.subscribeComponents(me);
        }
        
        components.push(component);
    },
    
    privateMembers: (function () {
        var _subscribeComponents = function(scope) {
            Ext.each(scope.watcher.getComponents() || [], function (component) {
                component.on(scope.subscribeOnEvent, scope.subscribeMethod, scope);
            }, this);
        };

        return {
            subscribeComponents: _subscribeComponents
        };
    })()
});