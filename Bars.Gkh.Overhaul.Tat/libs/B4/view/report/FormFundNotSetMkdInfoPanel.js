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
        'B4.enums.TypeCollectRealObj',
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
                            xtype: 'b4combobox',
                            name: 'TypeCollectRealObj',
                            fieldLabel: 'Cобирать по домам',
                            editable: false,
                            store: B4.enums.TypeCollectRealObj.getStore(),
                            valueField: 'Value',
                            displayField: 'Display'
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
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'ProgramCr',
                            itemId: 'sfProgramCr',
                            fieldLabel: 'Программа капремонта',
                            store: 'B4.store.dict.ProgramCrAon',
                            editable: false,
                            emptyText: 'Все программы',
                            columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }]
                        }
                    ]
                }  
            ]
        });

        me.callParent(arguments);
    }
});