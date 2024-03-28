Ext.define('B4.view.wizard.package.BasePackagesStepFrame', {
    extend: 'B4.view.wizard.WizardBaseStepFrame',

    wizard: undefined,

    requires: [
        'B4.view.package.PackageGrid'
    ],

    stepId: 'packagesPreview',
    title: 'Пакеты для отправки',
    layout: 'fit',

    initComponent: function () {
        var me = this;

        me.items = [
            {
                xtype: 'packagegrid',
                // title: 'Пакеты данных для отправки',
                flex: 1,
                name: 'PackageGrid',
                needSign: me.wizard.needSign,
                listeners: {
                    signingStart: function () {
                        me.wizard.mask('Подписывание пакетов ...');
                    },
                    signingComplete: function () {
                        me.fireEvent('selectionchange', me);
                        me.wizard.unmask();
                    },
                    packageSigning: function (packageParams) {
                        var text = packageParams ? packageParams.name : 'пакетов';
                        me.wizard.mask('Подписывание ' + text + ' ...');
                    },
                    scope: me
                }
            }
        ];

        me.callParent(arguments);
    },

    init: function () {
        var me = this,
            packages = me.wizard.packages,
            hasPackages = packages && packages.length !== 0,
            packageGrid = me.down('b4grid[name=PackageGrid]'),
            packageGridStore = packageGrid.getStore(),
            recordsCount = packageGridStore.getCount();

        if (recordsCount === 0 && hasPackages === true) {
            packageGridStore.loadData(packages);
        }
    },

    allowForward: function () {

        var me = this,
            result = true;

        if (me.wizard.needSign) {
            //проверяем подписаны ли все пакеты
            var packageGrid = me.down('b4grid[name=PackageGrid]');

            packageGrid.getStore().each(function (rec) {
               // var signedDataLength = rec.get('SignedDataLength');
                var signed = rec.get('Signed');

                //if (!signedDataLength) {
                if (!signed) {
                    result = false;
                    return false;
                }
            });
        }

        return result;
    }
});
