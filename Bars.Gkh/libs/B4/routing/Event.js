/**
 * @private
 */
Ext.define('B4.routing.Event', {
    override: 'Ext.util.Event',

    fire: function () {
        var me = this,
            listeners = me.listeners,
            count = listeners.length,
            i,
            args,
            listener,
            scope;

        if (count > 0) {
            me.firing = true;
            for (i = 0; i < count; i++) {
                listener = listeners[i];
                args = arguments.length ? Array.prototype.slice.call(arguments, 0) : [];
                if (listener.o) {
                    args.push(listener.o);
                }
                scope = listener.scope || me.observable;
                if (scope && Ext.isFunction(scope.getContext) && !scope.isController) {
                    scope = scope.getContext.apply(scope, args);
                }
                scope = scope || listener.scope || me.observable;

                if (listener && listener.fireFn.apply(scope, args) === false) {
                    return (me.firing = false);
                }
            }
        }
        me.firing = false;
        return true;
    }
});