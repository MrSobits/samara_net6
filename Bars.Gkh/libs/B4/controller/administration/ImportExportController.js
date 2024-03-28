Ext.define('B4.controller.administration.ImportExportController', {    
    extend: 'B4.base.Controller',
    
    views: [
        'administration.ImportExportPanel',
        'administration.ImportExportLogGrid'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },
    
    stores: [
        'administration.ImportExportLogStore'
    ],

    refs: [
        { ref: 'mainPanel', selector: 'importexportpanel' }
    ],
    
    aspects:[
        {
            xtype: 'permissionaspect',
            applyBy: function(cmp, allowed) {
                if (!allowed) {
                    cmp.disable();
                } else {
                    cmp.enable();
                }
            },
            applyOn: { event: 'afterrender', selector: 'importexportpanel' },
            permissions: [
                {
                    name: 'Administration.ImportExport.Export',
                    applyTo: 'importexportpanel #fcEntity'
                },
                {
                    name: 'Administration.ImportExport.Import',
                    applyTo: 'importexportpanel #fcFile'
                }
            ]
        }  
    ],
    
    init: function() {
        var me = this;
       
        me.control({
            'importexportpanel [action="Export"]': { click: { fn: me.onExport, scope: me } },
            'importexportpanel [action="Import"]': { click: { fn: me.onImport, scope: me } },
            'importexportpanel b4filefield': {
                fileclear: { fn: me.onFileClear, scope: me },
                fileselected: { fn: me.onFileSelected, scope: me }
            },
            
            'importexportpanel b4selectfield': {
                change: { fn: me.onSelectFieldChange, scope: me }
            },
            'importexportloggrid b4updatebutton': {
                click: { fn: me.updateLogGrid, scope: me }
            }
        });

        me.callParent(arguments);
    },
    
    index: function() {
        var view = this.getMainPanel() || Ext.widget('importexportpanel', {closable: true});
        
        this.bindContext(view);
        this.application.deployView(view);
        
        this.disableImportButton(true);
        this.disableExportButton(true);
    },
    
    onExport: function () {
        var me = this,
            data = me.getMainPanel().down('b4selectfield').getValue();

        data = Ext.isArray(data) ? data : [data];

        var params = {};

        if (data.length > 0) {
            Ext.apply(params, { tableNames: data });
        }

        var urlParams = Ext.urlEncode(params);
        
        var newUrl = Ext.urlAppend('/importexport/GetEntities/?' + urlParams, '_dc=' + (new Date().getTime()));

        window.open(B4.Url.action(newUrl));
    },
    
    onImport: function() {
        var me = this,
            form = me.getMainPanel().getForm();

        form.submit({
            url: B4.Url.action('import', 'importexport'),
            success: function() {
                me.updateLogGrid();
            }
        });
    },
    
    onFileClear: function() {
        this.disableImportButton(true);
    },
    
    onFileSelected: function() {
        this.disableImportButton(false);
    },
    
    disableImportButton: function(disable) {
        this.getMainPanel().down('[action="Import"]').setDisabled(disable);
    },
    
    disableExportButton: function(disable) {
        this.getMainPanel().down('[action="Export"]').setDisabled(disable);
    },
    
    onSelectFieldChange: function(field, val) {
        this.disableExportButton(!val);
    },
    updateLogGrid: function () {
        var importExportLogGrid = this.getMainPanel().down('#importExportLogGrid');
        
        if (importExportLogGrid) {
            var store = importExportLogGrid.getStore();
            
            if (store && Ext.isFunction(store.reload)) {
                store.reload();
            }
        }
    }
});