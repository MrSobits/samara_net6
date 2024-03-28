Ext.define('B4.controller.DecisionNoticeRegister', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu'
    ],

    models: ['SpecialAccountDecisionNotice', 'RealityObject'],

    stores: ['DecisionNoticeRegister'],

    views:['decisionnoticereg.Grid'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
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
        this.control({
            'decisionnoticereggrid': {
                rowaction: {
                    fn: this.onRowAction,
                    scope: this
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

        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('decisionnoticereggrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    onRowAction: function (grid, action, record) {
        var me = this,
            portal,
            model,
            params,
            ro;

        me.mask('Загрузка', me.getMainView());
        if (action.toLowerCase() === 'edit') {
            portal = me.getController('PortalController');
            model = me.getModel('RealityObject');

            ro = record.get('RealityObject');

            params = new model({ Id: ro.Id, Address: ro.Address });

            params.defaultController = 'B4.controller.realityobj.OwnerProtocol';
            params.defaultParams = { realityObjectId: params.get('Id'), decisId: record.get('SpecialAccountDecision') };

            portal.loadController('B4.controller.realityobj.Navigation', params, portal.containerSelector, function () { me.unmask(); });
        }
    }
});