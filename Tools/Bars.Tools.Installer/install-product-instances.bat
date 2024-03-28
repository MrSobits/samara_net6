:: Пример как поставить 2 региона(Татарстан и Архангельск) и как их обновлять

:: Первоначальная установка инстансов продукта
msiexec /i C:\Users\Administrator\Desktop\setup1.msi MSINEWINSTANCE=1 TRANSFORMS=":InstTatarstan" INSTALLLOCATION="C:\Program Files (x86)\BARS Gkh - Tatarstan" ADDLOCAL=GkhAppFeature SERVER_NAME="192.168.228.143" DB_NAME="test_rt" LOGIN="bars" PASSWORD="123" /qb
msiexec /i C:\Users\Administrator\Desktop\setup1.msi MSINEWINSTANCE=1 TRANSFORMS=":InstArkhangelsk" INSTALLLOCATION="C:\Program Files (x86)\BARS Gkh - Arkhangelsk" ADDLOCAL=GkhAppFeature SERVER_NAME="192.168.228.143" DB_NAME="test_arhangelsk" LOGIN="bars" PASSWORD="123" /qb

:: Обновление инстансов продукта
msiexec /i C:\Users\Administrator\Desktop\setup2.msi /n {27053071-A5F2-4D21-BBD3-5F1EB071B630}  REINSTALL=ALL REINSTALLMODE=vomus
msiexec /i C:\Users\Administrator\Desktop\setup2.msi /n {CCE8AEB5-2BCF-4984-BD3C-0C492C1F7BB8} REINSTALL=ALL REINSTALLMODE=vomus

