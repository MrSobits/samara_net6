Ext.define('B4.view.actcheck.surveyaction.ActionEditWindow', {
    extend: 'B4.view.actcheck.BaseActionEditWindow',

    requires: [
        'B4.view.actcheck.actioneditwindowbaseitem.ControlledPersonInfoFieldSet',
        'B4.view.actcheck.actioneditwindowbaseitem.RepresentInfoFieldSet',
        'B4.view.actcheck.actioneditwindowbaseitem.RequisiteInfoFieldSet',
        'B4.view.actcheck.surveyaction.ResultInfoFieldSet'
    ],
    
    alias: 'widget.actchecksurveyactioneditwindow',
    title: 'Опрос',

    editFormItems: [
        {
            xtype: 'actcheckactionrequisiteinfofieldset',
            innerMessage: 'опроса',
            continueContainerHidden: false,
            dateAndPlaceContainerLayout: {
                type: 'vbox',
                align: 'stretch'
            }
        },
        {
            xtype: 'fieldset',
            title: 'Опрашиваемые лица',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            defaults: {
                margin: '0 10 10 10'
            },
            padding: 0,
            items: [
                {
                    xtype: 'actcheckactioncontrolledpersoninfofieldset'
                },
                {
                    xtype: 'actcheckactionrepresentinfofieldset'
                }
            ]
        },
        {
            xtype: 'surveyactionresultinfofieldset'
        }
    ]
});