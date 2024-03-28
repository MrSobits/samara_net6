Ext.define('B4.controller.InformationOnContracts', {
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.informationoncontracts.State'
    ],

    models:
    [
        'DisclosureInfo',
        'InformationOnContracts'
    ],
    stores: ['InformationOnContracts'],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    views: [
        'informationoncontracts.EditPanel',
        'informationoncontracts.EditWindow'
    ],

    mainView: 'informationoncontracts.EditPanel',
    mainViewSelector: '#informationOnContractsEditPanel',

    aspects: [
        {
            xtype: 'informationoncontractsstateperm',
            editFormAspectName: 'informationOnContractsEditPanelAspect'
        },
        {
            xtype: 'gkheditpanel',
            name: 'informationOnContractsEditPanelAspect',
            modelName: 'DisclosureInfo',
            editPanelSelector: '#informationOnContractsEditPanel',

            listeners: {
                beforesave: function (asp, record) {
                    record.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                }
            },
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' b4savebutton'] = { 'click': { fn: this.onSaveBtnClick, scope: this } };
                actions[this.editPanelSelector + ' b4addbutton'] = { 'click': { fn: this.onAddBtnClick, scope: this} };
                actions[this.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: this.onUpdateBtnClick, scope: this} };
                actions[this.editPanelSelector + ' #cbContractsAvailability'] = { 'change': { fn: this.setDisableGrid, scope: this} };
            },
            onSaveBtnClick: function () {
                var me = this,
                    record = me.controller.getMainView().getRecord();

                if (record) {
                    this.getPanel().getForm().updateRecord();
                    this.saveRecordHasUpload(record);
                }
            },
            onAddBtnClick: function () {
                this.controller.getAspect('informationOnContractsGridWindowAspect').editRecord();
            },
            onUpdateBtnClick: function () {
                this.setData(this.controller.params.disclosureInfoId);
                if (Ext.ComponentQuery.query('#cbContractsAvailability')[0].value == 10) {
                    this.controller.getAspect('informationOnContractsGridWindowAspect').updateGrid();
                }
            },

            setDisableGrid: function (field) {

                var grid = Ext.ComponentQuery.query('#informationOnContractsGrid')[0];
                var editPanel = Ext.ComponentQuery.query('#informationOnContractsEditPanel')[0];
                var addInformationOnContractsButton = Ext.ComponentQuery.query('#addInformationOnContractsButton')[0];

                if (field.getValue() != 10) {
                    grid.setDisabled(true);
                    grid.getStore().removeAll();
                    addInformationOnContractsButton.hide();
                }
                else {
                    grid.setDisabled(false);
                    grid.getStore().load();
                    addInformationOnContractsButton.show();
                }
                editPanel.doLayout();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'informationOnContractsGridWindowAspect',
            gridSelector: '#informationOnContractsGrid',
            editFormSelector: '#informationOnContractsEditWindow',
            storeName: 'InformationOnContracts',
            modelName: 'InformationOnContracts',
            editWindowView: 'informationoncontracts.EditWindow',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' #sflRealityObject'] = { 'beforeload': { fn: this.controller.onBeforeLoadRealityObject, scope: this } };
            },            
            listeners: {
                beforesave: function (asp, record) {
                    record.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                    return true;
                }
            }
        }
    ],

    init: function () {
        var actions = {};

        this.getStore('InformationOnContracts').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
        actions[this.mainViewSelector] = { 'activate': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);
    },

    onMainViewAfterRender: function () {
        var me = this,
            asp = me.getAspect('informationOnContractsEditPanelAspect');

        if (asp) {
            me.mask('Загрузка');
            asp.getModel().load(me.params.disclosureInfoId, {
                success: function (rec) {
                    this.setPanelData(rec);
                    me.unmask();
                },
                scope: asp
            });
        }
    },

    onLaunch: function () {

        if (this.params) {
            this.getAspect('informationOnContractsEditPanelAspect').setData(this.params.disclosureInfoId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoId = this.params.disclosureInfoId;
        }
    },
    
    onBeforeLoadRealityObject: function(field, options) {
        options.params = {};
        options.params.disclosureInfoId = this.controller.params.disclosureInfoId;
    }
});
