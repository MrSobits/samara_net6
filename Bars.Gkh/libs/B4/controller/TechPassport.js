Ext.define('B4.controller.TechPassport', {
    extend: 'B4.controller.MenuItemController',
    
    params: {},
    editPanelSelector: '#mainformpanel',

    requires: [
        'B4.Url',
        'B4.Ajax',
        'B4.aspects.GkhPassport',
        'B4.ux.button.Save',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
    ],

    refs: [
        {
            ref: 'menu',
            selector: 'realityobjNavigationPanel menutreepanel'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [{
        xtype: 'gkhpassportaspect',
        name: 'techPassportWindowAspect'
    }, {
        xtype: 'gkhstatepermissionaspect',
        name: 'tpperm',
        permissions: [{
            name: 'Gkh.RealityObject.Register.TechPassport.Edit',
            applyTo: 'b4savebutton',
            selector: 'panel[type=tpform]'
        }]
    }],

    index: function (id, sectionId) {
        var me = this, asp, rec,
            view,
            sectionNumber = sectionId.match(/(\d+)/g).join('.') + '. ';
            
        me.params.realityObjectId = id;
        me.params.sectionId = sectionId;
                
        view = me.getMainComponent();
        
        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.setContextValue(view, 'sectionId', sectionId);
        me.application.deployView(view, 'reality_object_info');

        asp = me.getAspect('techPassportWindowAspect');
        me.mask('Загрузка', me.getMainComponent());

        B4.Ajax.request(B4.Url.action('GetForm', 'TechPassport', {
                sectionId: sectionId,
                realityObjectId: id
            }))
            .next(function(response) {
                me.unmask();
                var data = Ext.decode(response.responseText),
                    title = sectionNumber + data.form.Components[0].Title;

                if (Ext.isEmpty(view.title)) {
                    view.setTitle(title);
                }

                Ext.Array.each(data.editors, function(editor) {
                    if (editor.Store) {
                        var store = Ext.StoreMgr.lookup(editor.Store);
                        if (store == null) {
                            me.getStore(editor.Store);
                        }
                    }
                }, me);

                asp.createMetastruct(data);

                return true;
            }, me)
            .error(function() {
                me.unmask();
            }, me);

        me.getAspect('tpperm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    getMainComponent: function () {
        var me = this,
            sel = me.editPanelSelector + me.params.sectionId;
        var editPanel = me.getCmpInContext(sel);

        if (!editPanel) {
            editPanel = me.createPanel();
        }

        return editPanel;
    },

    createPanel: function () {
        var me = this,
            rec = me.getMenu().getSelectionModel().getSelection()[0],
            title = '';
        
        me.mainPanelSelector = 'mainformpanel' + me.params.sectionId;

        if (rec) {
            var options = rec.get('options') || {};
            title = options.title || title;
        }

        return Ext.create('Ext.panel.Panel', {
            itemId: me.mainPanelSelector,
            title: title,
            closable: true,
            autoScroll: true,
            layout: 'vbox',
            type: 'tpform',
            renderTo: Ext.getBody(),
            items: [{}],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });
    }
});