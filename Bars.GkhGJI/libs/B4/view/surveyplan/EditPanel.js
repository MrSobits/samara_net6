Ext.define('B4.view.surveyplan.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.surveyPlanEditPanel',
    closable: true,
    minWidth: 800,
    layout: {
        type: 'border'
    },
    title: 'План проверки',
    trackResetOnLoad: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.view.surveyplan.ContragentGrid',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.PlanJurPersonGji'
    ],

    initComponent: function() {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                falign: 'stretch',
                labelAlign: 'right'
            },
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
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-arrow-refresh',
                                    action: 'CreateCandidates',
                                    text: 'Рассчитать плановые даты проверок'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    action: 'ChangeState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'container',
                    region: 'north',
                    padding: 5,
                    layout: 'vbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 500,
                            width: 700
                        },
                        {
                            xtype: 'b4selectfield',
                            width: 700,
                            name: 'PlanJurPerson',
                            fieldLabel: 'План',
                            store: 'B4.store.dict.PlanJurPersonGji',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'surveyPlanContragentGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});