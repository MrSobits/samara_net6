Ext.define('B4.controller.dict.PrivilegedCategory', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['dict.PrivilegedCategory'],
    stores: ['dict.PrivilegedCategory'],
    views: [
        'dict.privilegedcategory.Grid',
        'dict.privilegedcategory.EditWindow'
    ],

    mainView: 'dict.privilegedcategory.Grid',
    mainViewSelector: 'privilegedcategoryGrid',

    mixins: { context: 'B4.mixins.Context' },

    refs: [{
        ref: 'mainView',
        selector: 'privilegedcategoryGrid'
    }],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRegOp.Dictionaries.PrivilegedCategory.Create', applyTo: 'b4addbutton', selector: 'privilegedcategoryGrid' },
                { name: 'GkhRegOp.Dictionaries.PrivilegedCategory.Edit', applyTo: 'b4savebutton', selector: '#privilegedcategoryEditWindow' },
                {
                    name: 'GkhRegOp.Dictionaries.PrivilegedCategory.Delete', applyTo: 'b4deletecolumn', selector: 'privilegedcategoryGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'privilegedcategoryGridWindowAspect',
            gridSelector: 'privilegedcategoryGrid',
            editFormSelector: '#privilegedcategoryEditWindow',
            storeName: 'dict.PrivilegedCategory',
            modelName: 'dict.PrivilegedCategory',
            editWindowView: 'dict.privilegedcategory.EditWindow',
            otherActions: function(actions) {
                var me = this;
                actions['#privilegedcategoryEditWindow [name=HasLimitArea]'] = { 'change': { fn: me.onChangeLimitAreaChkBox, scope: me } };
            },
            onChangeLimitAreaChkBox: function (chkBox, newValue) {
                var me = this,
                    form = me.getForm(),
                    record = form.getRecord(),
                    limitAreaField = form.down('[name=LimitArea]');

                limitAreaField.setDisabled(!newValue);
                limitAreaField.allowBlank = !newValue;

                if (newValue == false) {
                    limitAreaField.setValue(null);
                    record.set('LimitArea', null);
                }

                limitAreaField.validate();
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    var me = this,
                        form = me.getForm(),
                        hasLimitAreaField = form.down('[name=HasLimitArea]');

                    hasLimitAreaField.setValue(record.get('LimitArea') != null);
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView();
        if (!view) {
            view = Ext.widget('privilegedcategoryGrid');
            me.bindContext(view);
            me.application.deployView(view);
            me.getStore('dict.PrivilegedCategory').load();
        }
    }
});