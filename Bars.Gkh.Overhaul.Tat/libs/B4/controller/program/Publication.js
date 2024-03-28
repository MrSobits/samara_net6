Ext.define('B4.controller.program.Publication', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.aspects.ButtonDataExport'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: [
        'program.PublicationGrid'
    ],

    refs: [
        {
            ref: 'mainPanel',
            selector: 'publicationproggrid'
        }
    ],
    
    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'buttonExportAspect',
            gridSelector: 'publicationproggrid',
            buttonSelector: 'publicationproggrid #btnExport',
            controllerName: 'PublishedProgramRecord',
            actionName: 'Export'
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'publicationproggrid b4updatebutton': { click: { fn: me.onUpdate, scope: me } },
            'publicationproggrid b4combobox': { change: { fn: me.onComboChange, scope: me } },
            'publicationproggrid combobox[name="Municipality"]': {
                select: { fn: me.onSelectMunicipality, scope: me },
                render: { fn: me.renderMuField, scope: me }
            }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainPanel() || Ext.widget('publicationproggrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().on('beforeload', me.onStoreBeforeLoad, me);
        view.getStore().load();
    },

    renderMuField: function (field) {
        var me = this,
            store = field.getStore();

        store.on('load', me.onLoadMunicipality, me, { single: true });
        store.load();
    },

    onLoadMunicipality: function (store, records) {
        var me = this,
            panel = me.getMainPanel(),
            cmb = panel.down('combobox[name="Municipality"]');

        if (records[0]) {
            cmb.setValue(records[0].data);
        }
    },

    onComboChange: function() {
        this.getMainPanel().getStore().load();
    },
    
    onStoreBeforeLoad: function (store, operation) {
        var moId = this.getMainPanel().down('b4combobox[name="Municipality"]').getValue();
        operation.params.mo_id = moId;
        operation.params.isPublPrograms = true;
        
        Ext.applyIf(store.getProxy().extraParams, {
            usedForCorrection: true
        });
    },

    onUpdate: function () {
        this.getMainPanel().getStore().load();
    }
});