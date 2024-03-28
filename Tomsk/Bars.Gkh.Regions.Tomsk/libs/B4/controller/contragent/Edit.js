Ext.define('B4.controller.contragent.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'Contragent'
    ],

    views: [
        'contragent.EditPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'contragent.EditPanel',
    mainViewSelector: 'contragentEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'contragentEditPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.Contragent.Edit', applyTo: 'b4savebutton', selector: 'contragentEditPanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'contragentEditPanelAspect',
            editPanelSelector: 'contragentEditPanel',
            modelName: 'Contragent',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' button'] = { 'click': { fn: this.btnClick, scope: this} };
                actions[this.editPanelSelector + ' #tfcontragentOutsideAddress'] = { 'change': { fn: this.onChangeAddressOutside, scope: this} };
                actions[this.editPanelSelector + ' #sfCtrgOrgForm'] = { 'change': { fn: this.onChangeOrgForm, scope: this } };
                actions[this.editPanelSelector + ' #sfParent'] = { 'beforeload': { fn: this.onBeforeLoadParent, scope: this } };
            },
            disableField: function (fieldSelector, disabled) {
                var field = this.getPanel().down(fieldSelector);
                field.allowBlank = disabled;
                field.setDisabled(disabled);
            },
            onBeforeLoadParent: function(store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.contragentId = this.getContextValue(this.getMainComponent(), 'contragentId');
            },
            onChangeOrgForm: function (field, newValue) {
                var form = this.getPanel();
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
            onChangeAddressOutside: function (field, newValue) {
                var panel = this.getPanel(),
                    factAddressField = panel.down('b4fiasselectaddress[name=FiasFactAddress]'),
                    jurAddressField = panel.down('b4fiasselectaddress[name=FiasJuridicalAddress]');

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

            btnClick: function (btn) {
                var pasteField = null;

                if (btn.itemId == 'btnCopyButtonFactAddress') {
                    pasteField = Ext.ComponentQuery.query('#contragentFiasFactAddressField')[0];
                }
                else if (btn.itemId == 'btnCopyButtonAddressOutsideSubject') {
                    pasteField = Ext.ComponentQuery.query('#contragentFiasAddressOutsideSubjectField')[0];
                }
                else if (btn.itemId == 'btnCopyButtonMailingAddress') {
                    pasteField = Ext.ComponentQuery.query('#contragentFiasMailingAddressField')[0];
                }

                if (pasteField)
                    this.copyPaste(pasteField);
            },

            copyPaste: function (pasteField) {
                var jurAddressField = Ext.ComponentQuery.query('#contragentFiasJuridicalAddressField')[0];
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
            listeners: {
                savesuccess: function (asp, rec) {
                    asp.setData(rec.getId());
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentEditPanel');

        me.bindContext(view);
        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view, 'contragent_info');

        me.getAspect('contragentEditPanelAspect').setData(id);
    }
});