using System;
using System.Collections.Generic;
using System.Text;

class FieldController {
    class FieldInstance{
    }

    public FieldController() {
        FieldInstance fieldInstance = new FieldInstance();

        FieldProcess();
    }

    void FieldProcess() {
        ThreadManager.GetInstance().RegisterWork(() => {
            while (true) {

            }
        });
    }
}
