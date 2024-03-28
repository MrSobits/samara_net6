Ext.define('B4.controller.constructionobject.Contract', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateButton',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.constructionobject.Contract'
    ],

    views: [
        'constructionobject.contract.Grid',
        'constructionobject.contract.EditWindow'
    ],

    models: ['constructionobject.Contract'],

    stores: ['constructionobject.Contract'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'constructionobject.contract.Grid',
    mainViewSelector: 'constructionobjectcontractgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjectcontractgrid'
        }
    ],

    aspects: [
        {
            xtype: 'constructionobjectcontractpermission',
            name: 'contractPermissionAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'constructobjContrGridWindowAspect',
            gridSelector: 'constructionobjectcontractgrid',
            editFormSelector: 'constructobjcontracteditwindow',
            modelName: 'constructionobject.Contract',
            editWindowView: 'constructionobject.contract.EditWindow',
            listeners: {
                getdata: function(asp, record) {
                    record.set('ConstructionObject', this.controller.getContextValue(this.controller.getMainView(), 'constructionObjectId'));
                },
                aftersetformdata: function (asp, rec) {
                    this.controller.getAspect('constructobjContrStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
            },
            onBeforeLoadContragent: function (field, options) {
                if (options.params == null) {
                    options.params = {};
                }
                options.params.showAll = true;
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'constructobjContrStateButtonAspect',
            stateButtonSelector: 'constructobjcontracteditwindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    this.setStateData(entityId, newState);

                    var editWindowAspect = asp.controller.getAspect('constructobjContrGridWindowAspect');
                    editWindowAspect.updateGrid();

                    var model = this.controller.getModel('constructionobject.Contract');
                    model.load(entityId, {
                        success: function(rec) {
                            editWindowAspect.setFormData(rec);
                        },
                        scope: this
                    }); 
                }
            }
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'constructObjectContractStateTransferAspect',
            gridSelector: 'constructionobjectcontractgrid',
            menuSelector: 'constructObjContractGridStateMenu',
            stateType: 'gkh_construct_obj_contract'
        }
    ],

    index: function(id) {
        var view = this.getMainView() || Ext.widget('constructionobjectcontractgrid');

        this.bindContext(view);
        this.setContextValue(view, 'constructionObjectId', id);
        this.application.deployView(view, 'construction_object_info');
        this.getAspect('contractPermissionAspect').setPermissionsByRecord({ getId: function () { return id; } });

        view.getStore().load();
    },

    init: function () {
        var actions = {};

        actions[this.mainViewSelector] = {
            'store.beforeload': {
                fn: this.onBeforeTypeWorkLoad,
                scope: this
            }
        };

        this.control(actions);

        this.callParent(arguments);
    },

    onBeforeTypeWorkLoad: function (_, opts) {
        var view = this.getMainView();
        if (view) {
            opts.params.objectId = this.getContextValue(view, 'constructionObjectId');
        }
    }
});