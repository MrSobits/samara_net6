Ext.define('B4.controller.realityobj.ServiceOrg', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.realityobj.ServiceOrg'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'servorg.RealityObjectContract'
    ],
    
    stores: [
        'servorg.RealityObjectContract'
    ],

    views: [
        'realityobj.ServiceOrgGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjserviceorggrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'realityobjserviceorgperm',
            name: 'realityObjServiceOrgPerm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjServiceOrgGridWindowAspect',
            gridSelector: 'realityobjserviceorggrid',
            editFormSelector: 'realityobjserorgeditwindow',
            storeName: 'realityobj.ServiceOrg',
            modelName: 'realityobj.ServiceOrg',
            editWindowView: 'realityobj.ServiceOrgEditWindow',
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #sfContragentOrganization'] = {
                     'beforeload': { fn: this.onBeforeLoadContragent, scope: this}
                };
            },
            onBeforeLoadContragent: function (store, operation) {
                var me = this;
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.typeServOrg = me.getForm().down('#cbTypeServOrg').getValue();
            },
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                    }
                },
                beforerowaction: function (asp, grid, action, record) {
                    switch (action.toLowerCase()) {
                        case 'doubleclick':
                        case 'edit':
                            Ext.History.add(Ext.String.format('serviceorganizationedit/{0}/realobjcontract?contractId={1}', record.get('ServOrgId'), record.getId()));
                            return false;
                        case 'delete':
                            Ext.Msg.alert('Ошибка!', 'В данном разделе нельзя удалять записи. Для удаления необходимо перейти в Участники процесса - Поставщики жилищных услуг - раздел \"Договора с жилыми домами\".');
                            return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjserviceorggrid');

        view.getStore().on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjserviceorggrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        view.getStore().load();
        me.getAspect('realityObjServiceOrgPerm').setPermissionsByRecord({ getId: function () { return id } });
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    }
});