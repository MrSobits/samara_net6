Ext.define('B4.controller.Contragent', {
    extend: 'B4.base.Controller',
    requires: [ 
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    models: ['Contragent'],
    stores: ['Contragent'],
    views: ['contragent.Grid', 'contragent.AddWindow'],

    mainView: 'contragent.Grid',
    mainViewSelector: 'contragentGrid',

    refs: [{
        ref: 'mainView',
        selector: 'contragentGrid'
    }],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.Contragent.Create', applyTo: 'b4addbutton', selector: 'contragentGrid' },
                { name: 'Gkh.Orgs.Contragent.Delete', applyTo: 'b4deletecolumn',  selector: 'contragentGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'contragentGridWindowAspect',
            gridSelector: 'contragentGrid',
            editFormSelector: '#contragentAddWindow',
            storeName: 'Contragent',
            modelName: 'Contragent',
            editWindowView: 'contragent.AddWindow',
            controllerEditName: 'B4.controller.contragent.Navi',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' #btnCopyButtonFactAddress'] = { 'click': { fn: this.btnClick, scope: this } };
                actions[this.editFormSelector + ' #tfctrgAddWindowOutsideAddress'] = { 'change': { fn: this.onChangeOutsideAddress, scope: this } };
                actions[this.editFormSelector + ' #sfCtrgOrgForm'] = { 'change': { fn: this.onChangeOrgForm, scope: this } };
                
            },
            disableField: function (fieldSelector, disabled) {
                var form = this.getForm();
                form.down(fieldSelector).allowBlank = disabled;
                form.down(fieldSelector).setDisabled(disabled);
            },
            onChangeOrgForm: function (field, newValue) {
                var form = this.getForm();
                
                if (newValue) {
                    if (newValue.Code == '98') {
                        this.disableField('#tfCtrgInn', true);
                        this.disableField('#tfCtrgKpp', true);
                        //this.disableField('#tfCtrgOgrn', true);
                    } else {
                        this.disableField('#tfCtrgInn', false);
                        newValue.Code = newValue.Code.replace(new RegExp(" ", 'g'), " "); // замена пустого символа на пробел
                        newValue.Code == '91' || newValue.Code == '5 01 02' ? form.down('#tfCtrgKpp').allowBlank = true : this.disableField('#tfCtrgKpp', false);
                        //this.disableField('#tfCtrgOgrn', false);
                    }
                }
            },
            onChangeOutsideAddress: function (field, newValue) {
                var form = this.getForm(),
                    factAddressField = form.down('b4fiasselectaddress[name=FiasFactAddress]'),
                    jurAddressField = form.down('b4fiasselectaddress[name=FiasJuridicalAddress]');

                if (newValue) {
                    factAddressField.allowBlank = true;
                    jurAddressField.allowBlank = true;
                } else {
                    factAddressField.allowBlank = false;
                    jurAddressField.allowBlank = false;
                }

                jurAddressField.isValid();
                factAddressField.isValid();

            },
            btnClick: function () {
                var form = this.getForm(),
                    pasteField = form.down('b4fiasselectaddress[name=FiasFactAddress]'),
                    jurAddressField = form.down('b4fiasselectaddress[name=FiasJuridicalAddress]');

                var copy = jurAddressField.getValue();

                if (!copy) {
                    Ext.Msg.alert('Внимание', 'Необходимо заполнить юридический адрес!');
                    return;
                }

                var currAdtr = pasteField.getValue();
                var newadrr = {
                    Id: currAdtr ? currAdtr.Id : 0,
                    AddressName: copy.AddressName,
                    PlaceCode: copy.PlaceCode,
                    PlaceGuidId: copy.PlaceGuidId,
                    PlaceName: copy.PlaceName,
                    PlaceAddressName: copy.PlaceAddressName,
                    StreetCode: copy.StreetCode,
                    StreetGuidId: copy.StreetGuidId,
                    StreetName: copy.StreetName,
                    House: copy.House,
                    Housing: copy.Housing,
                    Building: copy.Building,
                    Flat: copy.Flat,
                    Coordinate: copy.Coordinate,
                    PostCode: copy.PostCode
                };

                pasteField.setValue(newadrr);
            },
            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                    }
                }
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.get('Id') : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('contragentedit/{0}', id));
                    } else {
                        model.load(id, {
                            success: function (rec) {
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
            xtype: 'b4buttondataexportaspect',
            name: 'contragentButtonExportAspect',
            gridSelector: 'contragentGrid',
            buttonSelector: 'contragentGrid #btnExport',
            controllerName: 'Contragent',
            actionName: 'Export'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('contragentGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('Contragent').load();
    },
    
    init: function () {
        this.getStore('Contragent').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },
    
    onBeforeLoad: function (store, operation) {
        if (!operation.params) {
            operation.params = {};
        }
        operation.params.showAll = true;

    }
});