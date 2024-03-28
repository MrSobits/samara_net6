Ext.define('B4.view.actcheck.instrexamaction.ActionEditWindow', {
    extend: 'B4.view.actcheck.BaseActionEditWindow',

    requires: [
        'B4.view.actcheck.actioneditwindowbaseitem.CarriedOutEventSelectField',
        'B4.view.actcheck.actioneditwindowbaseitem.ControlledPersonInfoFieldSet',
        'B4.view.actcheck.actioneditwindowbaseitem.RepresentInfoFieldSet',
        'B4.view.actcheck.actioneditwindowbaseitem.ResultInfoFieldSet',
        'B4.view.actcheck.instrexamaction.NormativeDocGrid',
        'B4.view.actcheck.instrexamaction.RequisiteInfoFieldSet'
    ],

    alias: 'widget.actcheckinstrexamactioneditwindow',
    title: 'Инструментальное обследование',

    // Видимость блока
    // "Лица, присутствующие при обследовании"
    personInfoFieldSetHidden: false,

    // Приписка к itemId
    itemIdInnerMessage: '',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            editFormItems: [
                {
                    xtype: 'instrexamactionrequisiteinfofieldset'
                },
                {
                    xtype: 'fieldset',
                    title: 'Лица, присутствующие при обследовании',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        margin: '0 10 10 10'
                    },
                    hidden: me.personInfoFieldSetHidden,
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
                    xtype: 'instrexamactionnormativedocgrid',
                    itemIdInnerMessage: me.itemIdInnerMessage,
                    height: 200
                },
                {
                    xtype: 'actcheckactionresultinfofieldset',
                    title: 'Результаты обследования',
                    itemIdInnerMessage: me.itemIdInnerMessage,
                    beforeViolationItems: [
                        {
                            xtype: 'checkbox',
                            name: 'TerritoryAccessDenied',
                            boxLabel: 'Отказано в доступе на территорию',
                            fieldStyle: 'vertical-align: middle;',
                            margin: '0 0 0 205',
                            labelAlign: 'right'
                        }
                    ],
                    afterViolationItems: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 200,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'actcheckactioncarriedouteventselectfield',
                                    fieldLabel: 'Действия, которые проводились в результате обследования',
                                    margin: '5 0 5 0',
                                    hidden: true
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Используемое оборудование',
                                    name: 'UsingEquipment',
                                    margin: '10 0 10 0',
                                    maxLength: 500
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});