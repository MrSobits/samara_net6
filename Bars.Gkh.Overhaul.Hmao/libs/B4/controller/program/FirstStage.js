Ext.define('B4.controller.program.FirstStage', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
   
    requires: [
        'B4.view.program.FirstStageGrid',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    
    refs: [
        { ref: 'mainPanel', selector: 'programfirststagegrid' }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Ovrhl.Program1Stage.MakeStage1',
                    applyTo: 'b4addbutton[action=makefirststage]',
                    selector: 'programfirststagegrid'
                },
                {
                    name: 'Ovrhl.Program1Stage.MakeStage2',
                    applyTo: 'b4addbutton[action=makesecondstage]',
                    selector: 'programfirststagegrid'
                }
            ]
        }
    ],

    // Init
    init: function() {
        this.control({
            'programfirststagegrid b4addbutton[action=makefirststage]': { click: { fn: this.makeFirstStage, scope: this } },
            'programfirststagegrid b4addbutton[action=makesecondstage]': { click: { fn: this.makeSecondStage, scope: this } },
            'programfirststagegrid b4updatebutton': { click: { fn: this.update, scope: this } }
        });

        this.callParent(arguments);
    },
    
    // Action Index - shows LongProgram grid
    index: function () {
        var view = this.getMainPanel() || Ext.widget('programfirststagegrid');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    },
    
    update: function(btn) {
        btn.up('programfirststagegrid').getStore().load();
    },
    
    makeFirstStage: function (btn) {
        var me = this,
            grid = btn.up('programfirststagegrid');
        
        me.mask('Формирование 1 этапа программы...', grid);
        
        B4.Ajax.request({
            url: B4.Url.action('MakeStage1', 'RealityObjectStructuralElementInProgramm'),
            timeout: 9999999
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            me.unmask();
            if (response.success) {
                grid.getStore().load();
            } else {
                Ext.Msg.alert('Ошибка!', response.message);
            }
        }).error(function (resp) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', resp.message ? resp.message : 'При формировании плана произошла ошибка!');
        });
    },
    
    makeSecondStage: function (btn) {
        var me = this,
            grid = btn.up('programfirststagegrid');

        me.mask('Формирование программы...', grid);

        B4.Ajax.request({
            url: B4.Url.action('MakeStage3', 'RealityObjectStructuralElementInProgramm'),
            timeout: 9999999
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            me.unmask();
            if (response.success) {
                Ext.History.add('dpkr');
            } else {
                Ext.Msg.alert('Ошибка!', response.message);
            }
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При формировании плана произошла ошибка!');
        });
    }
});