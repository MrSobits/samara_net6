Ext.define('B4.aspects.permission.specialobjectcr.ContractCrType', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.contractspecialobjectcrtypeperm',

    permissions: [
     {
         name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.Expertise',
         applyTo: 'b4combobox[name=TypeContractObject]',
         selector: 'specialobjectcrcontractwin',
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
         name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.RoMoAggreement',
         applyTo: 'b4combobox[name=TypeContractObject]',
         selector: 'specialobjectcrcontractwin',
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
         name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.BuildingControl',
         applyTo: 'b4combobox[name=TypeContractObject]',
         selector: 'specialobjectcrcontractwin',
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
         name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.TechSepervision',
         applyTo: 'b4combobox[name=TypeContractObject]',
         selector: 'specialobjectcrcontractwin',
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
         name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.Psd',
         applyTo: 'b4combobox[name=TypeContractObject]',
         selector: 'specialobjectcrcontractwin',
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
         name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.Insurance',
         applyTo: 'b4combobox[name=TypeContractObject]',
         selector: 'specialobjectcrcontractwin',
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