Ext.define('B4.controller.TerminateContract', {
    extend: 'B4.base.Controller',
 views: [ 'terminatecontract.EditPanel' ], 

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.terminatecontract.State'
    ],

    models: [
        'DisclosureInfo',
        'ManagingOrganization'
    ],
    
    stores: ['TerminateContract'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    mainView: 'terminatecontract.EditPanel',
    mainViewSelector: '#terminateContractEditPanel',

    aspects: [
    {
        xtype: 'terminatecontractstateperm',
        editFormAspectName: 'terminateContractEditPanelAspect'
    },
    {
        xtype: 'gkheditpanel',
        name: 'terminateContractEditPanelAspect',
        modelName: 'DisclosureInfo',
        editPanelSelector: '#terminateContractEditPanel',
        otherActions: function(actions) {
            actions[this.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: this.onUpdateButtonClick, scope: this } };
            actions[this.editPanelSelector + ' #realityObjButton'] = { 'click': { fn: this.showEditForm, scope: this } };
            actions[this.editPanelSelector + ' #cbTerminateContract'] = { 'change': { fn: this.changeTerminateContract, scope: this } };
        },
        listeners: {
            beforesetpaneldata: function (asp, rec, panel) {

                B4.Ajax.request(B4.Url.action('Get', 'DisclosureInfo', {
                    id: asp.controller.params.disclosureInfoId
                })).next(function (response) {
                    var obj = Ext.decode(response.responseText);
                    asp.controller.params.ManagingOrgId = obj.data.ManagingOrgId;
                    asp.controller.params.TypeManagement = obj.data.TypeManagement;
                    
                    var grid = panel.down('#terminateContractGrid');
                    if (rec.get('TerminateContract') != 10) {
                        grid.setDisabled(true);
                    }
                    else {
                        grid.setDisabled(false);
                    }

                    return true;
                }).error(function () {
                    Ext.Msg.alert('Ошибка', 'Не удалось получить данные');
                });
            }
        },
        
        changeTerminateContract: function (field, newValue, oldValue) {
            //При первом заходе не сохраняем
            if (oldValue) {
                this.saveRequestHandler();
            }

            this.setDisableGrid(field);
        },

        setDisableGrid: function (field) {
            var grid = Ext.ComponentQuery.query('#terminateContractGrid')[0];

            if (field.getValue() != 10) {
                grid.setDisabled(true);
                grid.getStore().removeAll();
            }
            else {
                grid.setDisabled(false);
                grid.getStore().load();
            }
        },

        //показываем управление жилыми домами
        showEditForm: function () {
            this.controller.application.getRouter().redirectTo('managingorganizationedit/' + this.controller.params.ManagingOrgId + '/contractView');
        },

        onUpdateButtonClick: function () {
            this.setData(this.controller.params.disclosureInfoId);
            if (Ext.ComponentQuery.query('#cbTerminateContract')[0].value == 10) {
                this.controller.getStore('TerminateContract').load();
            }
        }
    }],

    init: function () {
        this.getStore('TerminateContract').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {

        if (this.params) {
            this.getAspect('terminateContractEditPanelAspect').setData(this.params.disclosureInfoId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoId = this.params.disclosureInfoId;
        }
    }
});