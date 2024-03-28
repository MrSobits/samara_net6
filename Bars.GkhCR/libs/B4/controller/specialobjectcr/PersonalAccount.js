Ext.define('B4.controller.specialobjectcr.PersonalAccount', {
/*
* Контроллер раздела лицевые счета
*/
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.specialobjectcr.PersonalAccount'
    ],

    models: [
        'specialobjectcr.PersonalAccount'
    ],
    stores: [
        'specialobjectcr.PersonalAccount'
    ],
    views: [
        'specialobjectcr.PersonalAccountGrid',
        'specialobjectcr.PersonalAccountEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'specialobjectcr.PersonalAccountGrid',
    mainViewSelector: 'specialobjectcrpersonalaccountgrid',

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',

    aspects: [
        {
            xtype: 'personalaccountspecialobjectcrperm',
            name:'personalAccountObjectCrPerm'
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела лицевых счетов
            */
            xtype: 'grideditctxwindowaspect',
            name: 'personalAccountGridWindowAspect',
            gridSelector: 'specialobjectcrpersonalaccountgrid',
            editFormSelector: 'specialobjectcrpersonalaccounteditwindow',
            modelName: 'specialobjectcr.PersonalAccount',
            editWindowView: 'specialobjectcr.PersonalAccountEditWindow',
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
            view = me.getMainView() || Ext.widget('specialobjectcrpersonalaccountgrid'),
            store;

        me.getAspect('personalAccountObjectCrPerm').setPermissionsByRecord({ getId: function() { return id; } });

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');

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