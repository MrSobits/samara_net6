Ext.define('B4.controller.dict.MultipurposeGlossary', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.mixins.LayoutControllerLoader',
        'B4.mixins.MaskBody',
        'B4.aspects.permission.dict.MultipurposeGlossary'
    ],

    glossaryId: null,
    
    views: [
        'dict.multipurpose.GlossaryNavigation',
        'dict.multipurpose.GlossaryEditWindow',
        'dict.multipurpose.ItemsGrid'
    ],
    
    mainView: 'dict.multipurpose.GlossaryNavigation',
    mainViewSelector: 'multipurposeGlossaryNavigation',
    
    stores: [
        'dict.multipurpose.MultipurposeGlossary',
        'dict.multipurpose.MultipurposeGlossaryItem'
    ],
    models: [
        'dict.multipurpose.MultipurposeGlossary',
        'dict.multipurpose.MultipurposeGlossaryItem'
    ],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    refs: [
        {
            ref: 'mainView',
            selector: 'multipurposeGlossaryNavigation'
        },
        {
            ref: 'menuGrid',
            selector: '#multiGlossaryMenuGrid'
        },
        {
            ref: 'itemsGrid',
            selector: '#multipurposeItemsGrid'
        },
        {
            ref: 'editWindow',
            selector: '#multipurposeGlossaryEdit'
        }
    ],
    
    aspects: [
        {
            xtype: 'multipurposedictperm'
        },
        {
            xtype: 'grideditwindowaspect',
            gridSelector: '#multiGlossaryMenuGrid',
            editFormSelector: '#multipurposeGlossaryEdit',
            editWindowView: 'dict.multipurpose.GlossaryEditWindow',
            modelName: 'dict.multipurpose.MultipurposeGlossary'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: '#multipurposeItemsGrid',
            storeName: 'dict.multipurpose.MultipurposeGlossaryItem',
            modelName: 'dict.multipurpose.MultipurposeGlossaryItem',
            listeners: {
                beforeaddrecord: function(asp, rec) {
                    rec.set('Glossary', { Id: asp.controller.glossaryId });
                }
            }
        }
    ],
    
    init: function () {
        this.control({
            '#multiGlossaryMenuGrid': {
                selectionchange: { fn: this.gridSelectionChange, scope: this }
            }
        });

        this.callParent(arguments);
    },
    
    index: function () {
        var view = this.getMainView() || Ext.widget('multipurposeGlossaryNavigation');
        this.bindContext(view);
        this.application.deployView(view);
    },

    gridSelectionChange: function (selModel, selected, eOpts) {
        if (selected.length == 0) {
            return;
        }

        this.getItemsGrid().down('b4addbutton').enable();

        var glossary = selected[0];
        this.glossaryId = glossary.get('Id');

        var store = this.getItemsGrid().getStore();
        Ext.apply(store.getProxy().extraParams, {
            glossaryId: glossary.get('Id')
        });
        
        store.load();
    }
});