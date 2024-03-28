Ext.define('B4.aspects.permission.preventiveaction.Task', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.preventiveactiontaskpermissions',

    requires: [
        'B4.enums.PreventiveActionType'
    ],
    
    applyOn: {
        event: 'aftersetpaneldata',
        selector: '#preventiveActionTaskEditPanel'
    },

    permissions: [
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.ConsultingQuestion.View',
            applyTo: '#consultingQuestionGrid',
            selector: '#preventiveActionTaskEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    var panel = component.up('#taskTabPanel'),
                        actionTypeEnumCombo = panel.down('b4enumcombo[name=ActionType]');
                    
                    if (allowed && actionTypeEnumCombo.getValue() === B4.enums.PreventiveActionType.Consultation) component.tab.show();
                    else component.tab.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.ConsultingQuestion.Create',
            applyTo: 'b4addbutton',
            selector: '#consultingQuestionGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.ConsultingQuestion.Edit',
            applyTo: '#consultingQuestionSaveButton',
            selector: '#consultingQuestionGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.ConsultingQuestion.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#consultingQuestionGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.Tasks.Create',
            applyTo: 'b4addbutton',
            selector: '#taskOfPreventiveActionTaskGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.Tasks.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#taskOfPreventiveActionTaskGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.Regulations.Create',
            applyTo: '#normativeDocAddButton',
            selector: '#preventiveActionTaskRegulationsTabPanel'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.Regulations.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#preventiveActionTaskRegulationsTabPanel'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.Objectives.Create',
            applyTo: 'b4addbutton',
            selector: 'objectivetabpanel'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.Objectives.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'objectivetabpanel'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.Item.Create',
            applyTo: 'b4addbutton',
            selector: '#preventiveActionTaskItemGrid'
        },
        {
            name: 'GkhGji.DocumentsGji.PreventiveActionTask.Registry.Item.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#preventiveActionTaskItemGrid'
        }
    ]
});