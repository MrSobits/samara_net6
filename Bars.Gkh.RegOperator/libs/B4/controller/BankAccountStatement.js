Ext.define('B4.controller.BankAccountStatement', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhGridPermissionAspect',

        'B4.view.bankaccountstatement.GroupEditWindow',
        'B4.view.bankaccountstatement.GroupGrid'
    ],

    models: [
        'BankAccountStatementGroup',
        'BankAccountStatement'
    ],

    stores: [],

    views: [
        'bankaccountstatement.GroupEditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'bankaccstatementgroupgrid'
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'bankaccstatementgrdaspect',
            gridSelector: 'bankaccstatementgroupgrid',
            editFormSelector: 'bankaccountstatementgroupeditwindow',
            //storeName: 'CreditOrgServiceCondition',
            modelName: 'BankAccountStatementGroup',
            editWindowView: 'bankaccountstatement.GroupEditWindow',
            listeners: {
                aftersetformdata: function(asp, record, form) {
                    var statementStore = form.down('bankaccstatementgrid').getStore(),
                        id = record.get('Id');
                    statementStore.on('beforeload', function(store, operation) {
                        operation.params = operation.params || {};
                        operation.params.groupId = id;
                    });

                    statementStore.load();
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('bankaccstatementgroupgrid');
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});