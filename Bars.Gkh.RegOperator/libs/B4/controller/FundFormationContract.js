Ext.define('B4.controller.FundFormationContract', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.form.SelectField',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    
    stores: [
        'FundFormationContract'
    ],
    
    models: [
        'FundFormationContract'
    ],
    
    views: [
        'fundformatcontr.EditWindow',
        'fundformatcontr.Grid'
    ],
    
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'fundFormatContrGridAspect',
            gridSelector: 'fundformatcontrgrid',
            editFormSelector: 'fundformatcontrwin',
            storeName: 'FundFormationContract',
            modelName: 'FundFormationContract',
            editWindowView: 'fundformatcontr.EditWindow',
            onSaveSuccess: function(asp, rec) {
                this.closeWindowHandler(this.getForm());
            },
            updateGrid: function () {
                this.getGrid().getStore().load();
            }
        }
    ],
    
    refs: [
        { ref: 'mainPanel', selector: 'fundformatcontrgrid' }
    ],

    index: function() {
        var view = this.getMainPanel() || Ext.widget('fundformatcontrgrid');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});