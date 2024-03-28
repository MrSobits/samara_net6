Ext.define('B4.controller.program.SecondStage', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
   
    requires: [
        'B4.view.program.SecondStageGrid',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    
    refs: [
        { ref: 'mainPanel', selector: 'programsecondstagegrid' }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Ovrhl.Program2Stage.MakeStage3',
                    applyTo: 'b4addbutton[action=makethirdstage]',
                    selector: 'programsecondstagegrid'
                }
            ]
        }
    ],

    // Init
    init: function() {
        this.control({
            'programsecondstagegrid b4addbutton[action=makethirdstage]': { click: { fn: this.makeThirdStage, scope: this } },
            'programsecondstagegrid b4updatebutton' : { click: { fn: this.update, scope: this } }
        });

        this.callParent(arguments);
    },
    
    // Action Index - shows LongProgram grid
    index: function () {
        var view = this.getMainPanel() || Ext.widget('programsecondstagegrid');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    },
    
    update: function(btn) {
        btn.up('programsecondstagegrid').getStore().load();
    },
    
    makeThirdStage: function (btn) {
        var me = this,
            grid = btn.up('programsecondstagegrid');
        
        me.mask('Формирование 3 этапа программы...', grid);
        
        B4.Ajax.request({
            url: B4.Url.action('MakeStage3', 'RealityObjectStructuralElementInProgramm'),
            timeout: 9999999
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            me.unmask();
            if (response.success) {
                Ext.History.add('longprogram/');
            } else {
                Ext.Msg.alert('Ошибка!', response.message);
            }
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При формировании плана произошла ошибка!');
        });
    }
});