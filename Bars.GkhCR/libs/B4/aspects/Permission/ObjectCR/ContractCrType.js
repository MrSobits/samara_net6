Ext.define('B4.aspects.permission.objectcr.ContractCrType', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.contractcrtypeperm',

    permissions: [
     {
         name: 'GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.Expertise',
         applyTo: '#cbTypeContractObject',
         selector: 'objectcrcontractwin',
         applyBy: function (component, allowed) {
             var str = component.getStore();
             if (!allowed) {
                 str.filter(
                 { id: 'Expertise', filterFn: function (item) { return item.get('Value') != 'Экспертиза'; } });
             } else {
                 str.filters.removeAtKey('Expertise');
             }
         }
     },
     {
         name: 'GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.RoMoAggreement',
         applyTo: '#cbTypeContractObject',
         selector: 'objectcrcontractwin',
         applyBy: function (component, allowed) {
             var str = component.getStore();
             if (!allowed) {
                 str.filter(
                 { id: 'RoMoAggreement', filterFn: function (item) { return item.get('Value') != 'Договор о функции заказчика между РО и МО'; } });
             } else {
                 str.filters.removeAtKey('RoMoAggreement');
             }
         }
     },
     {
         name: 'GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.BuildingControl',
         applyTo: '#cbTypeContractObject',
         selector: 'objectcrcontractwin',
         applyBy: function (component, allowed) {
             var str = component.getStore();
             if (!allowed) {
                 str.filter(
                 { id: 'BuildingControl', filterFn: function (item) { return item.get('Value') != 'Строительный контроль'; } });
             } else {
                 str.filters.removeAtKey('BuildingControl');
             }
         }
     },
     {
         name: 'GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.TechSepervision',
         applyTo: '#cbTypeContractObject',
         selector: 'objectcrcontractwin',
         applyBy: function (component, allowed) {
             var str = component.getStore();
             if (!allowed) {
                 str.filter(
                 { id: 'TechSepervision', filterFn: function (item) { return item.get('Value') != 'Технический надзор'; } });
             } else {
                 str.filters.removeAtKey('TechSepervision');
             }
         }
     },
     {
         name: 'GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.Psd',
         applyTo: '#cbTypeContractObject',
         selector: 'objectcrcontractwin',
         applyBy: function (component, allowed) {
             var str = component.getStore();
             if (!allowed) {
                 str.filter(
                 { id: 'Psd', filterFn: function (item) { return item.get('Value') != 'ПСД'; } });
             } else {
                 str.filters.removeAtKey('Psd');
             }
         }
     },
     {
         name: 'GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.Insurance',
         applyTo: '#cbTypeContractObject',
         selector: 'objectcrcontractwin',
         applyBy: function (component, allowed) {
             var str = component.getStore();
             if (!allowed) {
                 str.filter(
                 { id: 'Insurance', filterFn: function (item) { return item.get('Value') != 'Страхование'; } });
             } else {
                 str.filters.removeAtKey('Insurance');
             }
         }
     }
    ]
});