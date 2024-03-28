Ext.define('B4.view.fundsinfo.GridPanel', {
    extend: 'Ext.panel.Panel',
    closable: false,
    itemId: 'fundsInfoGridPanel',
    layout: 'border',
    
    requires: [
        'B4.view.fundsinfo.Grid'
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
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Сведения об образовании резервного фонда товарищества или кооператива, иных специальных фондов товарищества или кооператива, в том числе фондов на проведение текущего и капитального ремонта общего имущества в МКД</span>'
                },
                Ext.create('B4.view.fundsinfo.Grid', { region: 'center' })
            ]
        });

        me.callParent(arguments);
    }
});
