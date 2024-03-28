Ext.define('B4.controller.GeneralData', {
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.InlineGrid'
    ],

    models:
    [
        'DisclosureInfo',
        'manorg.WorkMode',
        'Contragent',
        'ManagingOrganization'
    ],

    stores:
    [
        'generaldata.ManOrgWorkMode',
        'generaldata.ManOrgReceptionCitizens',
        'generaldata.ManOrgDispatcherWork'
    ],
    
    views: [
        'generaldata.EditPanel'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    mainView: 'generaldata.EditPanel',
    mainViewSelector: '#generalDataEditPanel',

    aspects: [
    {
        xtype: 'inlinegridaspect',
        name: 'manorgWorkModeDiGridAspect',
        storeName: 'generaldata.ManOrgWorkMode',
        modelName: 'manorg.Mode',
        gridSelector: '#workModeGridDi'
    },
    {
        xtype: 'inlinegridaspect',
        name: 'manorgReceptionCitizensDiGridAspect',
        storeName: 'generaldata.ManOrgReceptionCitizens',
        modelName: 'manorg.WorkMode',
        gridSelector: '#receptionCitizensGridDi'
    },
    {
        xtype: 'inlinegridaspect',
        name: 'manorgDispatcherWorDikGridAspect',
        storeName: 'generaldata.ManOrgDispatcherWork',
        modelName: 'manorg.WorkMode',
        gridSelector: '#dispatcherWorkGridDi'
    },
    {
        xtype: 'gkheditpanel',
        name: 'generalDataEditPanelAspect',
        editPanelSelector: '#generalDataEditPanel',
        modelName: 'DisclosureInfo',
        otherActions: function (actions) {
            actions[this.editPanelSelector + ' #saveButton'] = { 'click': { fn: this.saveRequestHandler, scope: this} };
            actions[this.editPanelSelector + ' #updateButton'] = { 'click': { fn: this.onUpdateButtonClick, scope: this} };
            actions[this.editPanelSelector + ' #contragentButton'] = { 'click': { fn: this.showEditForm, scope: this} };
            actions[this.editPanelSelector + ' #managingOrgButton'] = { 'click': { fn: this.showEditForm, scope: this} };
        },

        listeners: {
            beforesetpaneldata: function (asp, rec, panel) {
                var taFioMemberAudit = panel.down('#taFioMemberAudit');
                var taFioMemberManagement = panel.down('#taFioMemberManagement');
                var shareSfMo = panel.down('#shareSfMo');
                var form988PeriodStart = new Date('2015-01-01T00:00:00');
                var taDispatchFile = panel.down('#tfDispatchFile');

                //для управляющей организации с типом УК устава нет, поэтому скрываем поле
                if (rec.get('TypeManagement') === 10) {
                    taDispatchFile.hide();
                    taDispatchFile.allowBlank = true;                    
                }
                else {
                    taDispatchFile.show();
                    taDispatchFile.allowBlank = false;
                }

                if (rec.get('TypeManagement') == 20 || rec.get('TypeManagement') == 40) {
                    taFioMemberAudit.show();
                    taFioMemberManagement.show();
                }
                else {
                    taFioMemberAudit.hide();
                    taFioMemberManagement.hide();
                }
                
                if (shareSfMo) {
                    if (new Date(asp.controller.params.recDi.PeriodDi.DateStart) >= form988PeriodStart) {
                        shareSfMo.show();
                    } else {
                        shareSfMo.hide();
                    }
                }

                asp.controller.params.ContragentId = rec.get('ContragentId');
                asp.controller.params.ManagingOrgId = rec.get('ManagingOrgId');
                
                asp.controller.getStore('generaldata.ManOrgWorkMode').load();
                asp.controller.getStore('generaldata.ManOrgReceptionCitizens').load();
                asp.controller.getStore('generaldata.ManOrgDispatcherWork').load();
            }
        },

        onUpdateButtonClick: function () {
            this.setData(this.controller.params.disclosureInfoId);
        },

        //показываем контрагента или упр орг для редактирования
        showEditForm: function (btn) {
            var me = this;
            var params;
            switch (btn.itemId) {
                case 'contragentButton':
                    {
                        Ext.History.add('contragentedit/' + me.controller.params.ContragentId + '/');
                        break;
                    }
                case 'managingOrgButton':
                    {
                        Ext.History.add('managingorganizationedit/' + me.controller.params.ManagingOrgId + '/');
                        break;
                    }
            }
        }
    }],

    init: function () {
        this.getStore('generaldata.ManOrgWorkMode').on('beforeload', this.onBeforeLoad, this, 10);
        this.getStore('generaldata.ManOrgReceptionCitizens').on('beforeload', this.onBeforeLoad, this, 20);
        this.getStore('generaldata.ManOrgDispatcherWork').on('beforeload', this.onBeforeLoad, this, 30);
        
        this.callParent(arguments);
    },

    onLaunch: function () {

        if (this.params) {
            this.getAspect('generalDataEditPanelAspect').setData(this.params.disclosureInfoId);
        }
    },

    onBeforeLoad: function (store, operation, type) {
        if (this.params) {
            operation.params.manorgId = this.params.ManagingOrgId;
            operation.params.typeMode = type;
        }
    }
});