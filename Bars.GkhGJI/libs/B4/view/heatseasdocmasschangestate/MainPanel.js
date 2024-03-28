Ext.define('B4.view.heatseasdocmasschangestate.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Массовая смена статусов документов отопительного сезона',
    alias: 'widget.heatSeasDocMassChangeStateMainPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.view.heatseasdocmasschangestate.Grid',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.HeatSeasonPeriodGji'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    itemId: 'changeStatePanel',
                    closable: false,
                    layout: 'vbox',
                    frame: true,
                    border: false,
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            //name: 'ProgramCr',
                            itemId: 'sfHeatSeasonPeriod',
                            fieldLabel: 'Период отопительного сезона',
                           

                            store: 'B4.store.dict.HeatSeasonPeriodGji',
                            width: 500,
                            editable: false
                        },
                        {
                            xtype: 'b4combobox',
                            itemId: 'cbCurrentState',
                            url: '/State/GetListByType',
                            fields: ['Id', 'Name'],
                            fieldLabel: 'Текущий статус',
                            width: 500,
                            disabled: true,
                            editable: false
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    itemId: 'cbNextState',
                                    url: '/StateTransfer/GetStates',
                                    fields: ['Id', 'Name'],
                                    fieldLabel: 'Новый статус',
                                    padding: '0 5 0 0',
                                    width: 500,
                                    labelWidth: 200,
                                    labelAlign: 'right',
                                    disabled: true,
                                    editable: false
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'btnChangeState',
                                    text: 'Выполнить',
                                    disabled: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'heatSeasDocMassChangeStateGrid',
                    border: false,
                    flex: 1
                }
            ]
        }, me);

        me.callParent(arguments);
    }
});