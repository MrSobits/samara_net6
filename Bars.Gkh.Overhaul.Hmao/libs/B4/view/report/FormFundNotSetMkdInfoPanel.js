Ext.define('B4.view.report.FormFundNotSetMkdInfoPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'reportFormFundNotSetMkdInfoPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.form.SelectField'
        
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: { type: 'vbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 200,
                        width: 600,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'Municipalities',
                            itemId: 'tfMunicipality',
                            fieldLabel: 'Муниципальные образования',
                            emptyText: 'Все МО'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateTimeReport',
                            fieldLabel: 'Дата отчета'
                        },
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'HouseTypes',
                            itemId: 'tfHouseType',
                            fieldLabel: 'Тип дома',
                            emptyText: 'Все'
                        },
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'HouseTypes',
                            itemId: 'tfHouseCondition',
                            fieldLabel: 'Состояние дома',
                            emptyText: 'Все'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});