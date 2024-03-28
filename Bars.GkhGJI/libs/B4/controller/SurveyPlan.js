Ext.define('B4.controller.SurveyPlan', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
        'B4.aspects.StateContextMenu',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    models: ['SurveyPlan'],
    stores: ['SurveyPlan'],
    views: [
        'surveyplan.Grid',
        'surveyplan.AddWindow'
    ],

    mainView: 'surveyplan.Grid',
    mainViewSelector: 'surveyPlanGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'surveyPlanGrid'
        },
        {
            ref: 'addView',
            selector: 'surveyplanaddwindow'
        }
    ],

    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'surveyPlanStateTransferAspect',
            gridSelector: 'surveyPlanGrid',
            menuSelector: 'surveyPlanGridStateMenu',
            stateType: 'gji_survey_plan'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'personGridWindowAspect',
            gridSelector: 'surveyPlanGrid',
            editFormSelector: 'surveyplanaddwindow',
            storeName: 'SurveyPlan',
            modelName: 'SurveyPlan',
            editWindowView: 'surveyplan.AddWindow',
            editRecord: function (record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model = me.controller.getModel(me.modelName);

                if (id) {
                    me.controller.application.redirectTo(Ext.String.format('surveyplanedit/{0}', id));
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        }
    ],

    init: function() {
        this.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('surveyPlanGrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});