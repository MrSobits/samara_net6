Ext.define('B4.view.actcheck.inspectionaction.ActionEditWindow', {
    extend: 'B4.view.actcheck.BaseActionEditWindow',

    requires: [
        'B4.view.actcheck.actioneditwindowbaseitem.CarriedOutEventSelectField',
        'B4.view.actcheck.actioneditwindowbaseitem.ControlledPersonInfoFieldSet',
        'B4.view.actcheck.actioneditwindowbaseitem.RepresentInfoFieldSet',
        'B4.view.actcheck.actioneditwindowbaseitem.RequisiteInfoFieldSet',
        'B4.view.actcheck.actioneditwindowbaseitem.ResultInfoFieldSet'
    ],

    alias: 'widget.actcheckinspectionactioneditwindow',
    title: 'Осмотр',

    // Видимость блока
    // "Лица, присутствующие при осмотре"
    personInfoFieldSetHidden: false,

    // Приписка к itemId
    itemIdInnerMessage: '',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            editFormItems: [
                {
                    xtype: 'actcheckactionrequisiteinfofieldset',
                    innerMessage: 'осмотра',
                    continueContainerHidden: false,
                    dateAndPlaceContainerLayout: {
                        type: 'vbox',
                        align: 'stretch'
                    }
                },
                {
                    xtype: 'fieldset',
                    title: 'Лица, присутствующие при осмотре',
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
                    xtype: 'actcheckactionresultinfofieldset',
                    title: 'Результаты осмотра',
                    itemIdInnerMessage: me.itemIdInnerMessage,
                    afterViolationItems: [
                        {
                            xtype: 'actcheckactioncarriedouteventselectfield',
                            fieldLabel: 'Действия, которые проводились в процессе осмотра',
                            margin: '5 0 5 0',
                            labelWidth: 200,
                            labelAlign: 'right',
                            hidden: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});