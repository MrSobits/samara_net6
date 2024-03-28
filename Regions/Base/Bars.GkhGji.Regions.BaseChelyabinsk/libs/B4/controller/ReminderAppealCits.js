Ext.define('B4.controller.ReminderAppealCits', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.view.reminder.AppealCitsGrid',
        'B4.aspects.StateContextMenu',
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    stores: ['reminder.AppealCitsReminder'],

    views: ['reminder.AppealCitsGrid'],
    
    refs: [
        { ref: 'MainGrid', selector: 'reminderAppealCitsGrid' }
    ],
    
    init: function () {
        var actions = {};
        
        actions['reminderAppealCitsGrid'] = { 'rowaction': { fn: this.rowAction, scope: this } };

        this.control(actions);

        this.callParent(arguments);
    },
    
    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'appealCitsStateTransferAspect',
            gridSelector: '#reminderAppealCitsGrid',
            stateType: 'gji_appcits_executant',
            menuSelector: 'reminderAppealCitsGridStateMenu',
            entityIdProperty: 'AppealCitsExecutant'
        },
    ],


    // метод для Панели инспектора - роуты
    index: function () {
        var grid = this.getMainGrid();

        if (!grid) {
            grid = Ext.widget('reminderAppealCitsGrid');
            
            this.bindContext(grid);
            this.application.deployView(grid);
            
        }
        
        grid.getStore().load();
    },
    
    rowAction: function(grid, action, record) {
        var me = this,
            portal = me.getController('PortalController'),
            controllerEditName = 'B4.controller.AppealCits',
            params = {};

        params.appealId = record.get('AppealCits');
        portal.loadController(controllerEditName, params);
    }

});