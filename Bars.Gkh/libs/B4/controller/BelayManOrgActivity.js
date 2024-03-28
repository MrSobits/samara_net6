Ext.define('B4.controller.BelayManOrgActivity', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.BelayManOrgActivity'
    ],

    controllers: [
        'B4.controller.belaypolicy.Navigation'
    ],

    models: [
        'BelayManOrgActivity',
        'BelayPolicy'
    ],
    
    stores: [
        'BelayManOrgActivity',
        'BelayPolicy'
    ],
    
    views: [
        'belaymanorgactivity.Grid',
        'belaymanorgactivity.EditWindow',
        'belaypolicy.AddWindow',
        'belaypolicy.Grid'
    ],

    aspects: [
        {
            xtype: 'belaymanorgactivityperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'belayManOrgActivityGridWindowAspect',
            gridSelector: 'belayManOrgActivityGrid',
            editFormSelector: '#belayManOrgActivityEditWindow',
            storeName: 'BelayManOrgActivity',
            modelName: 'BelayManOrgActivity',
            editWindowView: 'belaymanorgactivity.EditWindow',
            otherActions: function (actions) {
                actions[this.gridSelector] = {
                    'rowaction': { fn: this.rowAction, scope: this },
                    'itemdblclick': { fn: this.rowDblClick, scope: this },
                    'gridaction': { fn: this.gridAction, scope: this },
                    'destroy': { fn: this.gridDestroy, scope: this }
                };
            },
            gridDestroy: function () {
                var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                if (editWindow) {
                    editWindow.close();
                }
            },
            getForm: function () {
                var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                if (!editWindow) {
                    editWindow = this.controller.getView(this.editWindowView).create();

                    var cmp = this.controller.getMainComponent();

                    if (cmp) {
                        cmp.add(editWindow);
                    }
                }

                return editWindow;
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record);
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record);
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'belayManOrgActivityButtonExportAspect',
            gridSelector: 'belayManOrgActivityGrid',
            buttonSelector: '#belayManOrgActivityGrid #btnExport',
            controllerName: 'BelayManOrgActivity',
            actionName: 'Export'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'belayPolicyGridWindowAspect',
            gridSelector: 'belayPolicyGrid',
            editFormSelector: '#belayPolicyAddWindow',
            storeName: 'BelayPolicy',
            modelName: 'BelayPolicy',
            editWindowView: 'belaypolicy.AddWindow',
            controllerEditName: 'B4.controller.belaypolicy.Navigation',
            listeners: {
                getdata: function (asp, record) {
                    record.data.BelayManOrgActivity = this.controller.belayManOrgActivId;
                }
            }
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'belaymanorgactivity.Grid',
    mainViewSelector: 'belayManOrgActivityGrid',

    //это селектор окна который используется в дальнейшем в редактировании
    editWindowSelector: '#belayManOrgActivityEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'belayManOrgActivityGrid'
        }
    ],

    init: function () {
        this.getStore('BelayPolicy').on('beforeload', this.onBeforeLoadParent, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('belayManOrgActivityGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('BelayManOrgActivity').load();
    },

    setCurrentId: function (record) {
        this.belayManOrgActivId = record.getId();

        var editWindow =  Ext.ComponentQuery.query(this.editWindowSelector)[0];
        
        var storePolicy = this.getStore('BelayPolicy');
        storePolicy.removeAll();

        if (this.belayManOrgActivId > 0) {
            this.manOrgId = record.get('ManagingOrganization').Id;
            editWindow.down('#belayPolicyGrid').setDisabled(false);
            storePolicy.load();
        } else {
            editWindow.down('#belayPolicyGrid').setDisabled(true);
        }
    },

    onBeforeLoadParent: function (store, operation) {
        operation.params.belayManOrgActivId = this.belayManOrgActivId;
    }
});