using System;
using System.Collections.Generic;
using System.Text;

class GameObject {
    protected int object_ID;

    public int GetObjectID() {
        return object_ID;
    }

    public void SetObjectID(int object_ID) {
        this.object_ID = object_ID;
    }
}
