Ext.define('B4.view.diagnostic.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    height: 450,
    bodyPadding: 5,
    title: 'Диагностика',
    closeAction: 'hide',
    trackResetOnLoad: true,
    alias: 'widget.diagnosticeditwindow',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.store.diagnostic.CollectedDiagnosticResult',
        'B4.store.diagnostic.DiagnosticResult',
        'B4.view.diagnostic.Grid',
        'B4.view.Control.GkhTriggerField',
        'B4.TextValuesOverride',
        'B4.store.diagnostic.DiagnosticResult',
        'B4.store.diagnostic.CollectedDiagnosticResult'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                flex: 1
            },
            items: [
                {
                    xtype: 'diagnosticresultgrid',
                    flex: 1
                }

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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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