Ext.define('B4.controller.DecisionNoticeRegister', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu'
    ],

    models: [
        'SpecialAccountDecisionNotice',
        'RealityObject'
    ],

    stores: [
        'DecisionNoticeRegister'
    ],

    views: [
        'decisionnoticereg.Grid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'decisionnoticereggrid'
        }
    ],
    
    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'decNoticeRegisterStateTransferAspect',
            gridSelector: 'decisionnoticereggrid',
            stateType: 'ovrhl_decision_notice',
            menuSelector: 'decNoticeGridStateMenu'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'decisionNoticeButtonExportAspect',
            gridSelector: 'decisionnoticereggrid',
            buttonSelector: 'decisionnoticereggrid button[action="export"]',
            controllerName: 'SpecialAccountDecisionNotice',
            actionName: 'Export'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'decisionnoticereggrid': {
                rowaction: {
                    fn: me.onRowAction,
                    scope: me
                }
            },
            'decisionnoticereggrid b4updatebutton': {
                click: {
                    fn: function (btn) {
                        btn.up('decisionnoticereggrid').getStore().load();
                    }
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('decisionnoticereggrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    onRowAction: function (grid, action, record) {
        var me = this;

        if (action.toLowerCase() === 'edit' && record.get('RealityObject')) {
            me.application.redirectTo(Ext.String.format('realityobjectedit/{0}', record.get('RealityObject').Id));
        } else {
            B4.QuickMsg.msg('Предупреждение', 'Идентификатор дома в базе не найден', 'warning');
        }
    }
});