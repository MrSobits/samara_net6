Ext.define('B4.aspects.GkhManyGridEditWindow', {
    extend: 'B4.aspects.GridEditWindow',

    alias: 'widget.gkhmanygrideditwindow',

    editWindowView: null,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.on('aftersetformdata', this.onAfterSetFormData, this);

        this.on('beforegridaction', this.onBeforeGridAction, this);
        this.on('beforerowaction', this.onBeforeRowAction, this);

        this.on('savesuccess', this.onSaveSuccess, this);
    },

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.editFormSelector + ' b4closebutton'] = {
            'click': {
                fn: this.closeWindowHandler,
                scope: this
            }
        };

        for (var i = 0; i < this.gridSelectors.length; i++) {
            actions[this.gridSelectors[i]] = {
                'rowaction': {
                    fn: this.rowAction,
                    scope: this
                },
                'itemdblclick': {
                    fn: this.rowDblClick,
                    scope: this
                },
                'gridaction': {
                    fn: this.gridAction,
                    scope: this
                }
            };

            actions[this.gridSelectors[i] + ' b4addbutton'] = {
                'click': {
                    fn: this.btnAction,
                    scope: this
                }
            },

            actions[this.gridSelectors[i] + ' b4updatebutton'] = {
                'click': {
                    fn: this.btnAction,
                    scope: this
                }
            };
        }
        controller.control(actions);
    },

    btnAction: function (btn) {
        this.gridSelector = '#' + btn.up("#toolbarPayment").ownerCt.itemId;
        this.storeName = btn.up("#toolbarPayment").ownerCt.getStore().storeId;
        this.typePayment = btn.up("#toolbarPayment").ownerCt.getStore().typePayment;
        this.getGrid().fireEvent('gridaction', this.getGrid(), btn.actionName);
    },

    /*closeWindowHandler: function () {
        this.getForm().close();
    },*/

    /*getForm: function () {
        var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

        if (!editWindow) {
            editWindow = this.controller.getView(this.editWindowView).create();
        }

        return editWindow;
    },*/

    onAfterSetFormData: function (aspect, rec, form) {
        form.show();
    },

    /*onSaveSuccess: function (aspect) {
        aspect.getForm().close();
    },*/

    onBeforeGridAction: function (aspect, grid) {
        this.gridSelector = '#' + grid.itemId;
    },

    onBeforeRowAction: function (aspect, grid) {
        this.gridSelector = '#' + grid.itemId;
    }
});