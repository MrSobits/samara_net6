Ext.define('B4.view.informationoncontracts.GridPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.infcontractgridpanel',
    closable: false,
    itemId: 'informationOnContractsGridPanel',
    layout: 'border',
    
    requires: [
        'B4.view.informationoncontracts.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                { 
                    xtype: 'container',
                    region: 'north',
                    padding: 2,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Сведения о заключении товариществом/кооперативом договоров об оказании услуг по содержанию и (или) выполнению работ по ремонту общего имущества в МКД и предоставлению коммунальных услуг, а также иных договоров, обеспечивающих содержание и ремонт общего имущества в МКД и предоставление коммунальных услуг </span>'
                },
                {
                    xtype: 'infcontractgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
