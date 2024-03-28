Ext.define('B4.view.dict.typesurveygji.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'border',
    width: 1000,
    height: 500,
    minHeight: 400,
    maxHeight: 600,
    bodyPadding: 5,
    itemId: 'typeSurveyGjiEditWindow',
    title: 'Тип обследования',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.typesurveygji.KindInspGjiGrid',
        'B4.view.dict.typesurveygji.GoalInspGjiGrid',
        'B4.view.dict.typesurveygji.TaskInspGjiGrid',
        'B4.view.dict.typesurveygji.InspFoundationGjiGrid',
        'B4.view.dict.typesurveygji.InspFoundationCheckGjiGrid',
        'B4.view.dict.typesurveygji.TypeSurveyProvidedDocGjiGrid',
        'B4.view.dict.typesurveygji.AdminRegulationGjiGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    border: false,
                    region: 'north',
                    layout: {
                        type: 'anchor'
                    },
                    items:[
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            anchor: '100%',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            anchor: '100%',
                            maxLength: 300
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    border: false,
                    items: [
                        {
                            xtype: 'kindInspGjiGrid'
                        },
                        {
                            xtype: 'goalInspGjiGrid'
                        },
                        {
                            xtype: 'taskInspGjiGrid'
                        },
                        {
                            xtype: 'inspFoundationCheckGjiGrid'
                        },
                        {
                            xtype: 'inspFoundationGjiGrid'
                        },
                        {
                            xtype: 'adminRegulationGjiGrid'
                        },
                        {
                            xtype: 'typesurveyprovideddocgjigrid'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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