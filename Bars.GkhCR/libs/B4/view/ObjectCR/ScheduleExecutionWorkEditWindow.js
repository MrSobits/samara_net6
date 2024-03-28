Ext.define('B4.view.objectcr.ScheduleExecutionWorkEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 400,
    maxWidth: 700,
    minWidth: 520,
    minHeight: 310,
    height: 500,
    bodyPadding: 5,
    title: 'График выполнения работ',
    closeAction: 'destroy',

    alias: 'widget.scheduleExecutionWorkEditWindow',

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.store.dict.Work',
        'B4.ux.button.Close',
        'B4.view.objectcr.ScheduleExecutionWorkStageGrid',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [            
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Work',
                    itemId: 'sflWork',
                    fieldLabel: 'Вид работы',                  
                    store: 'B4.store.dict.Work',
                    allowBlank: false,
                    disabled: true
                },  
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateStartWork',
                            fieldLabel: 'Начало работ',
                            format: 'd.m.Y',
                            itemId: 'dfDateStartWork'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEndWork',
                            fieldLabel: 'Окончание работ',
                            format: 'd.m.Y',
                            itemId: 'dfDateEndWork'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    itemId: 'taDescription',
                    maxLength: 2000
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        { xtype: 'scheduleexecutionworkstagegrid' }
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
                            columns: 1,
                            items: [
                                { xtype: 'b4savebutton' },
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});