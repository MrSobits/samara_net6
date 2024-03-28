Ext.define('B4.controller.objectcr.PersonalAccount', {
/*
* Контроллер раздела лицевые счета
*/
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.objectcr.PersonalAccount'
    ],

    models: ['objectcr.PersonalAccount'],
    stores: ['objectcr.PersonalAccount'],
    views: [
        'objectcr.PersonalAccountGrid',
        'objectcr.PersonalAccountEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'objectcr.PersonalAccountGrid',
    mainViewSelector: 'personalaccountgrid',

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [
        {
            xtype: 'personalaccountobjectcrperm',
            name:'personalAccountObjectCrPerm'
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела лицевых счетов
            */
            xtype: 'grideditctxwindowaspect',
            name: 'personalAccountGridWindowAspect',
            gridSelector: 'personalaccountgrid',
            editFormSelector: 'objectcrpersonalaccounteditwindow',
            modelName: 'objectcr.PersonalAccount',
            editWindowView: 'objectcr.PersonalAccountEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ObjectCr', this.controller.getContextValue(this.controller.getMainComponent(), 'objectcrId'));
                    }
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('personalaccountgrid'),
            store;

        me.getAspect('personalAccountObjectCrPerm').setPermissionsByRecord({ getId: function() { return id; } });

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);
    },
    
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.objectCrId = this.params.id;
        }
    }
});