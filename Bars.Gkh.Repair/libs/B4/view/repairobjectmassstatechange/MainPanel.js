Ext.define('B4.view.repairobjectmassstatechange.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Массовая смена статусов объектов ТР',
    alias: 'widget.massRepairChangeStateMainPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    requires: [
        'B4.view.repairobjectmassstatechange.Grid',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.RepairProgram'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    name: 'massRepairChangeStatePanel',
                    closable: false,
                    layout: 'vbox',
                    frame: true,
                    border: false,
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'sfProgram',
                            fieldLabel: 'Программа тек.ремонта',
                            store: 'B4.store.dict.RepairProgram',
                            width: 400,
                            editable: false
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'cbCurrentState',
                            url: '/State/GetListByType',
                            fields: ['Id', 'Name'],
                            fieldLabel: 'Текущий статус',
                            width: 400,
                            disabled: true,
                            editable: false
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'cbNextState',
                                    url: '/StateTransfer/GetStates',
                                    fields: ['Id', 'Name'],
                                    fieldLabel: 'Новый статус',
                                    padding: '0 5 0 0',
                                    width: 400,
                                    labelWidth: 150,
                                    labelAlign: 'right',
                                    disabled: true,
                                    editable: false
                                },
                                {
                                    xtype: 'button',
                                    name: 'btnChangeState',
                                    text: 'Выполнить',
                                    disabled: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'repairObjectMassChangeStateGrid',
                    border: false,
                    flex: 1
                }
            ]
        }, me);
        
        me.callParent(arguments);
    }
});