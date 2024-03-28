Ext.define('B4.view.efficiencyrating.Panel', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.ux.button.Add',
        'B4.ux.button.Save',

        'B4.view.efficiencyrating.FactorTreePanel',
        'B4.view.efficiencyrating.FactorPanel',
        'B4.view.efficiencyrating.AttributeForm',

        'B4.form.SelectField',
        'B4.store.dict.EfficiencyRatingPeriod',
        'B4.model.dict.EfficiencyRatingPeriod'
    ],

    title: 'Редактор рейтинга эффективности УО',
    alias: 'widget.efficiencyRatingPanel',

    layout: { type: 'hbox', align: 'stretch' },
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            defaults: {
                margin: '0 2'
            },
            items: [
                {
                    xtype: 'efFactorTreePanel',
                    width: 300
                },
                {
                    xtype: 'container',
                    name: 'efMain',
                    border: null,
                    flex: 1,
                    layout: { type: 'hbox', align: 'stretch' },
                    items:[
                        {
                            xtype: 'efFactorPanel',
                            flex: 1,
                            margin: '0 4 0 0'
                        },
                        {
                            xtype: 'efAttributeFormPanel',
                            width: 400
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
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '10px, 0',
                            items: [
                                {
                                    width: 400,
                                    labelWidth: 70,
                                    labelAlign: 'right',
                                    xtype: 'b4selectfield',
                                    name: 'EfficiencyRatingPeriod',
                                    fieldLabel: 'Период',
                                    editable: false,
                                    idProperty: 'Id',
                                    textProperty: 'Name',
                                    allowBlank: false,
                                    store: 'B4.store.dict.EfficiencyRatingPeriod',
                                    columns: [
                                        {
                                            dataIndex: 'Name',
                                            flex: 1,
                                            text: 'Период',
                                            filter: {
                                                xtype: 'textfield',
                                                maxLength: 255
                                            }
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'DateStart',
                                            width: 100,
                                            text: 'Дата начала',
                                            filter: {
                                                xtype: 'datefield',
                                                operand: CondExpr.operands.eq
                                            }
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'DateEnd',
                                            width: 100,
                                            text: 'Дата окончания',
                                            filter: {
                                                xtype: 'datefield',
                                                operand: CondExpr.operands.eq
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'button',
                                    margin: '0 10px',
                                    text: 'Скопировать редактор',
                                    actionName: 'copyconstructor'
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
