:: Установить приложение как инстанс(чтобы иметь копии одного и того же приложения для разных регионов на одной машине)
:: Пример msiexec /i Setup.msi MSINEWINSTANCE=1 TRANSFORMS=":InstTatarstan"
msiexec /i Setup.msi MSINEWINSTANCE=1 TRANSFORMS=":<InstanceId>" INSTALLLOCATION="C:\Program Files (x86)\BARS Gkh - Tatarstan" ADDLOCAL=GkhAppFeature /qb


:: Обновить инстанс на новую версию
:: Пример msiexec /i Setup.msi /n {27053071-A5F2-4D21-BBD3-5F1EB071B630} REINSTALL=ALL REINSTALLMODE=vomus /qb
msiexec /i Setup.msi /n {<Instance Product Code>} REINSTALL=ALL REINSTALLMODE=vomus /qb