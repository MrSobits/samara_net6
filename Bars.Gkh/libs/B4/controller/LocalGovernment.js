Ext.define('B4.controller.LocalGovernment', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    controllers: ['B4.controller.localgov.Navigation'],
    models: ['LocalGovernment'],
    stores: ['LocalGovernment'],
    views: [
        'localgov.AddWindow',
        'localgov.Grid'
    ],

    aspects: [
        {
            xtype: 'gkhgrideditformaspect',
            name: 'localGovGridWindowAspect',
            gridSelector: 'localGovGrid',
            editFormSelector: '#localGovAddWindow',
            storeName: 'LocalGovernment',
            modelName: 'LocalGovernment',
            editWindowView: 'localgov.AddWindow',
            deleteWithRelatedEntities: true,
            controllerEditName: 'B4.controller.localgov.Navigation'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'localGovernmentButtonExportAspect',
            gridSelector: 'localGovGrid',
            buttonSelector: 'localGovGrid #btnExport',
            controllerName: 'LocalGovernment',
            actionName: 'Export'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.LocalGov.Create', applyTo: 'b4addbutton', selector: 'localGovGrid' },
                { name: 'Gkh.Orgs.LocalGov.Edit', applyTo: 'b4savebutton', selector: '#localGovEditPanel' },
                { name: 'Gkh.Orgs.LocalGov.Edit', applyTo: 'b4savebutton', selector: '#receptionJurGrid' },
                { name: 'Gkh.Orgs.LocalGov.Edit', applyTo: 'b4savebutton', selector: '#receptionCitizensGrid' },
                { name: 'Gkh.Orgs.LocalGov.Delete', applyTo: 'b4deletecolumn', selector: 'localGovGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    mainView: 'localgov.Grid',
    mainViewSelector: 'localGovGrid',

    refs: [
    {
        ref: 'mainView',
        selector: 'localGovGrid'
    }],

    index: function () {
        var view = this.getMainView() || Ext.widget('localGovGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('LocalGovernment').load();
    }
});