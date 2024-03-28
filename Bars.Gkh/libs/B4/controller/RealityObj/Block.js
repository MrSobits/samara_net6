Ext.define('B4.controller.realityobj.Block', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.realityobj.Block',
        'B4.aspects.fieldrequirement.realityobj.Block'
    ],

    models: [
        'realityobj.Block'
    ],
    
    stores: [
        'realityobj.Block'
    ],
    
    views: [
        'realityobj.BlockGrid',
        'realityobj.BlockEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjblockgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'realityobjblockrequirement',
            name: 'realityObjBlockRequirementPerm',
            viewSelector: 'blockeditwindow',
            listeners: {
                'apply': function(asp, req, opts, comp) {
                    if (comp && comp.validate) {
                        comp.validate();
                    }
                }
            }
        },
        {
            xtype: 'realityobjblockperm',
            name: 'realityObjBlockPerm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjBlockGridEditAspect',
            gridSelector: 'realityobjblockgrid',
            storeName: 'realityobj.Block',
            modelName: 'realityobj.Block',
            editWindowView: 'realityobj.BlockEditWindow',
            editFormSelector: 'blockeditwindow',
            closeWindowHandler: function () {
                var me = this;
                me.controller.params = null;
                me.getForm().close();
            },
            listeners: {
                beforesave: function (asp, rec) {
                    if (rec.get('LivingArea') > rec.get('AreaTotal')) {
                        Ext.Msg.alert('Ошибка', 'Жилая площадь не может быть больше общей площади');
                        return false;
                    }
                    asp.controller.params = null;
                    return true;
                },
                getdata: function (me, record) {
                    if (!parseInt(record.get('Id'))) {
                        record.set('RealityObject', { Id: me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId') });
                    }
                },
                afterlayout: function (aspect, record, form) {
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('realityobj.Block').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjblockgrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.Block').load();
        me.getAspect('realityObjBlockPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    }
});