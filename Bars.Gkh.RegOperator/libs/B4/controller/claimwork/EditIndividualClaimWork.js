Ext.define('B4.controller.claimwork.EditIndividualClaimWork', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkDocument',
        'B4.aspects.ClaimWorkButtonPrintAspect',
        'B4.aspects.IndividualClamworkPermAspect',
        'B4.enums.LawsuitDocumentType',
        'B4.enums.LawsuitFactInitiationType',
        'B4.enums.DebtorType',
        'B4.aspects.ClaimWorkDocCreateButton',
        'B4.aspects.DebtorClaimWorkEditAspect',
        'B4.store.regop.owner.IndividualAccountOwner',
        'B4.aspects.permission.Lawsuit',
        'B4.aspects.ClaimWorkButtonPrintAspect'

    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'claimwork.IndividualClaimWork'
    ],

    views: [
        'claimwork.IndividualEditPanel'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'individualclaimworkeditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'individualclaimworkperm'
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'individualClaimWorkCreateButtonAspect',
            buttonSelector: 'individualclaimworkeditpanel acceptmenubutton',
            containerSelector: 'individualclaimworkeditpanel'
        },
        {
            xtype: 'debtorclaimworkeditaspect',
            name: 'individualClaimWorkEditPanelAspect',
            editPanelSelector: 'individualclaimworkeditpanel',
            modelName: 'claimwork.IndividualClaimWork',
            listeners: {
                savesuccess: function() {
                    this.controller.getAspect('individualClaimWorkCreateButtonAspect').reloadMenu();
                }
            }
         },       
         {
            xtype: 'claimworkbuttonprintaspect',
            name: 'accountPrintAspect',
            buttonSelector: 'claimworkaccountdetailgrid gkhbuttonprint[action=Print]',
            codeForm: 'AccountNotification, CourtClaimAccount, LawSuitClaimAccount, LawSuitAccount, PretensionAccount',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    grid = view.down('claimworkaccountdetailgrid'),
                    records = grid.getSelectionModel().getSelection(),
                    recIds = [],
                    param = { claimWorkId: me.controller.getContextValue(view, 'claimWorkId') };

                Ext.each(records,
                    function (rec) {
                        recIds.push(rec.get('Id'));
                    });

                Ext.apply(me.params, { recIds: Ext.JSON.encode(recIds) });
                me.params.userParams = Ext.JSON.encode(param);
            },
            printReport: function (itemMenu) {
                var me = this,
                    frame = Ext.get('downloadIframe');
                if (frame != null) {
                    Ext.destroy(frame);
                }

                me.getUserParams(itemMenu.actionName);

                if (Ext.JSON.decode(me.params.recIds).length == 0) {
                    Ext.Msg.alert('Ошибка', 'Необходимо выбрать хотя бы одну запись для печати');
                    return;
                }

                Ext.apply(me.params, { reportId: itemMenu.actionName });

                me.mask('Обработка...');
                B4.Ajax.request({
                    url: B4.Url.action('ReportAccountPrint', 'ClaimWorkReport'),
                    params: me.params,
                    timeout: 9999999
                })
                    .next(function (resp) {
                        var tryDecoded;

                        me.unmask();
                        try {
                            tryDecoded = Ext.JSON.decode(resp.responseText);
                        } catch (e) {
                            tryDecoded = {};
                        }

                        var id = resp.data ? resp.data : tryDecoded.data;

                        if (id > 0) {
                            Ext.DomHelper.append(document.body,
                                {
                                    tag: 'iframe',
                                    id: 'downloadIframe',
                                    frameBorder: 0,
                                    width: 0,
                                    height: 0,
                                    css: 'display:none;visibility:hidden;height:0px;',
                                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                                });

                            me.fireEvent('onprintsucess', me);
                        }
                    })
                    .error(function (err) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка', err.message || err);
                    });
            },
            onBeforeLoadReportStore: function (store, operation) {
                operation.params = {};
                operation.params.codeForm = this.codeForm;
                operation.params.type = this.controller.getDebtorType();
                operation.params.claimWorkId = this.controller.getContextValue(this.controller.getMainView(), 'claimWorkId');
            }
        }
    ],

    getDebtorType: function () {
        var me = this,
            type = me.getContextValue(me.getMainComponent(), 'type');

        if (type === 'IndividualClaimWork') {
            return B4.enums.DebtorType.Individual;
        }
        if (type === 'LegalClaimWork') {
            return B4.enums.DebtorType.Legal;
        }

        return null;
    },

    index: function (type, id) {
        var me = this,
            view = me.getMainView() || Ext.widget('individualclaimworkeditpanel');

        me.bindContext(view);
        
        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.application.deployView(view, 'claim_work');
        me.getAspect('accountPrintAspect').loadReportStore();

        me.getAspect('individualClaimWorkCreateButtonAspect').setData(id, type);
        me.getAspect('individualClaimWorkEditPanelAspect').setData(id, type);
    }
});