Ext.define('B4.controller.MembershipUnions', {
    extend: 'B4.base.Controller',
 views: [ 'membershipunions.EditPanel' ], 

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel'
    ],

    models: ['DisclosureInfo',
             'ManagingOrganization'],
    
    stores: ['MembershipUnions'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    mainView: 'membershipunions.EditPanel',
    mainViewSelector: '#membershipUnionsEditPanel',

    aspects: [
    {
        xtype: 'gkheditpanel',
        name: 'membershipUnionsEditPanelAspect',
        modelName: 'DisclosureInfo',
        editPanelSelector: '#membershipUnionsEditPanel',
        otherActions: function (actions) {
            actions[this.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: this.onUpdateButtonClick, scope: this} };
            actions[this.editPanelSelector + ' #membershipUnionsButton'] = { 'click': { fn: this.showEditForm, scope: this} };
            actions[this.editPanelSelector + ' #cbMembershipUnions'] = { 'change': { fn: this.changeMembershipUnions, scope: this } };
        },
        listeners: {
            beforesetpaneldata: function (asp, rec) {
              
                B4.Ajax.request(B4.Url.action('Get', 'DisclosureInfo', {
                    id: asp.controller.params.disclosureInfoId
                })).next(function (response) {
                    var obj = Ext.decode(response.responseText);
                    asp.controller.params.ManagingOrgId = obj.data.ManagingOrgId;

                    return true;
                }).error(function () {
                    Ext.Msg.alert('Ошибка', 'Не удалось получить данные');
                });
            }
        },
        //показываем членство в объединениях
        showEditForm: function () {
            this.controller.application.redirectTo('managingorganizationedit/' + this.controller.params.ManagingOrgId + '/membership/');
        },

        onUpdateButtonClick: function () {
            this.setData(this.controller.params.disclosureInfoId);
            if (Ext.ComponentQuery.query('#cbMembershipUnions')[0].value == 10) {
                this.controller.getStore('MembershipUnions').load();
            }
        },

        changeMembershipUnions: function (field, newValue, oldValue) {
            //При первом заходе не сохраняем
            if (oldValue) {
                this.saveRequestHandler();
            }

            this.setDisableGrid(field);
        },

        setDisableGrid: function (field) {
            var grid = Ext.ComponentQuery.query('#membershipUnionsGrid')[0];

            if (field.getValue() != 10) {
                grid.setDisabled(true);
                grid.getStore().removeAll();
            }
            else {
                grid.setDisabled(false);
                grid.getStore().load();
            }
        }
    }],

    init: function () {
        this.getStore('MembershipUnions').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('membershipUnionsEditPanelAspect').setData(this.params.disclosureInfoId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoId = this.params.disclosureInfoId;
        }
    }
});