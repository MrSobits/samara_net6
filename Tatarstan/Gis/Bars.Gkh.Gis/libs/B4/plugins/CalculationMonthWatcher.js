/**
 * Глобальный наблюдатель за компонентами на основе расчетного месяца
 * Вешается как плагин на компонент "Расчетный месяц" и при изменении сохраняет значение
 *  и обновляет его на всех открытых панелях, которые используют данный плагин
 */
Ext.define('B4.plugins.CalculationMonthWatcher', {
    extend: 'B4.plugins.GlobalComponentWatcher',
    
    alias: 'plugin.calculationmonthwatcher',

    afterTriggerEvent: null,
    
    subscribeOnEvent: 'change',
    
    subscribeMethod: function(changedComponent) {
        var me = this;
        
        var value = Ext.Date.format(changedComponent.getValue(), 'F, Y'),
            components = me.watcher.getComponents();

        if (components) {
            Ext.each(components, function(cmp) {
                if (changedComponent != cmp) {
                    cmp.setRawValue(value);
                }
            }, me);
        }

        me.watcher.monthValue.value = value;
        
        if (me.afterTriggerEvent) {
            me.afterTriggerEvent(changedComponent);
        }
        
        return false;
    },
    
    init: function (component) {
        var me = this;
        
        me.callParent(arguments);

        if (!me.watcher.monthValue) {
            me.watcher.monthValue = { value: Ext.Date.format(new Date(), 'F, Y') };
        }
        
        component.setRawValue(me.watcher.monthValue.value);
    }
});