using System;
using System.Collections.Generic;
using System.Text;

class GameObject {
    protected int OBJECT_ID;

    public int GetObjectID() {
        return OBJECT_ID;
    }

    public void SetObjectID(int object_ID) {
        this.OBJECT_ID = object_ID;
    }
}
