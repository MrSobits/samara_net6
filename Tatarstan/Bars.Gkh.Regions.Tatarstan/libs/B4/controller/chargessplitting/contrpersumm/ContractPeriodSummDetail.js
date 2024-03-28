Ext.define('B4.controller.chargessplitting.contrpersumm.ContractPeriodSummDetail', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.StateButton',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    stores: [
       'chargessplitting.contrpersumm.ContractPeriodSumm'
    ],

    views: [
        'chargessplitting.contrpersumm.detail.Panel',
        'chargessplitting.contrpersumm.detail.Grid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'chargessplitting.contrpersumm.detail.Panel',
    mainViewSelector: 'contrpersummdetailpanel',

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'ContractPeriodSummDetailGridAspect',
            storeName: 'chargessplitting.contrpersumm.ContractPeriodSummDetail',
            modelName: 'chargessplitting.contrpersumm.ContractPeriodSummDetail',
            gridSelector: 'contractperiodsummarydetailgrid',
            saveButtonSelector: 'contrpersummdetailpanel b4savebutton'
        },
        {
            xtype: 'statebuttonaspect',
            name: 'UoStateButtonAspect',
            stateButtonSelector: 'button[name=btnUoState]',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    this.controller.loadFormData();
                }
            }
        },
        {
            xtype: 'statebuttonaspect',
            name: 'RsoStateButtonAspect',
            stateButtonSelector: 'button[name=btnRsoState]',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    this.controller.loadFormData();
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'ContractPeriodSummDetailPrintAspect',
            buttonSelector: 'gkhbuttonprint[name=PrintButton]',
            codeForm: 'ContractPeriodSummDetailReport',
            printController: 'GkhReport',
            printAction: 'ReportPrint',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    param = {
                        reportId: 'ContractPeriodSummDetailReport',
                        contractId: me.controller.getContextValue(view, 'contractId')
                    };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            name: 'contractPeriodSummDetailPermissionAspect',
            applyOn: {
                event: 'beforerender',
                selector: 'contractperiodsummarydetailgrid'
            },
            permissions: [
                //печать просмотр
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Print_View',
                    applyTo: 'gkhbuttonprint',
                    selector: 'contrpersummdetailpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                //поля просмотр
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Uo_View',
                    applyTo: 'textfield[name=ChargedResidents]',
                    selector: 'contrpersummdetailpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Uo_View',
                    applyTo: 'textfield[name=PaidResidents]',
                    selector: 'contrpersummdetailpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Uo_View',
                    applyTo: 'textfield[name=TransferredPubServOrg]',
                    selector: 'contrpersummdetailpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Rso_View',
                    applyTo: 'textfield[name=ChargedManOrg]',
                    selector: 'contrpersummdetailpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Rso_View',
                    applyTo: 'textfield[name=PaidManOrg]',
                    selector: 'contrpersummdetailpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Rso_View',
                    applyTo: 'textfield[name=BilledPubServOrg]',
                    selector: 'contrpersummdetailpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Rso_View',
                    applyTo: 'textfield[name=PaidPubServOrg]',
                    selector: 'contrpersummdetailpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                //статусы просмотр
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Uo_View',
                    applyTo: 'buttongroup[name=uoState]',
                    selector: 'contrpersummdetailpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Rso_View',
                    applyTo: 'buttongroup[name=rsoState]',
                    selector: 'contrpersummdetailpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                //статусы редактирование
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Uo_Edit',
                    applyTo: 'buttongroup[name=uoState] button',
                    selector: 'contrpersummdetailpanel'
                },
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Rso_Edit',
                    applyTo: 'buttongroup[name=rsoState] button',
                    selector: 'contrpersummdetailpanel'
                },
                //столбцы просмотр
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Uo_View',
                    applyTo: 'gridcolumn[name=ManOrgGroup]',
                    selector: 'contractperiodsummarydetailgrid',
                    applyBy: function (column, allowed) {
                        column.allowShow = allowed;
                    }
                },
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Rso_View',
                    applyTo: 'gridcolumn[name=PubServOrgGroup]',
                    selector: 'contractperiodsummarydetailgrid',
                    applyBy: function (column, allowed) {
                        column.allowShow = allowed;
                    }
                },
                //столбцы редактирование
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Uo_Edit',
                    applyTo: 'gridcolumn[name=ManOrgGroup]',
                    selector: 'contractperiodsummarydetailgrid',
                    applyBy: function (column, allowed) {
                        var subColumns = column.getGridColumns();
                        subColumns.forEach(function (col) { col.allowEdit = allowed });
                    }
                },
                {
                    name: 'Gkh.ContractPeriodSumm.Detail.Rso_Edit',
                    applyTo: 'gridcolumn[name=PubServOrgGroup]',
                    selector: 'contractperiodsummarydetailgrid',
                    applyBy: function (column, allowed) {
                        var subColumns = column.getGridColumns();
                        subColumns.forEach(function (col) { col.allowEdit = allowed });
                    }
                }
            ]
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions['contractperiodsummarydetailgrid'] = {
            'store.beforeload': { fn: me.onBeforeLoadStore, scope: me },
            'store.load': { fn: me.onAfterLoadStore, scope: me },
            'resize': { fn: me.applyColumnPermission, scope: me }
        };

        me.control(actions);

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contrpersummdetailpanel');

        me.getAspect('ContractPeriodSummDetailPrintAspect').loadReportStore();

        me.bindContext(view);
        me.setContextValue(view, 'contractId', id);
        me.application.deployView(view);

        view.down('contractperiodsummarydetailgrid').getStore().load();
    },

    onBeforeLoadStore: function (store, operation) {
        var me = this,
            view = me.getMainView();

        operation.params.contractId = me.getContextValue(view, 'contractId');
    },

    loadFormData: function () {
        var me = this,
            view = me.getMainView(),
            form = view.down('panel[name=periodSummForm]'),
            model = me.getModel('chargessplitting.contrpersumm.ContractPeriodSumm'),
            id = me.getContextValue(view, 'contractId');

        me.mask(form);

        model.load(id, {
            success: function (rec) {
                form.getForm().setValues(rec.getData());

                me.getAspect('RsoStateButtonAspect').setStateData(rec.get('RsoSummId'), rec.get('RsoState'));
                me.getAspect('UoStateButtonAspect').setStateData(rec.get('UoSummId'), rec.get('UoState'));
            },
            callback: function() {
                me.unmask(form);
            },
            scope: me
        });
    },

    applyColumnPermission: function () {
        var grid = this.getMainView(),
            rsoGroup = grid.down('gridcolumn[name=PubServOrgGroup]'),
            uoGroup = grid.down('gridcolumn[name=ManOrgGroup]');

        rsoGroup.setVisible(rsoGroup.allowShow);
        uoGroup.setVisible(uoGroup.allowShow);
    },

    onAfterLoadStore: function () {
        var me = this;

        me.loadFormData();
        me.applyColumnPermission();
    }
});