Ext.define('B4.controller.utilityclaimwork.EditUtilityDebtor', {
    extend: 'B4.base.Controller',

    requires: [

        'B4.aspects.StateContextButton',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkDocCreateButton'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'utilityclaimwork.UtilityDebtor'
    ],

    views: [
        'utilityclaimwork.EditPanel'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'utilitydebtorclaimworkeditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.UtilityDebtor.Debt.Save', applyTo: 'b4savebutton', selector: 'utilitydebtorclaimworkeditpanel'
                },
                {
                    name: 'Clw.ClaimWork.UtilityDebtor.Debt.Save', applyTo: 'button[name=State]', selector: 'utilitydebtorclaimworkeditpanel'
                }
            ]
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'utilityDebtorClaimWorkCreateButtonAspect',
            buttonSelector: 'utilitydebtorclaimworkeditpanel acceptmenubutton',
            containerSelector: 'utilitydebtorclaimworkeditpanel'
        },
        {
            xtype: 'gkheditpanel',
            name: 'utilityDebtorClaimWorkEditPanelAspect',
            editPanelSelector: 'utilitydebtorclaimworkeditpanel',
            modelName: 'utilityclaimwork.UtilityDebtor',
            
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    var me = this;                   
                    me.controller.getAspect('utilityDebtorButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                },
                savesuccess: function () {
                    this.controller.getAspect('utilityDebtorClaimWorkCreateButtonAspect').reloadMenu();
                }
            }
        },
        {
            /*
             * Вешаем аспект смены статуса в карточке редактирования
             */
            xtype: 'statecontextbuttonaspect',
            name: 'utilityDebtorButtonAspect',
            stateButtonSelector: 'utilitydebtorclaimworkeditpanel button[name=State]',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    asp.setStateData(entityId, newState);
                }
            }
        }
    ],

    init: function () {
        var me = this;
   
        me.callParent(arguments);
    },

    index: function (type, id) {
        var me = this,
            view = me.getMainView() || Ext.widget('utilitydebtorclaimworkeditpanel');

        me.bindContext(view);

        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.application.deployView(view, 'claim_work');

        me.getAspect('utilityDebtorClaimWorkEditPanelAspect').setData(id);
        me.getAspect('utilityDebtorClaimWorkCreateButtonAspect').setData(id, type);
    }
});