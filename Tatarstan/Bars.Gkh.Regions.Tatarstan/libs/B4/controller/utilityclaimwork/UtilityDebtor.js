Ext.define('B4.controller.utilityclaimwork.UtilityDebtor', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.view.utilityclaimwork.Grid',
        'B4.aspects.GkhGridEditForm',
        'B4.controller.claimwork.Navi',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhButtonImportAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'utilityclaimwork.UtilityDebtor'
    ],

    stores: [
         'utilityclaimwork.UtilityDebtor'
    ],

    views: [
        'utilityclaimwork.Grid',
        'utilityclaimwork.UtilityDebtorAddWindow',
        'utilityclaimwork.UtilityDebtorImportWindow'
    ],

    refs: [
       {
           ref: 'mainView',
           selector: 'utilitydebtorclaimworkgrid'
       }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.UtilityDebtor.Add', applyTo: 'b4addbutton', selector: 'utilitydebtorclaimworkgrid'
                },
                {
                    name: 'Clw.ClaimWork.UtilityDebtor.Import', applyTo: 'gkhbuttonimport', selector: 'utilitydebtorclaimworkgrid'
                }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            gridSelector: 'utilitydebtorclaimworkgrid',
            editFormSelector: 'utilitydebtoraddwin',
            storeName: 'utilityclaimwork.UtilityDebtor',
            modelName: 'utilityclaimwork.UtilityDebtor',
            editWindowView: 'utilityclaimwork.UtilityDebtorAddWindow',
            controllerEditName: 'B4.controller.claimwork.Navi',
            updateGrid: function() {
                this.getGrid().getStore().load();
            },
            editRecord: function(record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('claimwork/{0}/{1}', 'UtilityDebtorClaimWork', id));
                    } else {
                        model.load(id, {
                            success: function(rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        },
        {
            /*
            *аспект для импорта
            */
            xtype: 'gkhbuttonimportaspect',
            name: 'utilityDebtorImportAspect',
            loadImportList: false,
            buttonSelector: 'utilitydebtorclaimworkgrid gkhbuttonimport',
            windowImportView: 'utilityclaimwork.UtilityDebtorImportWindow',
            windowImportSelector: 'utilitydebtorimportwindow',
            maxFileSize: 5242880
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('utilitydebtorclaimworkgrid');
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});