Ext.define('B4.view.taskcalendar.TaskWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.view.taskcalendar.DisposalGrid',
        'B4.view.taskcalendar.CourtPracticeGrid',
        'B4.view.taskcalendar.ProtocolGrid',
        'B4.view.taskcalendar.SMEVGrid',
        'B4.view.taskcalendar.AppealGrid',
        'B4.view.taskcalendar.SoprGrid',
        'B4.view.taskcalendar.AdmonitionGrid',
        'B4.view.taskcalendar.PrescriptionGrid'
        //'B4.view.version.ActualizeByFiltersDeleteGrid',
    ],
    alias: 'widget.taskcalendarEditWindow',
    title: 'Детализация по задачам',
    closeAction: 'hide',
    modal: false,
    trackResetOnLoad: true,
    maximizable: true,
    width: '80%',
    height: '60%',
    minHeight: '50%',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 5,
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    items: [
                        {
                            xtype: 'taskcalendarappealgrid',
                        },
                        {
                            xtype: 'taskcalendarsoprgrid',
                        },
                        {
                            xtype: 'taskcalendaradmongrid',
                        },
                        {
                            xtype: 'taskcalendardisposalgrid',
                        },
                        {
                            xtype: 'taskcalendarprescriptiongrid',
                        },
                        {
                            xtype: 'taskcalendarprotocolgrid'
                        },
                        {
                            xtype: 'taskcalendarcourtpracticeGrid',
                        },
                        {
                            xtype: 'taskcalendarsmevgrid',
                        }
                    ]
                },
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [

                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    //margin:'2 10 3 10',
                                    xtype: 'b4closebutton',
                                    text: 'Закрыть'
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