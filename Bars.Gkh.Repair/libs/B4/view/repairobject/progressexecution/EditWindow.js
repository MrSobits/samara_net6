Ext.define('B4.view.repairobject.progressexecution.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.progressExecutionWorkEditWindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 700,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    itemId: 'progressExecutionWorkEditWindow',
    title: 'Ход выполнения работ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires:
    [
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField',

        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 150
            },
            items: [
            	{
            	    xtype: 'container',
            	    columnWidth: 0.5,
            	    layout: {
            	        type: 'form'
            	    },
            	    defaults: {
            	        labelWidth: 150,
            	        labelAlign: 'right'
            	    },
            	    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'VolumeOfCompletion',
                            fieldLabel: 'Объем выполнения',
                            itemId: 'dcfVolumeOfCompletion',
                            width: 300
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'CostSum',
                            itemId: 'dcfCostSum',
                            fieldLabel: 'Сумма расходов',
                            width: 300
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'PercentOfCompletion',
                            itemId: 'dcfPercentOfCompletion',
                            fieldLabel: 'Процент выполнения',
                            maxValue: 100,
                            width: 300
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
                                { xtype: 'b4savebutton' }
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
