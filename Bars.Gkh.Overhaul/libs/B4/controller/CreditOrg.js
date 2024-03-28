Ext.define('B4.controller.CreditOrg', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhGridEditForm'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['CreditOrg'],
    stores: ['CreditOrg', 'creditorg.ExceptChildren'],
    views: [
        'creditorg.Grid',
        'creditorg.EditWindow'
    ],

    mainView: 'creditorg.Grid',
    mainViewSelector: 'creditorggrid',

    refs: [{
        ref: 'mainView',
        selector: 'creditorggrid'
    }],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            name: 'permissions',
            permissionGrant: [],
            permissions: [
                { name: 'Ovrhl.CreditOrg.Create', applyTo: 'b4addbutton', selector: 'creditorggrid' },
                { name: 'Ovrhl.CreditOrg.Edit', applyTo: 'b4savebutton', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Delete', applyTo: 'b4deletecolumn', selector: 'creditorggrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                
                { name: 'Ovrhl.CreditOrg.Fields.Name', applyTo: '#tfName', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.Parent', applyTo: '#cbIsFilial', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.Parent', applyTo: '#sfParent', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.Inn', applyTo: '#tfInn', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.Kpp', applyTo: '#tfKpp', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.Bik', applyTo: '#tfBik', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.Okpo', applyTo: '#tfOkpo', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.CorrAccount', applyTo: '#tfCorrAccount', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.Address', applyTo: '#fsaAddress', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.Address', applyTo: '#cbIsAddressOut', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.Address', applyTo: '#tfAddressOutSubject', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.MailingAddress', applyTo: '#fsaMailingAddress', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.MailingAddress', applyTo: '#cbIsMailingAddressOut', selector: 'creditorgwindow' },
                { name: 'Ovrhl.CreditOrg.Fields.MailingAddress', applyTo: '#tfMailingAddressOutSubject', selector: 'creditorgwindow' }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'gridWindowAspect',
            gridSelector: 'creditorggrid',
            editFormSelector: 'creditorgwindow',
            storeName: 'CreditOrg',
            modelName: 'CreditOrg',
            deleteWithRelatedEntities: true,
            editWindowView: 'creditorg.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var permissions = asp.controller.getAspect('permissions').permissionGrant;
                    form.down('#tfAddressOutSubject').setDisabled(!permissions['creditorgwindow #tfAddressOutSubject'] || !record.get('IsAddressOut'));
                    form.down('#fsaAddress').setDisabled(!permissions['creditorgwindow #fsaAddress'] || record.get('IsAddressOut'));
                    
                    form.down('#tfMailingAddressOutSubject').setDisabled(!permissions['creditorgwindow #tfMailingAddressOutSubject'] || !record.get('IsMailingAddressOut'));
                    form.down('#fsaMailingAddress').setDisabled(!permissions['creditorgwindow #fsaMailingAddress'] || record.get('IsMailingAddressOut'));
                    
                    form.down('#sfParent').setDisabled(!permissions['creditorgwindow #sfParent'] || !record.get('IsFilial'));
                }
            },
            otherActions: function(actions) {
                actions['creditorgwindow #sfParent'] = {
                    'beforeload': {
                        fn: function(store, operation) {
                            operation.params = operation.params || {};
                            operation.params.currOrgId = this.getForm().getRecord().get('Id');
                        },
                        scope: this
                    },
                    'select': {
                        fn: function(field, data) {
                            if (data) {
                                var form = field.up('creditorgwindow');

                                if (!form.down('#tfName').getValue()) {
                                    form.down('#tfName').setValue(data['Name']);
                                }
                                form.down('#tfInn').setValue(data['Inn']);
                                form.down('#tfKpp').setValue(data['Kpp']);
                                form.down('#tfBik').setValue(data['Bik']);
                                form.down('#tfOkpo').setValue(data['Okpo']);
                                form.down('#cbIsAddressOut').setValue(data['IsAddressOut']);
                                if (data['IsAddressOut']) {
                                    form.down('#tfAddressOutSubject').setValue(data['AddressOutSubject']);
                                } else {
                                    data['FiasAddress'].Id = 0;
                                    form.down('#fsaAddress').setValue(data['FiasAddress']);
                                }
                            }
                        },
                        scope: this
                    }
                };
                actions['creditorgwindow #cbIsAddressOut'] = {
                    'change': {
                        fn: function (cmp, newValue) {
                            var permissions = this.controller.getAspect('permissions').permissionGrant;
                            this.getForm().down('#tfAddressOutSubject').setDisabled(!permissions['creditorgwindow #tfAddressOutSubject'] || !newValue);
                            this.getForm().down('#fsaAddress').setDisabled(!permissions['creditorgwindow #fsaAddress'] || newValue);
                        },
                        scope: this
                    }
                };
                actions['creditorgwindow #cbIsMailingAddressOut'] = {
                    'change': {
                        fn: function (cmp, newValue) {
                            var permissions = this.controller.getAspect('permissions').permissionGrant;
                            this.getForm().down('#tfMailingAddressOutSubject').setDisabled(!permissions['creditorgwindow #tfMailingAddressOutSubject'] || !newValue);
                            this.getForm().down('#fsaMailingAddress').setDisabled(!permissions['creditorgwindow #fsaMailingAddress'] || newValue);
                        },
                        scope: this
                    }
                };
                actions['creditorgwindow #cbIsFilial'] = {
                    'change': {
                        fn: function (cmp, newValue) {
                            var permissions = this.controller.getAspect('permissions').permissionGrant;
                            this.getForm().down('#sfParent').setDisabled(!permissions['creditorgwindow #sfParent'] || !newValue);
                        },
                        scope: this
                    }
                };
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'buttonExportAspect',
            gridSelector: 'creditorggrid',
            buttonSelector: 'creditorggrid #btnExport',
            controllerName: 'CreditOrg',
            actionName: 'Export'
        }
    ],
    index: function () {
        var view = this.getMainView() || Ext.widget('creditorggrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('CreditOrg').load();
    }
});