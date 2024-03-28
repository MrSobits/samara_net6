Ext.define('B4.controller.program.Dpkr', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },
    
    views: ['program.DpkrGroupedYearGrid'],
    
    refs:[
        {ref: 'grid', selector: 'dpkrpublish'}
    ],
    
    init: function() {
        this.control({
            'dpkrpublish b4updatebutton': { click: { fn: this.update, scope: this } }
        });

        this.callParent(arguments);
    },
    
    index: function() {
        var view = this.getGrid() || Ext.widget('dpkrpublish');

        this.bindContext(view);
        this.application.deployView(view);
    },
    
    update: function() {
        this.getGrid().getStore().reload();
    }
});