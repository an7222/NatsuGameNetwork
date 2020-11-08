using System;
using System.Collections.Generic;
using System.Text;

class Singleton<T> where T : new() {
    static T instance = default;

    public static T GetInstance() {
        if (instance == null) {
            instance = new T();
        }

        return instance;
    }
}