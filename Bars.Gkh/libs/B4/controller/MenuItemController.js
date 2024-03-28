Ext.define('B4.controller.MenuItemController', {
    extend: 'B4.base.Controller',

    /**
     * @cfg {String} Класс родительского контроллера
     */
    parentCtrlCls: null,

    init: function () {
        var me = this,
            naviCtrl;
        if (!me.parentCtrlCls) {
            Ext.Error.raise('Не указан родительский контроллер');
        }
        naviCtrl = me.application.getRouter().getController(me.parentCtrlCls);
        if (!naviCtrl.isInitialized) {
            naviCtrl.init(me.application);
        }
        me.callParent(arguments);
    }
});