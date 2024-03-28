/*
* средства источников финансирования
*/
Ext.define('B4.controller.workscr.FinSources', {
    
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.permission.typeworkcr.FinSources'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['objectcr.FinanceSourceRes'],
    
    stores: ['objectcr.FinanceSourceRes'],
    
    views: [
        'objectcr.FinanceSourceResGrid',
        'objectcr.FinanceSourceResEditWindow'
    ],

    mainView: 'objectcr.FinanceSourceResGrid',
    mainViewSelector: 'financesourceresgrid',

    aspects: [
        {
            xtype: 'finsourcestypeworkcrperm',
            name: 'financeSourcePerm',
            editFormAspectName: 'financeSourceResGridWindowAspect'
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования средств источника финансирования
            */
            xtype: 'grideditctxwindowaspect',
            name: 'financeSourceResGridWindowAspect',
            gridSelector: 'financesourceresgrid',
            editFormSelector: 'financesourcereseditwin',
            modelName: 'objectcr.FinanceSourceRes',
            editWindowView: 'objectcr.FinanceSourceResEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ObjectCr', asp.controller.getObjectId());
                        record.set('TypeWork', asp.controller.getTypeWorkId());
                    }
                }
            }
        }
    ],
    
    index: function(id, objectId) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('financesourceresgrid');

            view.getStore().on('beforeload',
                function(s, operation) {
                    operation.params.twId = id;
                    operation.params.objectId = objectId;
                });
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();

        me.getAspect('financeSourcePerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    getTypeWorkId: function () {
        var me = this;
        return me.getContextValue(me.getMainView(), 'twId');
    },

    getObjectId: function () {
        var me = this;
        return me.getContextValue(me.getMainView(), 'objectId');
    }
});