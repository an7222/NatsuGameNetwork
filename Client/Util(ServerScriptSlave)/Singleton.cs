using System;
using System.Collections.Generic;
using System.Text;

class Singleton<T> where T : new() {
    static Lazy<T> instance = new Lazy<T>(() => new T());

    public static T GetInstance() {
        return instance.Value;
    }
}