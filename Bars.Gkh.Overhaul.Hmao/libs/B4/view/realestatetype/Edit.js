Ext.define('B4.view.realestatetype.Edit', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.realestatetypeedit',

    requires: [
        'Ext.tab.Panel',
        'B4.ux.button.Save',
        'Ext.form.field.Text',
        'Ext.form.field.Hidden',
        'Ext.form.Panel',

        'B4.view.realestatetype.CommonParamGrid',
        'B4.view.realestatetype.StructElemGrid'
    ],
    // Id редактируемого RealEstateType
    objId: 0,

    title: 'Редактирование типа дома',
    closable: true,


    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'start'
            },
            items: [
                {
                    xtype: 'form',
                    border: 0,
                    defaults: {
                        width: 600,
                        labelWidth: 200,
                        labelAlign: 'right',
                        margin: '5 0 5 0'
                    },
                    items: [
                        {
                            xtype: 'hiddenfield',
                            name: 'Id'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            maxLength: 100
                        },
                        {
                            xtype: 'numberfield',
                            name: 'MarginalRepairCost',
                            fieldLabel: 'Предельная стоимость ремонта на кв.м общей площади, руб.',
                            minValue: 0,
                            hideTrigger: true,
                            allowDecimals: true,
                            decimalSeparator: ',',
                            negativeText: 'Значение не может быть отрицательным'
                        }
                    ],
                    tbar: {
                        items: [
                            {
                                xtype: 'b4savebutton'
                            }
                        ]
                    }
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    items: [
                        {
                            title: 'Общие характеристики',
                            xtype: 'commonparameditgrid'
                        },
                        {
                            title: 'Конструктивные элементы',
                            xtype: 'structelemeditgrid'
                        },
                        {
                            xtype: 'retmunicipalitygrid',
                            title: 'Муниципальные образования'
                        },
                        {
                            title: 'Параметры очередности',
                            xtype: 'realestatetypepriorityparamgrid',
                            hidden: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});