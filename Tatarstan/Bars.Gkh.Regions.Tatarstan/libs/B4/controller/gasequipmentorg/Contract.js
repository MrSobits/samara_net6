Ext.define('B4.controller.gasequipmentorg.Contract', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GridEditWindow'],

    models: ['gasequipmentorg.Contract'],

    views: [
        'gasequipmentorg.ContractEditWindow',
        'gasequipmentorg.ContractGrid'
    ],

    mainView: 'gasequipmentorg.ContractGrid',
    mainViewSelector: 'gasequipmentorgcontractgrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [                  
        {
            xtype: 'grideditwindowaspect',
            name: 'gasEquipmentOrgContractGridWindowAspect',
            gridSelector: 'gasequipmentorgcontractgrid',
            editFormSelector: 'gasequipmentorgcontracteditwindow',
            storeName: 'gasequipmentorg.Contract',
            modelName: 'gasequipmentorg.Contract',
            editWindowView: 'gasequipmentorg.ContractEditWindow',
            otherActions: function (actions) {
                var me = this;
                actions['gasequipmentorgcontracteditwindow #sfRealityObject'] = {
                    'beforeload': {
                        fn: function (store, operation) {
                            operation.params = operation.params || {};
                            operation.params.GasEquipmentOrg = me.controller.getContextValue(me.controller.getMainView(), 'gasEquipmentOrgId');
                        },
                        scope: me
                    }
                };                
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.getId()) {
                        record.data.GasEquipmentOrg = asp.controller.getContextValue(asp.controller.getMainView(), 'gasEquipmentOrgId');
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'gasequipmentorgcontractgrid b4updatebutton': {
                click: { fn: me.updateGrid, scope: me }
            }
        });

        me.callParent(arguments);
    },
    
    updateGrid: function (btn) {
        btn.up('gasequipmentorgcontractgrid').getStore().load();
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('gasequipmentorgcontractgrid');

        me.bindContext(view);
        me.setContextValue(view, 'gasEquipmentOrgId', id);
        me.application.deployView(view, 'gasequipmentorg_info');

        var store = view.getStore();

        store.on('beforeload', me.onBeforeLoad, me);
        store.load();
               
    },

    edit: function (id, contractId) {
        var me = this,
            aspect,
            baseModel = me.getModel('gasequipmentorg.Contract');

        contractId ? baseModel.load(contractId, {
            success: function (record) {
                aspect = me.getAspect('gasEquipmentOrgContractGridWindowAspect');
                aspect.editRecord(record);
            },
            scope: aspect
        }) : function () {
            Ext.Msg.alert('Ошибка', 'Не найден поставщик ВДГО');
        };
    },

    onBeforeLoad: function (store, operation) {

        operation.params.GasEquipmentOrg = this.getContextValue(this.getMainView(), 'gasEquipmentOrgId');
        operation.params.fromContract = true;
    },       
});