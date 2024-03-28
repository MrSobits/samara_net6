Ext.define('B4.view.actcheck.explanationaction.ActionEditWindow', {
    extend: 'B4.view.actcheck.BaseActionEditWindow',

    requires: [
        'B4.form.FileField',
        'B4.view.actcheck.actioneditwindowbaseitem.RequisiteInfoFieldSet',
        'B4.view.actcheck.explanationaction.ControlledPersonInfoFieldSet'
    ],
    
    alias: 'widget.actcheckexplanationactioneditwindow',
    title: 'Объяснение',

    editFormItems: [
        {
            xtype: 'actcheckactionrequisiteinfofieldset'
        },
        {
            xtype: 'explanationactioncontrolledpersoninfofieldset'
        },
        {
            xtype: 'fieldset',
            title: 'Объяснение',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'textarea',
                    fieldLabel: 'Объяснение',
                    name: 'Explanation',
                    allowBlank: false,
                    labelWidth: 110,
                    labelAlign: 'right',
                    maxLength: 1000
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 110,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'AttachmentName',
                            fieldLabel: 'Наименование приложения',
                            maxLength: 255
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'AttachmentFile',
                            fieldLabel: 'Файл приложения',
                            padding: '5 0 5 5',
                            editable: false
                        }
                    ]
                }
            ]
        }
    ]
});