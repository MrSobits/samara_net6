Ext.define('B4.view.actcheck.actioneditwindowbaseitem.RepresentInfoFieldSet', {
    extend: 'Ext.form.FieldSet',

    alias: 'widget.actcheckactionrepresentinfofieldset',

    title: 'Представитель',
    layout: 'hbox',
    items: [
        {
            xtype: 'container',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 0.75,
            defaults: {
                labelWidth: 110,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'ФИО',
                    name: 'RepresentFio',
                    maxLength: 255
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Место работы',
                    name: 'RepresentWorkPlace',
                    maxLength: 255
                }
            ]
        },
        {
            xtype: 'container',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'Должность',
                    name: 'RepresentPost',
                    maxLength: 255
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Доверенность номер',
                    name: 'RepresentProcurationNumber',
                    maxLength: 50
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата выдачи доверенности',
                            name: 'RepresentProcurationIssuedOn',
                            labelWidth: 170,
                            format: 'd.m.Y',
                            flex: 1
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Срок действия',
                            name: 'RepresentProcurationValidPeriod',
                            maxLength: 25,
                            flex: 0.75
                        }
                    ]
                }
            ]
        }
    ]
});