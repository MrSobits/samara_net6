Ext.define('B4.controller.program.LoadProgram', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.aspects.GkhButtonImportAspect',
        'B4.view.program.LoadProgramGrid',
        'B4.view.program.LoadProgramImportWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    
    stores:
    [
        'program.LoadProgram'
    ],
    
    views: [
        'program.LoadProgramGrid',
        'program.LoadProgramImportWindow'
    ],

    refs: [
        { ref: 'grid', selector: 'loadprogramgrid' }
    ],

    aspects: [
        {
            /*
            аспект для импорта
            */
            xtype: 'gkhbuttonimportaspect',
            name: 'dpkrLoadImportAspect',
            buttonSelector: 'loadprogramgrid #btnImport',
            codeImport: 'DpkrLoad',
            windowImportView: 'program.LoadProgramImportWindow',
            windowImportSelector: 'loadprogramimportwindow',
            maxFileSize: 10485760,
            refreshData: function (importId) {
                if (importId == 'DpkrLoad') {
                    this.controller.getGrid().getStore().load();
                }
            }
        }
    ],
    
    init: function () {
        this.control({
            'loadprogramimportwin b4closebutton': { click: { fn: this.closeImportWin, scope: this } }
        });

        this.callParent(arguments);
    },

    getMainView: function() {
        
    },
    
    closeImportWin: function(btn) {
        btn.up('loadprogramimportwin').close();    
    },
    
    index: function () {
        var view = this.getGrid() || Ext.widget('loadprogramgrid');

        this.bindContext(view);
        this.application.deployView(view);
        
        view.getStore().load();
        
        this.getAspect('dpkrLoadImportAspect').loadImportStore();
    }
});