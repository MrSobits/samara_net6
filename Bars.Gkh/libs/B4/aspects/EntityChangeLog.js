Ext.define('B4.aspects.EntityChangeLog', {
    extend: 'B4.base.Aspect',

    alias: 'widget.entitychangelogaspect',

    /*
     * Селектор панели, для которой осуществляется переход вперед-назад
     */
    gridSelector: null,
    /*
     * Полный тип логируемой сущности
     */
    entityType: '',
    /*
     * Функция получения идентификатора сущности
     */
    getEntityId: Ext.emptyFn,
    /*
     * Имя сервиса для получения истории
     */
    inheritEntityChangeLogCode: null,

    init: function (controller) {
        var me = this,
            actions = {};

        if (!me.gridSelector) {
            Ext.Error.raise('Не указан селектор грида истории изменений');
        }
        if (me.getEntityId === Ext.emptyFn) {
            Ext.Error.raise('Не указана функция выборки');
        }

        actions[me.gridSelector] = {
            'render': function (grid) {
                var store = grid.getStore();
                store.on('beforeload', me.beforeStoreLoad, me);
                store.load();
            }
        };

        controller.control(actions);
        me.callParent(arguments);
    },

    beforeStoreLoad: function(store, operation) {
        var me = this,
            params = {};

        params.id = me.getEntityId();
        params.entityType = me.entityType;
        params.code = me.inheritEntityChangeLogCode;

        Ext.apply(operation.params, params);
    }
});