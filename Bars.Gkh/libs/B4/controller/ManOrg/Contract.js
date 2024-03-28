Ext.define('B4.controller.manorg.Contract', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.manorg.Contract',
        'B4.enums.TypeManagementManOrg',
        'B4.enums.TypeContractManOrgRealObj',
        'B4.enums.ClaimWork.TypeManagementManOrg',
        'B4.aspects.fieldrequirement.ManorgTsjJskContract',
        'B4.aspects.fieldrequirement.ManorgOwnersContract',
        'B4.aspects.ButtonDataExport'
    ],

    models: [
        'manorg.contract.Base',
        'manorg.contract.Transfer',
        'manorg.contract.Owners',
        'manorg.contract.JskTsj',
        'realityobj.DirectManagContract'
    ],

    stores: [
        'manorg.contract.Base',
        'manorg.contract.Transfer',
        'manorg.ForSelect',
        'manorg.ForSelected',
        'realityobj.ByManOrg'
    ],

    views: [
        'manorg.contract.Grid',
        'manorg.contract.TransferEditWindow',
        'manorg.contract.OwnersEditWindow',
        'manorg.contract.JskTsjEditWindow',
        'manorg.contract.RelationEditWindow',
        'SelectWindow.MultiSelectWindow',
        'manorg.contract.DirectManagEditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    params: {},

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'manorgContractGridButtonExportAspect',
            gridSelector: 'manorgContractGrid',
            buttonSelector: 'manorgContractGrid #btnExport',
            controllerName: 'ManOrgJskTsjContract',
            actionName: 'Export'
        },
        {
            xtype: 'manorgcontractperm'
        },
        {
            xtype: 'manorgownerscontractfieldrequirement'
        },
        {
            xtype: 'manorgtsjjskcontractfieldrequirement'
        },
        /*
        *аспект взаимодействия грида и 20 и 40 ( ТСЖ  ЖСХ) 
        */
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgContractTransferWindowAspect',
            gridSelector: 'manorgContractGrid',
            modelName: 'manorg.contract.Transfer',
            modelType: B4.enums.TypeContractManOrgRealObj.ManagingOrgJskTsj,
            editFormSelector: '#manorgTransferEditWindow',
            editWindowView: 'manorg.contract.TransferEditWindow',
            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' #sfRealityObject'] = {
                    'beforeload': {
                        fn: function (store, operation) {
                            operation.params = operation.params || {};
                            operation.params.manorgId = me.controller.getContextValue(me.controller.getMainView(), 'manorgId');
                        },
                        scope: me
                    }
                };

                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
               
            },

            onChangeCheckbox: function (field, newValue) {
                this.controller.params.showCloseAppeals = newValue;
                var view = field.up('manorgContractGrid'),
                    store = view.getStore();
                store.filters.clear();
                var controller = this.controller;
                store.filter([
                    { property: 'manorgId', value: controller.getContextValue(controller.getMainView(), 'manorgId') },
                    { property: 'showClose', value: newValue },
                    { property: 'fromManagOrg', value: true }]);

  
            },

            rowAction: function (grid, action, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowAction.apply(this, arguments);
                }
            },

            rowDblClick: function (view, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowDblClick.apply(this, arguments);
                }
            },

            gridAction: function (grid, action) {
                if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
                    switch (action.toLowerCase()) {
                        case 'addmanorgowners':
                            this.editRecord();
                            break;
                        case 'update':
                            this.updateGrid();
                            break;
                    }
                }
            },

            listeners: {
                getdata: function (asp, record) {
                    var controller = asp.controller;

                    if (!record.getId()) {
                        record.set('ManagingOrganization', controller.getContextValue(controller.getMainView(), 'manorgId'));
                        record.set('TypeContractManOrgRealObj', this.modelType);
                    }
                },
                beforegridaction: function (asp, grid, action) {
                    var me = this,
                        typeManagementManOrg = B4.enums.ClaimWork.TypeManagementManOrg,   
                        type = me.controller.getContextValue(me.controller.getMainView(), 'TypeManagement');
                    return type === typeManagementManOrg.TSJ || type === typeManagementManOrg.JSK;
                },

                aftersetformdata: function (asp, record) {
                    asp.controller.setContract(record.getId());

                    asp.controller.mask('Загрузка', asp.controller.getMainView());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'ManOrgContractOwners', {
                        contractId: record.getId()
                    })).next(function (response) {
                        var obj = Ext.JSON.decode(response.responseText);

                        var editWindow = Ext.ComponentQuery.query(asp.editFormSelector)[0];

                        var selectField = editWindow.down('#sfRealityObject');
                        selectField.setValue(obj);

                        if (obj.TypeHouse != 30) {
                            var paymentsTab = editWindow.down('tabpanel tab[text=Сведения о сроках]');

                            if (paymentsTab) {
                                paymentsTab.hide();
                            }
                        }

                        asp.controller.unmask();
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                    editWindow.down('#docDateMonth')
                       .setValue({ 'ThisMonthPaymentDocDate': record.get('ThisMonthPaymentDocDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesBeginDateRb')
                       .setValue({ 'ThisMonthInputMeteringDeviceValuesBeginDate': record.get('ThisMonthInputMeteringDeviceValuesBeginDate') });


                    editWindow.down('#thisMonthInputMeteringDeviceValuesEndDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesEndDate': record.get('ThisMonthInputMeteringDeviceValuesEndDate') });

                    editWindow.down('#thisMonthPaymentServiceDateRb')
                        .setValue({ 'ThisMonthPaymentServiceDate': record.get('ThisMonthPaymentServiceDate') });
                }
            }
        },

        /*
        *аспект взаимодействия грида и 10 и 100 (УК Прочее)
        */
        {
            xtype: 'grideditwindowaspect',
            name: 'manorgContractOwnersGridWindowAspect',
            gridSelector: 'manorgContractGrid',
            modelName: 'manorg.contract.Owners',
            modelType: B4.enums.TypeContractManOrgRealObj.ManagingOrgOwners,
            editFormSelector: '#manorgContractOwnersEditWindow',
            editWindowView: 'manorg.contract.OwnersEditWindow',
            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' #sfRealityObject'] = {
                    'beforeload': {
                        fn: function (store, operation) {
                            operation.params = operation.params || {};
                            operation.params.manorgId = me.controller.getContextValue(me.controller.getMainView(), 'manorgId');
                        },
                        scope: me
                    }
                };
                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },

            onChangeCheckbox: function (field, newValue) {
                this.controller.params.showCloseAppeals = newValue;
                var view = field.up('manorgContractGrid'),
                    store = view.getStore();
                store.filters.clear();
                var controller = this.controller;
                store.filter([
                    { property: 'manorgId', value: controller.getContextValue(controller.getMainView(), 'manorgId') },
                    { property: 'showClose', value: newValue },
                    { property: 'fromManagOrg', value: true }]);


            },

            rowAction: function (grid, action, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowAction.apply(this, arguments);
                }
            },

            rowDblClick: function (view, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowDblClick.apply(this, arguments);
                }
            },

            gridAction: function (grid, action) {
                if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
                    switch (action.toLowerCase()) {
                        case 'addmanorgowners':
                            this.editRecord();
                            break;
                        case 'update':
                            this.updateGrid();
                            break;
                    }
                }
            },

            listeners: {
                getdata: function (asp, record) {
                    var controller = asp.controller;

                    if (!record.getId()) {
                        record.set('ManagingOrganization', controller.getContextValue(controller.getMainView(), 'manorgId'));
                        record.set('TypeContractManOrgRealObj', this.modelType);
                    }
                },

                beforegridaction: function (asp, grid, action) {
                    var me = this,
                        typeManagementManOrg = B4.enums.ClaimWork.TypeManagementManOrg,  
                        type = me.controller.getContextValue(me.controller.getMainView(), 'TypeManagement');
                    return type === typeManagementManOrg.UK || type === typeManagementManOrg.Other;
                },
                
                aftersetformdata: function (asp, record) {
                    asp.controller.setContract(record.getId());

                    asp.controller.mask('Загрузка', asp.controller.getMainView());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'ManOrgContractOwners', {
                        contractId: record.getId()
                    })).next(function (response) {
                        var obj = Ext.JSON.decode(response.responseText);

                        var editWindow = Ext.ComponentQuery.query(asp.editFormSelector)[0];

                        var selectField = editWindow.down('#sfRealityObject');
                        selectField.setValue(obj);

                        if (obj.TypeHouse != 30) {
                            var paymentsTab = editWindow.down('tabpanel tab[text=Сведения о сроках]');

                            if (paymentsTab) {
                                paymentsTab.hide();
                            }
                        }

                        asp.controller.unmask();
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                    editWindow.down('#docDateMonth')
                       .setValue({ 'ThisMonthPaymentDocDate': record.get('ThisMonthPaymentDocDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesBeginDateRb')
                       .setValue({ 'ThisMonthInputMeteringDeviceValuesBeginDate': record.get('ThisMonthInputMeteringDeviceValuesBeginDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesEndDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesEndDate': record.get('ThisMonthInputMeteringDeviceValuesEndDate') });

                    editWindow.down('#thisMonthPaymentServiceDateRb')
                        .setValue({ 'ThisMonthPaymentServiceDate': record.get('ThisMonthPaymentServiceDate') });
                }
            }
        },

        {
            xtype: 'grideditwindowaspect',
            name: 'manorgJskTsjContractGridWindowAspect',
            gridSelector: 'manorgContractGrid',
            modelName: 'manorg.contract.JskTsj',
            modelType: B4.enums.TypeContractManOrgRealObj.JskTsj,
            editFormSelector: '#jskTsjContractEditWindow',
            editWindowView: 'manorg.contract.JskTsjEditWindow',

            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #sfRealityObject'] = { 'beforeload': { fn: me.onBeforeLoadRealityObj, scope: me } };

                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },

            onChangeCheckbox: function (field, newValue) {
                this.controller.params.showCloseAppeals = newValue;
                var view = field.up('manorgContractGrid'),
                    store = view.getStore();
                store.filters.clear();
                var controller = this.controller;
                store.filter([
                    { property: 'manorgId', value: controller.getContextValue(controller.getMainView(), 'manorgId') },
                    { property: 'showClose', value: newValue },
                    { property: 'fromManagOrg', value: true }]);


            },

            /*устанавливаем id управляющей организации*/
            onBeforeLoadRealityObj: function (store, operation) {
                var me = this;
                operation.params = operation.params || {};
                operation.params.manorgId = me.controller.getContextValue(me.controller.getMainView(), 'manorgId');
            },

            gridAction: function (grid, action) {

                //тут мы по actionName кнопок подсовываем нужные параметры

                if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
                    if (action.toLowerCase() == 'addjsktsj') {
                        this.editRecord();
                    }
                }
            },

            onSaveSuccess: function (asp, record) {
                this.setContract(record);
            },

            setContract: function (record) {
                var id = record.getId();
                this.controller.setContract(id);

                var store = this.controller.getStore('manorg.contract.Transfer');

                var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];
                var grid = editWindow.down('#manOrgContractRelationGrid');

                store.removeAll();
                grid.setDisabled(!id);

                if (id > 0) {
                    store.load();
                }
            },

            rowAction: function (grid, action, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowAction.apply(this, arguments);
                }
            },

            rowDblClick: function (view, record) {
                if (record.get('TypeContractManOrgRealObj') === this.modelType) {
                    this.superclass.rowDblClick.apply(this, arguments);
                }
            },

            listeners: {
                getdata: function (asp, record) {
                    var controller = asp.controller;

                    if (!record.getId()) {
                        record.set('ManagingOrganization', controller.getContextValue(controller.getMainView(), 'manorgId'));
                        //данный параметр modelType был проставлен в методе getModel
                        record.set('TypeContractManOrgRealObj', this.modelType);
                    }
                },
                aftersetformdata: function (asp, record) {
                    asp.setContract(record);

                    asp.controller.mask('Загрузка', asp.controller.getMainView());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'ManOrgJskTsjContract', {
                        contractId: record.getId()
                    })).next(function (response) {
                        
                        var obj = Ext.JSON.decode(response.responseText);

                        var editWindow = Ext.ComponentQuery.query(asp.editFormSelector)[0];
                        var selectField = editWindow.down('#sfRealityObject');
                        selectField.setValue(obj);

                        if (obj.TypeHouse != 30) {
                            var paymentsTab = editWindow.down('tabpanel tab[text=Сведения о сроках]');

                            if (paymentsTab) {
                                paymentsTab.hide();
                            }
                        }

                        asp.controller.unmask();
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                    editWindow.down('#docDateMonth')
                         .setValue({ 'ThisMonthPaymentDocDate': record.get('ThisMonthPaymentDocDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesBeginDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesBeginDate': record.get('ThisMonthInputMeteringDeviceValuesBeginDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesEndDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesEndDate': record.get('ThisMonthInputMeteringDeviceValuesEndDate') });

                    editWindow.down('#thisMonthPaymentServiceDateRb')
                        .setValue({ 'ThisMonthPaymentServiceDate': record.get('ThisMonthPaymentServiceDate') });
                }
            }
        },

        {
            xtype: 'grideditwindowaspect',
            name: 'manOrgContractRelationWindowAspect',
            gridSelector: '#manOrgContractRelationGrid',
            editFormSelector: '#manOrgContractRelationEditWindow',
            storeName: 'manorg.contract.Transfer',
            modelName: 'manorg.contract.Transfer',
            editWindowView: 'manorg.contract.RelationEditWindow',
            listeners: {
                beforesave: function(asp, record) {
                    record.set('JskTsjContractId', asp.controller.getContextValue(asp.controller.getMainView(), 'contractId'));
                    record.set('ManOrgJskTsj', asp.controller.getContextValue(asp.controller.getMainView(), 'manorgId'));

                    return record;
                },
                aftersetformdata: function(asp, record) {
                    var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                    editWindow.down('#docDateMonth')
                         .setValue({ 'ThisMonthPaymentDocDate': record.get('ThisMonthPaymentDocDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesBeginDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesBeginDate': record.get('ThisMonthInputMeteringDeviceValuesBeginDate') });

                    editWindow.down('#thisMonthInputMeteringDeviceValuesEndDateRb')
                        .setValue({ 'ThisMonthInputMeteringDeviceValuesEndDate': record.get('ThisMonthInputMeteringDeviceValuesEndDate') });

                    editWindow.down('#thisMonthPaymentServiceDateRb')
                        .setValue({ 'ThisMonthPaymentServiceDate': record.get('ThisMonthPaymentServiceDate') });
                }
            },
            otherActions: function(actions) {
                var me = this;
                actions[me.editFormSelector + ' #sfManorg'] = { 'beforeload': { fn: me.onBeforeLoadManOrg, scope: me } };
            },

            onBeforeLoadManOrg: function(store, operation) {
                operation.params = operation.params || {};
                operation.params.manorgOnly = true;
                operation.params.showNotValid = false;
            }
        }
    ],

    mainView: 'manorg.contract.Grid',
    mainViewSelector: 'manorgContractGrid',

    jskTsjEditWindowSelector: '#jskTsjContractEditWindow',
    ownersEditWindowSelector: '#manorgContractOwnersEditWindow',
    transferEditWindowSelector: '#manorgTransferEditWindow',

    init: function () {
       var me = this,
            actions = {};

        me.getStore('manorg.contract.Transfer').on('beforeload', me.onBeforeLoadRelation, me);
       
        actions['manorgContractGrid'] = { 'afterlayout': { fn: this.onMainViewAfterRender, scope: this } };
        
        me.control(actions);
        me.callParent(arguments);

    },
    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('manorgContractGrid'),
            store = view.getStore(),
            args = me.processArgs();

        B4.Ajax.request(B4.Url.action('Get', 'ManagingOrganization', {
            id: id
        })).next(function (response) {
            var obj = Ext.JSON.decode(response.responseText);

            if (!obj) {
                Ext.Msg.alert('Внимание!', 'Не удалось определить управляющую организацию!');
                return;
            }

            me.setContextValue(view, 'TypeManagement', obj.data.TypeManagement);
           
        }).error(function () {
        });

        if (view && view.headerFilterPlugin) {
            view.headerFilterPlugin.clearFilters();
        }
        
        store.filters.clear();
        debugger;
        store.filter([
            { property: 'manorgId', value: id },
            { property: 'showClose', value: me.params.showCloseAppeals },
            { property: 'fromManagOrg', value: true }]);
        store.on('beforeload', me.onBeforeLoad, me);

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.setContextValue(view, 'fromManagOrg', true);

        me.application.deployView(view, 'manorgId_info');

        store.load();

        if (args && parseInt(args.contractId) > 0 && parseInt(args.type) > 0) {
            me.edit(id, parseInt(args.contractId), parseInt(args.type));

            if (args.newToken) {
                me.application.redirectTo(args.newToken);
            }
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            view = this.getMainView();
        if (me.params) {
            operation.params.manorgId = me.getContextValue(me.getMainView(), 'manorgId')
            operation.params.fromManagOrg = me.getContextValue(me.getMainView(), 'fromManagOrg')
        }
    },

    view: function(id) {
        this.index(id);
        this.application.getState().slice('managingorganizationedit/' + id);
    },

    edit: function (id, contractId, type) {
        var me = this,
            aspect,
            aspectName = null,
            baseModel = null;

        switch (type) {
            case B4.enums.TypeContractManOrgRealObj.ManagingOrgJskTsj:
                aspectName = 'manorgContractTransferWindowAspect';
                baseModel = me.getModel('manorg.contract.Transfer');
                break;
            case B4.enums.TypeContractManOrgRealObj.ManagingOrgOwners:
                aspectName = 'manorgContractOwnersGridWindowAspect';
                baseModel = me.getModel('manorg.contract.Owners');
                break;
            case B4.enums.TypeContractManOrgRealObj.JskTsj:
                aspectName = 'manorgJskTsjContractGridWindowAspect';
                baseModel = me.getModel('manorg.contract.JskTsj');
                break;
        }

        (contractId && baseModel && aspectName) ? baseModel.load(contractId, {
            success: function (record) {
                aspect = me.getAspect(aspectName);
                aspect.editRecord(record);
            },
            scope: aspect
        }) : function () {
            Ext.Msg.alert('Ошибка', 'Не найдено управление домом');
        };
    },

    onMainViewAfterRender: function () {
        var me = this,
            type = me.getContextValue(me.getMainView(), 'TypeManagement'),
            mainView = this.getMainView() || Ext.widget('manorgContractGrid');

        switch (type) {
            case 40:
            case 20:
                { // ТСЖ/ЖСК
                    mainView.down('#btnAddJskTsj').show();
                    mainView.down('#btnAddManOrgOwners').hide();
                    mainView.down('#gcIsTransManag').show();
                }
                break;
            case 10:
            case 100:
                { // УК
                    mainView.down('#btnAddJskTsj').hide();
                    mainView.down('#btnAddManOrgOwners').show();
                    mainView.down('#gcIsTransManag').hide();
                }
                break;
        }
    },

    onBeforeLoadRelation: function (store, operation) {
        debugger;
        if (this.params) {
            operation.params.contractId = this.getContractId();
            operation.params.fromManagOrg = true;
            operation.params.showClose = this.params.showCloseAppeals;
        }
    },

    setContract: function (id) {
        this.setContextValue(this.getMainView(), 'contractId', id);
    },

    getContractId: function () {
        return this.getContextValue(this.getMainView(), 'contractId');
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