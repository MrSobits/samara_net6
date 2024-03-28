Ext.define('B4.view.actcheck.surveyaction.ResultInfoFieldSet', {
    extend: 'Ext.form.FieldSet',

    alias: 'widget.surveyactionresultinfofieldset',

    requires: [
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.enums.YesNoNotSet',
        'B4.enums.HasValuesNotSet',
        'B4.view.actcheck.actioneditwindowbaseitem.RemarkGrid',
        'B4.view.actcheck.surveyaction.QuestionGrid'
    ],

    title: 'Результаты опроса',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    padding: '0 10 10 10',
    items: [
        {
            xtype: 'container',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    name: 'ProtocolReaded',
                    fieldLabel: 'Протокол прочитан',
                    enumName: 'B4.enums.YesNoNotSet',
                    labelWidth: 200,
                    labelAlign: 'right',
                    margin: '10 0 10 0'
                },
                {
                    xtype: 'surveyactionquestiongrid',
                    height: 200
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'HasRemark',
                    fieldLabel: 'Замечания',
                    enumName: 'B4.enums.HasValuesNotSet',
                    margin: '10 0 10 0',
                    labelWidth: 200,
                    labelAlign: 'right',
                    listeners: {
                        change: function (field, newValue) {
                            var fieldSet = field.up('surveyactionresultinfofieldset'),
                                actCheckActionRemarkGrid = fieldSet.down('#actCheckActionRemarkGrid');

                            if (newValue === B4.enums.HasValuesNotSet.Yes) {
                                actCheckActionRemarkGrid.show();
                            }
                            else {
                                actCheckActionRemarkGrid.hide();
                            }
                        }
                    }
                },
                {
                    xtype: 'actcheckactionremarkgrid',
                    height: 200
                }
            ]
        }
    ]
});