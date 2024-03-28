Ext.define('B4.controller.realityobj.Image', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.fieldrequirement.realityobj.Image',
        'B4.aspects.permission.realityobj.Image',
        'B4.aspects.permission.realityobj.RealityObjectFields'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'realityobj.Image'
    ],
    
    stores: [
        'realityobj.Image',
        'realityobj.Work'
    ],
    
    views: [
        'realityobj.ImageGrid',
        'realityobj.ImageEditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjimagegrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'realityobjimageperm',
            name: 'realityObjImagePerm'
        },
        {
            xtype: 'realityobjimagerequirement',
            name: 'realityObjImageRequirementAspect',
            viewSelector: 'realityobjimageeditwindow'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjImageGridWindowAspect',
            gridSelector: 'realityobjimagegrid',
            editFormSelector: 'realityobjimageeditwindow',
            storeName: 'realityobj.Image',
            modelName: 'realityobj.Image',
            editWindowView: 'realityobj.ImageEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.data.Id) {
                        record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                    }
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #imagesGroupComboBox'] = {
                    'change': { fn: me.onChangeImagesGroup, scope: me }
                };
                actions[me.editFormSelector + ' #workSelectField'] = {
                    'beforeload': { fn: me.onBeforeLoadWork, scope: me }
                };
                actions[me.editFormSelector + ' #periodSelectfield'] = {
                    'change': { fn: me.onChangePeriod, scope: me }
                };
            },
            onChangeImagesGroup: function (field, value) {
                var me = this,
                    period,
                    work,
                    editWindow = me.componentQuery(me.editFormSelector);

                if (value == 10) {
                    period = editWindow.down('#periodSelectfield');
                    period.hide();
                    period.setValue(null);
                    work = editWindow.down('#workSelectField');
                    work.hide();
                    work.setValue(null);
                }
                else {
                    editWindow.down('#periodSelectfield').show();
                    editWindow.down('#workSelectField').show();
                }
            },
            onBeforeLoadWork: function (store, operation) {
                var me = this;
                operation = operation || {};
                operation.params = operation.params || {};
                
                operation.params.realityObjectId = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                operation.params.periodId = me.controller.getContextValue(me.controller.getMainComponent(), 'preiodId');
            },
            onChangePeriod: function (field, newValue) {
                var me = this;
                if (newValue) {
                    me.controller.setContextValue(me.controller.getMainComponent(), 'preiodId', newValue.Id);
                } else {
                    me.controller.setContextValue(me.controller.getMainComponent(), 'preiodId', 0);
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('realityobj.Image').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjimagegrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        me.getStore('realityobj.Image').load();
        me.getAspect('realityObjImagePerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
    }
});