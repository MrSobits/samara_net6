Ext.define('B4.view.efficiencyrating.analitics.GraphWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.analiticsgraphwindow',
    mixins: ['B4.mixins.window.ModalMask'],

    layout: { type: 'vbox', align: 'stretch' },
    maximized: true,
    closable: true,
    closeAction: 'destroy',
    title: '',
    requires: [
        'B4.ux.button.Close',

        'B4.ux.Highcharts',
        'B4.ux.Highcharts.SplineSerie',
        'B4.ux.Highcharts.AreaSplineSerie',
        'B4.ux.Highcharts.PieSerie',
        'B4.ux.Highcharts.ColumnSerie'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me,
        {
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    name: 'Highcharts',
                    flex: 1,
                    layout: { type: 'vbox', align: 'stretch' },
                    items: []
                }
            ]
        });

        me.callParent(arguments);
    }

});