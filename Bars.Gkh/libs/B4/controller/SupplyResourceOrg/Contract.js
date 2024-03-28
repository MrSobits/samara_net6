Ext.define('B4.controller.supplyresourceorg.Contract', {
    extend: 'B4.base.Controller',
    params: null,
    requires: ['B4.aspects.GridEditWindow'],

    models: ['supplyresourceorg.Contract',
            'B4.model.supplyresourceorg.RealtyObject'],

    stores: ['supplyresourceorg.Contract'],

    views: [
        'supplyresourceorg.ContractEditWindow',
        'supplyresourceorg.ContractGrid'
    ],

    mainView: 'supplyresourceorg.ContractGrid',
    mainViewSelector: 'supplyresorgcontractgrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'supplyResOrgContractGridWindowAspect',
            gridSelector: 'supplyresorgcontractgrid',
            editFormSelector: 'supplyresorgcontracteditwindow',
            storeName: 'supplyresourceorg.Contract',
            modelName: 'supplyresourceorg.Contract',
            editWindowView: 'supplyresourceorg.ContractEditWindow',
            otherActions: function (actions) {
                actions['supplyresorgcontracteditwindow #sfRealityObject'] = {
                    'beforeload': {
                        fn: function (store, operation) {
                            operation.params = operation.params || {};
                            operation.params.supplyResOrgId = this.controller.getContextValue(this.controller.getMainView(), 'supplyresorgId');
                        },
                        scope: this
                    }
                };
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.getId()) {
                        record.data.ResourceOrg = asp.controller.getContextValue(asp.controller.getMainView(), 'supplyresorgId');
                    }
                }
            },
            saveRequestHandler: function () {
                var rec, from = this.getForm();
                if (this.fireEvent('beforesaverequest', this) !== false) {
                    from.getForm().updateRecord();
                    rec = this.getRecordBeforeSave(from.getRecord());

                    var data = rec.getData();
                    var dateStart = data.DateStart;
                    var dateEnd = data.DateEnd;

                    if (dateEnd != null && dateEnd < dateStart) {
                        B4.QuickMsg.msg("Ошибка", "\"Дата начала\" должна быть меньше \"Дата окончания\"", "error");
                        return;
                    }

                    this.fireEvent('getdata', this, rec);

                    if (from.getForm().isValid()) {
                        if (this.fireEvent('validate', this)) {
                            this.saveRecord(rec);
                        }
                    } else {
                        //получаем все поля формы
                        var fields = from.getForm().getFields();

                        var invalidFields = '';

                        //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
                        Ext.each(fields.items, function (field) {
                            if (!field.isValid()) {
                                invalidFields += '<br>' + field.fieldLabel;
                            }
                        });

                        //выводим сообщение
                        Ext.Msg.alert('Ошибка сохранения!', 'Не заполнены обязательные поля: ' + invalidFields);
                    }
                }
            }
        }
    ],

    init: function () {
        this.getStore('supplyresourceorg.Contract').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('supplyresorgcontractgrid'),
            args = me.processArgs();

        me.bindContext(view);
        me.setContextValue(view, 'supplyresorgId', id);
        me.application.deployView(view, 'supplyres_org');

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
            baseModel = me.getModel('supplyresourceorg.Contract');

        contractId ? baseModel.load(contractId, {
            success: function (record) {
                aspect = me.getAspect('supplyResOrgContractGridWindowAspect');
                aspect.editRecord(record);
            },
            scope: aspect
        }) : function () {
            Ext.Msg.alert('Ошибка', 'Не найден поставщик коммунальных услуг');
        };
    },

    onBeforeLoad: function(store, operation) {
        operation.params.supplyResOrgId = this.getContextValue(this.getMainView(), 'supplyresorgId');
        operation.params.fromContract = true;
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