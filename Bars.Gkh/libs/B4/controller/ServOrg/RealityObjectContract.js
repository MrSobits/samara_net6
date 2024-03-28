Ext.define('B4.controller.servorg.RealityObjectContract', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow'
    ],

    models: ['servorg.RealityObjectContract'],

    stores: [
        'servorg.RealityObjectContract'
    ],

    views: [
        'servorg.RealityObjectContractGrid',
        'servorg.RoContractEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
    {
        xtype: 'grideditwindowaspect',
        name: 'servorgRoContractGridWindowAspect',
        gridSelector: 'servorgrocontractgrid',
        editFormSelector: 'servorgrocontracteditwindow',
        editWindowView: 'servorg.RoContractEditWindow',
        modelName: 'servorg.RealityObjectContract',
        storeName: 'servorg.RealityObjectContract',
            
        listeners: {
            getdata: function (asp, record) {
                if (!record.getId()) {
                    record.set('ServOrg', asp.controller.getContextValue(asp.controller.getMainView(), 'servorgId'));
                }
            },
            
            aftersetformdata: function (asp, record) {

                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetInfo', 'ServiceOrgRealityObject', {
                    contractId: record.getId()
                })).next(function (response) {
                    asp.controller.unmask();
                    var obj = Ext.JSON.decode(response.responseText);

                    var editWindow = Ext.ComponentQuery.query(asp.editFormSelector)[0];
                    var selectField = editWindow.down('#sfRealityObject');
                    selectField.setValue(obj);
                }).error(function () {
                    asp.controller.unmask();
                });
            }

        },
            
            otherActions: function (actions) {
                actions['servorgrocontracteditwindow #sfRealityObject'] = { 'beforeload': { fn: this.onBeforeLoadRealityObj, scope: this } };
            },
            
            /*устанавливаем id организации*/
            onBeforeLoadRealityObj: function (store, operation) {
                operation.params = operation.params || {};
                operation.params.servorgId = this.controller.getContextValue(this.controller.getMainView(), 'servorgId');
            }
        }
    ],
    
    params: null,
    
    mainView: 'servorg.RealityObjectContractGrid',
    mainViewSelector: 'servorgrocontractgrid',
    

    init: function () {
        this.getStore('servorg.RealityObjectContract').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('servorgrocontractgrid'),
            args = me.processArgs();

        me.bindContext(view);
        me.setContextValue(view, 'servorgId', id);
        me.application.deployView(view, 'serv_org');

        view.getStore().load();

        if (args && parseInt(args.contractId) > 0) {
            me.edit(id, parseInt(args.contractId));

            if (args.newToken) {
                me.application.redirectTo(args.newToken);
            }
        }
    },

    edit: function (id, contractId) {
        var me = this,
            aspect,
            baseModel = me.getModel('servorg.RealityObjectContract');

        contractId ? baseModel.load(contractId, {
            success: function (record) {
                aspect = me.getAspect('servorgRoContractGridWindowAspect');
                aspect.editRecord(record);
            },
            scope: aspect
        }) : function () {
            Ext.Msg.alert('Ошибка', 'Не найден поставщик жилищных услуг');
        };
    },

    onBeforeLoad: function (store, operation) {
        operation.params.servorgId = this.getContextValue(this.getMainView(), 'servorgId');
    },

    processArgs: function () {
        var token = Ext.History.getToken(),
            result = null,
            argsIndex = token.indexOf('?'),
            args,
            param;

        if (argsIndex > -1) {
            result = {};
            args = token.substring(argsIndex + 1).replace(new RegExp('/', 'g'), '').trim().split('&');

            if (args.length > 0) {
                args.forEach(function (item) {
                    if (item.indexOf('=') > 0) {
                        param = item.split('=');

                        if (param.length > 1) {
                            result[param[0]] = param[1];
                        }
                    }
                });
            }

            result.newToken = token.substring(0, argsIndex);
        }

        return result;
    }
});