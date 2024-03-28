Ext.define('B4.controller.PoliticAuthority', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    controllers: ['B4.controller.politicauth.Navigation'],
    models: ['PoliticAuthority'],
    stores: ['PoliticAuthority'],
    views: [
        'politicauth.AddWindow',
        'politicauth.Grid'
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.PoliticAuth.Create', applyTo: 'b4addbutton', selector: 'politicAuthGrid' },
                { name: 'Gkh.Orgs.PoliticAuth.Edit', applyTo: 'b4savebutton', selector: '#politicAuthEditPanel' },
                { name: 'Gkh.Orgs.PoliticAuth.Edit', applyTo: 'b4savebutton', selector: '#politicAuthRecepJurGrid' },
                { name: 'Gkh.Orgs.PoliticAuth.Edit', applyTo: 'b4savebutton', selector: '#politicAuthRecepCitsGrid' },
                { name: 'Gkh.Orgs.PoliticAuth.Delete', applyTo: 'b4deletecolumn', selector: 'politicAuthGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'politicAuthGridWindowAspect',
            gridSelector: 'politicAuthGrid',
            editFormSelector: '#politicAuthAddWindow',
            storeName: 'PoliticAuthority',
            modelName: 'PoliticAuthority',
            editWindowView: 'politicauth.AddWindow',
            deleteWithRelatedEntities: true,
            controllerEditName: 'B4.controller.politicauth.Navigation'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'politicAuthorityButtonExportAspect',
            gridSelector: 'politicAuthGrid',
            buttonSelector: 'politicAuthGrid #btnExport',
            controllerName: 'PoliticAuthority',
            actionName: 'Export'
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    mainView: 'politicauth.Grid',
    mainViewSelector: 'politicAuthGrid',

    refs: [{
        ref: 'mainView',
        selector: 'politicAuthGrid'
    }],

    index: function () {
        var view = this.getMainView() || Ext.widget('politicAuthGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('PoliticAuthority').load();
    }
});